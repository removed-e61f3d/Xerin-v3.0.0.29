#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;
using KoiVM.Core.RT;
using KoiVM.Core.VM;

namespace KoiVM.Core.VMIR;

public class IRTranslator
{
	private static readonly Dictionary<Code, ITranslationHandler> handlers;

	public ScopeBlock RootScope { get; private set; }

	public IRContext Context { get; }

	public VMRuntime Runtime { get; }

	public VMDescriptor VM => Runtime.Descriptor;

	public ArchDescriptor Arch => VM.Architecture;

	internal BasicBlock<ILASTTree> Block { get; private set; }

	internal IRInstrList Instructions { get; private set; }

	static IRTranslator()
	{
		handlers = new Dictionary<Code, ITranslationHandler>();
		Type[] exportedTypes = typeof(IRTranslator).Assembly.GetExportedTypes();
		foreach (Type type in exportedTypes)
		{
			if (typeof(ITranslationHandler).IsAssignableFrom(type) && !type.IsAbstract)
			{
				ITranslationHandler translationHandler = (ITranslationHandler)Activator.CreateInstance(type);
				handlers.Add(translationHandler.ILCode, translationHandler);
			}
		}
	}

	public IRTranslator(IRContext ctx, VMRuntime runtime)
	{
		Context = ctx;
		Runtime = runtime;
	}

	internal IIROperand Translate(IILASTNode node)
	{
		if (node is ILASTExpression)
		{
			ILASTExpression iLASTExpression = (ILASTExpression)node;
			try
			{
				if (!handlers.TryGetValue(iLASTExpression.ILCode, out var value))
				{
					throw new NotSupportedException(iLASTExpression.ILCode.ToString());
				}
				int i = Instructions.Count;
				IIROperand result = value.Translate(iLASTExpression, this);
				for (; i < Instructions.Count; i++)
				{
					Instructions[i].ILAST = iLASTExpression;
				}
				return result;
			}
			catch (Exception innerException)
			{
				throw new Exception($"Failed to translate expr {iLASTExpression.CILInstr} @ {iLASTExpression.CILInstr.GetOffset():x4}.", innerException);
			}
		}
		if (node is ILASTVariable)
		{
			return Context.ResolveVRegister((ILASTVariable)node);
		}
		throw new NotSupportedException();
	}

	private IRInstrList Translate(BasicBlock<ILASTTree> block)
	{
		Block = block;
		Instructions = new IRInstrList();
		bool flag = false;
		foreach (IILASTStatement item in block.Content)
		{
			if (item is ILASTPhi)
			{
				ILASTVariable variable = ((ILASTPhi)item).Variable;
				Instructions.Add(new IRInstruction(IROpCode.POP)
				{
					Operand1 = Context.ResolveVRegister(variable),
					ILAST = item
				});
				continue;
			}
			if (item is ILASTAssignment)
			{
				ILASTAssignment iLASTAssignment = (ILASTAssignment)item;
				IIROperand operand = Translate(iLASTAssignment.Value);
				Instructions.Add(new IRInstruction(IROpCode.MOV)
				{
					Operand1 = Context.ResolveVRegister(iLASTAssignment.Variable),
					Operand2 = operand,
					ILAST = item
				});
				continue;
			}
			if (item is ILASTExpression)
			{
				ILASTExpression iLASTExpression = (ILASTExpression)item;
				OpCode opCode = iLASTExpression.ILCode.ToOpCode();
				if (!flag && (opCode.FlowControl == FlowControl.Cond_Branch || opCode.FlowControl == FlowControl.Branch || opCode.FlowControl == FlowControl.Return || opCode.FlowControl == FlowControl.Throw))
				{
					ILASTVariable[] stackRemains = block.Content.StackRemains;
					foreach (ILASTVariable variable2 in stackRemains)
					{
						Instructions.Add(new IRInstruction(IROpCode.PUSH)
						{
							Operand1 = Context.ResolveVRegister(variable2),
							ILAST = item
						});
					}
					flag = true;
				}
				Translate((ILASTExpression)item);
				continue;
			}
			throw new NotSupportedException();
		}
		Debug.Assert(flag);
		IRInstrList instructions = Instructions;
		Instructions = null;
		return instructions;
	}

	public void Translate(ScopeBlock rootScope)
	{
		RootScope = rootScope;
		Dictionary<BasicBlock<ILASTTree>, BasicBlock<IRInstrList>> blockMap = rootScope.UpdateBasicBlocks((BasicBlock<ILASTTree> block) => Translate(block));
		rootScope.ProcessBasicBlocks(delegate(BasicBlock<IRInstrList> block)
		{
			foreach (IRInstruction item in block.Content)
			{
				if (item.Operand1 is IRBlockTarget)
				{
					IRBlockTarget iRBlockTarget = (IRBlockTarget)item.Operand1;
					iRBlockTarget.Target = blockMap[(BasicBlock<ILASTTree>)iRBlockTarget.Target];
				}
				else if (item.Operand1 is IRJumpTable)
				{
					IRJumpTable iRJumpTable = (IRJumpTable)item.Operand1;
					for (int i = 0; i < iRJumpTable.Targets.Length; i++)
					{
						iRJumpTable.Targets[i] = blockMap[(BasicBlock<ILASTTree>)iRJumpTable.Targets[i]];
					}
				}
				if (item.Operand2 is IRBlockTarget)
				{
					IRBlockTarget iRBlockTarget2 = (IRBlockTarget)item.Operand2;
					iRBlockTarget2.Target = blockMap[(BasicBlock<ILASTTree>)iRBlockTarget2.Target];
				}
				else if (item.Operand2 is IRJumpTable)
				{
					IRJumpTable iRJumpTable2 = (IRJumpTable)item.Operand2;
					for (int j = 0; j < iRJumpTable2.Targets.Length; j++)
					{
						iRJumpTable2.Targets[j] = blockMap[(BasicBlock<ILASTTree>)iRJumpTable2.Targets[j]];
					}
				}
			}
		});
	}
}
