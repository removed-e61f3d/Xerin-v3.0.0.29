#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.CFG;

namespace KoiVM.Core.ILAST;

public class ILASTBuilder
{
	private struct BlockState
	{
		public ILASTVariable[] BeginStack;

		public ILASTTree ASTTree;
	}

	private MethodDef method;

	private CilBody body;

	private ScopeBlock scope;

	private IList<BasicBlock<CILInstrList>> basicBlocks;

	private Dictionary<Instruction, BasicBlock<CILInstrList>> blockHeaders;

	private Dictionary<BasicBlock<CILInstrList>, BlockState> blockStates;

	private List<ILASTExpression> instrReferences;

	private ILASTBuilder(MethodDef method, CilBody body, ScopeBlock scope)
	{
		this.method = method;
		this.body = body;
		this.scope = scope;
		basicBlocks = scope.GetBasicBlocks().Cast<BasicBlock<CILInstrList>>().ToList();
		blockHeaders = basicBlocks.ToDictionary((BasicBlock<CILInstrList> block) => block.Content[0], (BasicBlock<CILInstrList> block) => block);
		blockStates = new Dictionary<BasicBlock<CILInstrList>, BlockState>();
		instrReferences = new List<ILASTExpression>();
		Debug.Assert(basicBlocks.Count > 0);
	}

	public static void BuildAST(MethodDef method, CilBody body, ScopeBlock scope)
	{
		body.SimplifyMacros(method.Parameters);
		body.SimplifyBranches();
		ILASTBuilder iLASTBuilder = new ILASTBuilder(method, body, scope);
		List<BasicBlock<CILInstrList>> list = scope.GetBasicBlocks().Cast<BasicBlock<CILInstrList>>().ToList();
		iLASTBuilder.BuildAST();
	}

	private void BuildAST()
	{
		BuildASTInternal();
		BuildPhiNodes();
		Dictionary<BasicBlock<CILInstrList>, BasicBlock<ILASTTree>> blockMap = scope.UpdateBasicBlocks((BasicBlock<CILInstrList> block) => blockStates[block].ASTTree);
		Dictionary<Instruction, BasicBlock<ILASTTree>> newBlockMap = blockHeaders.ToDictionary((KeyValuePair<Instruction, BasicBlock<CILInstrList>> pair) => pair.Key, (KeyValuePair<Instruction, BasicBlock<CILInstrList>> pair) => blockMap[pair.Value]);
		foreach (ILASTExpression instrReference in instrReferences)
		{
			if (instrReference.Operand is Instruction)
			{
				instrReference.Operand = newBlockMap[(Instruction)instrReference.Operand];
				continue;
			}
			instrReference.Operand = ((IEnumerable<Instruction>)(Instruction[])instrReference.Operand).Select((Func<Instruction, IBasicBlock>)((Instruction instr) => newBlockMap[instr])).ToArray();
		}
	}

	private void BuildASTInternal()
	{
		Stack<BasicBlock<CILInstrList>> stack = new Stack<BasicBlock<CILInstrList>>();
		PopulateBeginStates(stack);
		HashSet<BasicBlock<CILInstrList>> hashSet = new HashSet<BasicBlock<CILInstrList>>();
		while (stack.Count > 0)
		{
			BasicBlock<CILInstrList> basicBlock = stack.Pop();
			if (hashSet.Contains(basicBlock))
			{
				continue;
			}
			hashSet.Add(basicBlock);
			Debug.Assert(blockStates.ContainsKey(basicBlock));
			BlockState value = blockStates[basicBlock];
			Debug.Assert(value.ASTTree == null);
			ILASTTree iLASTTree = BuildAST(basicBlock.Content, value.BeginStack);
			ILASTVariable[] stackRemains = iLASTTree.StackRemains;
			value.ASTTree = iLASTTree;
			blockStates[basicBlock] = value;
			foreach (BasicBlock<CILInstrList> target in basicBlock.Targets)
			{
				if (!blockStates.TryGetValue(target, out var value2))
				{
					ILASTVariable[] array = new ILASTVariable[stackRemains.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = new ILASTVariable
						{
							Name = $"ph_{target.Id:x2}_{i:x2}",
							Type = stackRemains[i].Type,
							VariableType = ILASTVariableType.PhiVar
						};
					}
					BlockState blockState = default(BlockState);
					blockState.BeginStack = array;
					value2 = blockState;
					blockStates[target] = value2;
				}
				else if (value2.BeginStack.Length != stackRemains.Length)
				{
					throw new InvalidProgramException("Inconsistent stack depth.");
				}
				stack.Push(target);
			}
		}
	}

	private void PopulateBeginStates(Stack<BasicBlock<CILInstrList>> workList)
	{
		for (int i = 0; i < body.ExceptionHandlers.Count; i++)
		{
			ExceptionHandler exceptionHandler = body.ExceptionHandlers[i];
			blockStates[blockHeaders[exceptionHandler.TryStart]] = new BlockState
			{
				BeginStack = new ILASTVariable[0]
			};
			BasicBlock<CILInstrList> basicBlock = blockHeaders[exceptionHandler.HandlerStart];
			workList.Push(basicBlock);
			if (exceptionHandler.HandlerType == ExceptionHandlerType.Fault || exceptionHandler.HandlerType == ExceptionHandlerType.Finally)
			{
				blockStates[basicBlock] = new BlockState
				{
					BeginStack = new ILASTVariable[0]
				};
				continue;
			}
			ASTType type = TypeInference.ToASTType(exceptionHandler.CatchType.ToTypeSig());
			if (!blockStates.ContainsKey(basicBlock))
			{
				ILASTVariable iLASTVariable = new ILASTVariable
				{
					Name = $"ex_{i:x2}",
					Type = type,
					VariableType = ILASTVariableType.ExceptionVar,
					Annotation = exceptionHandler
				};
				blockStates[basicBlock] = new BlockState
				{
					BeginStack = new ILASTVariable[1] { iLASTVariable }
				};
			}
			else
			{
				Debug.Assert(blockStates[basicBlock].BeginStack.Length == 1);
			}
			if (exceptionHandler.FilterStart != null)
			{
				ILASTVariable iLASTVariable2 = new ILASTVariable
				{
					Name = $"ef_{i:x2}",
					Type = type,
					VariableType = ILASTVariableType.FilterVar,
					Annotation = exceptionHandler
				};
				BasicBlock<CILInstrList> basicBlock2 = blockHeaders[exceptionHandler.FilterStart];
				workList.Push(basicBlock2);
				blockStates[basicBlock2] = new BlockState
				{
					BeginStack = new ILASTVariable[1] { iLASTVariable2 }
				};
			}
		}
		blockStates[basicBlocks[0]] = new BlockState
		{
			BeginStack = new ILASTVariable[0]
		};
		workList.Push(basicBlocks[0]);
		foreach (BasicBlock<CILInstrList> basicBlock3 in basicBlocks)
		{
			if (basicBlock3.Sources.Count <= 0 && !workList.Contains(basicBlock3))
			{
				blockStates[basicBlock3] = new BlockState
				{
					BeginStack = new ILASTVariable[0]
				};
				workList.Push(basicBlock3);
			}
		}
	}

	private void BuildPhiNodes()
	{
		foreach (KeyValuePair<BasicBlock<CILInstrList>, BlockState> blockState in blockStates)
		{
			BasicBlock<CILInstrList> key = blockState.Key;
			BlockState value = blockState.Value;
			if (key.Sources.Count == 0 && value.BeginStack.Length != 0)
			{
				Debug.Assert(value.BeginStack.Length == 1);
				ILASTPhi iLASTPhi = new ILASTPhi();
				iLASTPhi.Variable = value.BeginStack[0];
				iLASTPhi.SourceVariables = new ILASTVariable[1] { value.BeginStack[0] };
				ILASTPhi item = iLASTPhi;
				value.ASTTree.Insert(0, item);
			}
			else
			{
				if (value.BeginStack.Length == 0)
				{
					continue;
				}
				for (int i = 0; i < value.BeginStack.Length; i++)
				{
					ILASTPhi iLASTPhi2 = new ILASTPhi
					{
						Variable = value.BeginStack[i]
					};
					iLASTPhi2.SourceVariables = new ILASTVariable[key.Sources.Count];
					for (int j = 0; j < iLASTPhi2.SourceVariables.Length; j++)
					{
						iLASTPhi2.SourceVariables[j] = blockStates[key.Sources[j]].ASTTree.StackRemains[i];
					}
					value.ASTTree.Insert(0, iLASTPhi2);
				}
			}
		}
	}

	private ILASTTree BuildAST(CILInstrList instrs, ILASTVariable[] beginStack)
	{
		ILASTTree iLASTTree = new ILASTTree();
		Stack<ILASTVariable> evalStack = new Stack<ILASTVariable>(beginStack);
		Func<int, IILASTNode[]> func = delegate(int numArgs)
		{
			IILASTNode[] array = new IILASTNode[numArgs];
			for (int num = numArgs - 1; num >= 0; num--)
			{
				array[num] = evalStack.Pop();
			}
			return array;
		};
		List<Instruction> list = new List<Instruction>();
		foreach (Instruction instr in instrs)
		{
			if (instr.OpCode.OpCodeType == OpCodeType.Prefix)
			{
				list.Add(instr);
				continue;
			}
			int pushes;
			int pops;
			ILASTExpression iLASTExpression2;
			if (instr.OpCode.Code == Code.Dup)
			{
				pushes = (pops = 1);
				ILASTVariable iLASTVariable = evalStack.Peek();
				ILASTExpression iLASTExpression = new ILASTExpression();
				iLASTExpression.ILCode = Code.Dup;
				iLASTExpression.Operand = null;
				iLASTExpression.Arguments = new IILASTNode[1] { iLASTVariable };
				iLASTExpression2 = iLASTExpression;
			}
			else
			{
				instr.CalculateStackUsage(method.ReturnType.ElementType != ElementType.Void, out pushes, out pops);
				Debug.Assert(pushes == 0 || pushes == 1);
				if (pops == -1)
				{
					evalStack.Clear();
					pops = 0;
				}
				iLASTExpression2 = new ILASTExpression
				{
					ILCode = instr.OpCode.Code,
					Operand = instr.Operand,
					Arguments = func(pops)
				};
				if (iLASTExpression2.Operand is Instruction || iLASTExpression2.Operand is Instruction[])
				{
					instrReferences.Add(iLASTExpression2);
				}
			}
			iLASTExpression2.CILInstr = instr;
			if (list.Count > 0)
			{
				iLASTExpression2.Prefixes = list.ToArray();
				list.Clear();
			}
			if (pushes == 1)
			{
				ILASTVariable iLASTVariable2 = new ILASTVariable
				{
					Name = $"s_{instr.Offset:x4}",
					VariableType = ILASTVariableType.StackVar
				};
				evalStack.Push(iLASTVariable2);
				iLASTTree.Add(new ILASTAssignment
				{
					Variable = iLASTVariable2,
					Value = iLASTExpression2
				});
			}
			else
			{
				iLASTTree.Add(iLASTExpression2);
			}
		}
		iLASTTree.StackRemains = evalStack.Reverse().ToArray();
		return iLASTTree;
	}
}
