#define DEBUG
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Pdb;
using KoiVM.Core.Helpers.System;

namespace KoiVM.Core.CFG;

public class BlockParser
{
	public static ScopeBlock Parse(MethodDef method, CilBody body)
	{
		body.SimplifyMacros(method.Parameters);
		body.SimplifyBranches();
		ExpandSequencePoints(body);
		FindHeaders(body, out var headers, out var entries);
		List<BasicBlock<CILInstrList>> blocks = SplitBlocks(body, headers, entries);
		LinkBlocks(blocks);
		return AssignScopes(body, blocks);
	}

	private static void ExpandSequencePoints(CilBody body)
	{
		SequencePoint sequencePoint = null;
		foreach (Instruction instruction in body.Instructions)
		{
			if (instruction.SequencePoint != null)
			{
				sequencePoint = instruction.SequencePoint;
			}
			else
			{
				instruction.SequencePoint = sequencePoint;
			}
		}
	}

	private static void FindHeaders(CilBody body, out HashSet<Instruction> headers, out HashSet<Instruction> entries)
	{
		headers = new HashSet<Instruction>();
		entries = new HashSet<Instruction>();
		foreach (ExceptionHandler exceptionHandler in body.ExceptionHandlers)
		{
			headers.Add(exceptionHandler.TryStart);
			if (exceptionHandler.TryEnd != null)
			{
				headers.Add(exceptionHandler.TryEnd);
			}
			headers.Add(exceptionHandler.HandlerStart);
			entries.Add(exceptionHandler.HandlerStart);
			if (exceptionHandler.HandlerEnd != null)
			{
				headers.Add(exceptionHandler.HandlerEnd);
			}
			if (exceptionHandler.FilterStart != null)
			{
				headers.Add(exceptionHandler.FilterStart);
				entries.Add(exceptionHandler.FilterStart);
			}
		}
		IList<Instruction> instructions = body.Instructions;
		for (int i = 0; i < instructions.Count; i++)
		{
			Instruction instruction = instructions[i];
			if (instruction.Operand is Instruction)
			{
				headers.Add((Instruction)instruction.Operand);
				if (i + 1 < body.Instructions.Count)
				{
					headers.Add(body.Instructions[i + 1]);
				}
			}
			else if (instruction.Operand is Instruction[])
			{
				Instruction[] array = (Instruction[])instruction.Operand;
				foreach (Instruction item in array)
				{
					headers.Add(item);
				}
				if (i + 1 < body.Instructions.Count)
				{
					headers.Add(body.Instructions[i + 1]);
				}
			}
			else if ((instruction.OpCode.FlowControl == FlowControl.Throw || instruction.OpCode.FlowControl == FlowControl.Return) && i + 1 < body.Instructions.Count)
			{
				headers.Add(body.Instructions[i + 1]);
			}
		}
		if (instructions.Count > 0)
		{
			headers.Add(instructions[0]);
			entries.Add(instructions[0]);
		}
	}

	private static List<BasicBlock<CILInstrList>> SplitBlocks(CilBody body, HashSet<Instruction> headers, HashSet<Instruction> entries)
	{
		int num = 0;
		int num2 = -1;
		Instruction instruction = null;
		List<BasicBlock<CILInstrList>> list = new List<BasicBlock<CILInstrList>>();
		CILInstrList cILInstrList = new CILInstrList();
		for (int i = 0; i < body.Instructions.Count; i++)
		{
			Instruction instruction2 = body.Instructions[i];
			if (headers.Contains(instruction2))
			{
				if (instruction != null)
				{
					Instruction instruction3 = body.Instructions[i - 1];
					Debug.Assert(cILInstrList.Count > 0);
					list.Add(new BasicBlock<CILInstrList>(num2, cILInstrList));
					cILInstrList = new CILInstrList();
				}
				num2 = num++;
				instruction = instruction2;
			}
			cILInstrList.Add(instruction2);
		}
		if (list.Count == 0 || list[list.Count - 1].Id != num2)
		{
			Instruction instruction4 = body.Instructions[body.Instructions.Count - 1];
			Debug.Assert(cILInstrList.Count > 0);
			list.Add(new BasicBlock<CILInstrList>(num2, cILInstrList));
		}
		return list;
	}

	private static void LinkBlocks(List<BasicBlock<CILInstrList>> blocks)
	{
		Dictionary<Instruction, BasicBlock<CILInstrList>> dictionary = blocks.SelectMany((BasicBlock<CILInstrList> block) => block.Content.Select((Instruction instr) => new
		{
			Instr = instr,
			Block = block
		})).ToDictionary(instr => instr.Instr, instr => instr.Block);
		foreach (BasicBlock<CILInstrList> block in blocks)
		{
			foreach (Instruction item in block.Content)
			{
				if (item.Operand is Instruction)
				{
					BasicBlock<CILInstrList> basicBlock = dictionary[(Instruction)item.Operand];
					basicBlock.Sources.Add(block);
					block.Targets.Add(basicBlock);
				}
				else if (item.Operand is Instruction[])
				{
					Instruction[] array = (Instruction[])item.Operand;
					foreach (Instruction key in array)
					{
						BasicBlock<CILInstrList> basicBlock2 = dictionary[key];
						basicBlock2.Sources.Add(block);
						block.Targets.Add(basicBlock2);
					}
				}
			}
		}
		for (int j = 0; j < blocks.Count; j++)
		{
			Instruction instruction = blocks[j].Content.Last();
			if (instruction.OpCode.FlowControl != 0 && instruction.OpCode.FlowControl != FlowControl.Return && instruction.OpCode.FlowControl != FlowControl.Throw && j + 1 < blocks.Count)
			{
				BasicBlock<CILInstrList> basicBlock3 = blocks[j];
				BasicBlock<CILInstrList> basicBlock4 = blocks[j + 1];
				if (!basicBlock3.Targets.Contains(basicBlock4))
				{
					basicBlock3.Targets.Add(basicBlock4);
					basicBlock4.Sources.Add(basicBlock3);
					basicBlock3.Content.Add(Instruction.Create(OpCodes.Br, basicBlock4.Content[0]));
				}
			}
		}
	}

	private static ScopeBlock AssignScopes(CilBody body, List<BasicBlock<CILInstrList>> blocks)
	{
		Dictionary<ExceptionHandler, Tuple<ScopeBlock, ScopeBlock, ScopeBlock>> dictionary = new Dictionary<ExceptionHandler, Tuple<ScopeBlock, ScopeBlock, ScopeBlock>>();
		foreach (ExceptionHandler exceptionHandler in body.ExceptionHandlers)
		{
			ScopeBlock item = new ScopeBlock(ScopeType.Try, exceptionHandler);
			ScopeBlock item2 = new ScopeBlock(ScopeType.Handler, exceptionHandler);
			if (exceptionHandler.FilterStart != null)
			{
				ScopeBlock item3 = new ScopeBlock(ScopeType.Filter, exceptionHandler);
				dictionary[exceptionHandler] = Tuple.Create(item, item2, item3);
			}
			else
			{
				dictionary[exceptionHandler] = Tuple.Create<ScopeBlock, ScopeBlock, ScopeBlock>(item, item2, null);
			}
		}
		ScopeBlock scopeBlock = new ScopeBlock();
		Stack<ScopeBlock> stack = new Stack<ScopeBlock>();
		stack.Push(scopeBlock);
		foreach (BasicBlock<CILInstrList> block2 in blocks)
		{
			Instruction instruction = block2.Content[0];
			foreach (ExceptionHandler exceptionHandler2 in body.ExceptionHandlers)
			{
				Tuple<ScopeBlock, ScopeBlock, ScopeBlock> tuple = dictionary[exceptionHandler2];
				if (instruction == exceptionHandler2.TryEnd)
				{
					ScopeBlock scopeBlock2 = stack.Pop();
					Debug.Assert(scopeBlock2 == tuple.Item1);
				}
				if (instruction == exceptionHandler2.HandlerEnd)
				{
					ScopeBlock scopeBlock3 = stack.Pop();
					Debug.Assert(scopeBlock3 == tuple.Item2);
				}
				if (exceptionHandler2.FilterStart != null && instruction == exceptionHandler2.HandlerStart)
				{
					Debug.Assert(stack.Peek().Type == ScopeType.Filter);
					ScopeBlock scopeBlock4 = stack.Pop();
					Debug.Assert(scopeBlock4 == tuple.Item3);
				}
			}
			foreach (ExceptionHandler item4 in body.ExceptionHandlers.Reverse())
			{
				Tuple<ScopeBlock, ScopeBlock, ScopeBlock> tuple2 = dictionary[item4];
				ScopeBlock scopeBlock5 = ((stack.Count > 0) ? stack.Peek() : null);
				if (instruction == item4.TryStart)
				{
					if (scopeBlock5 != null)
					{
						AddScopeBlock(scopeBlock5, tuple2.Item1);
					}
					stack.Push(tuple2.Item1);
				}
				if (instruction == item4.HandlerStart)
				{
					if (scopeBlock5 != null)
					{
						AddScopeBlock(scopeBlock5, tuple2.Item2);
					}
					stack.Push(tuple2.Item2);
				}
				if (instruction == item4.FilterStart)
				{
					if (scopeBlock5 != null)
					{
						AddScopeBlock(scopeBlock5, tuple2.Item3);
					}
					stack.Push(tuple2.Item3);
				}
			}
			ScopeBlock block = stack.Peek();
			AddBasicBlock(block, block2);
		}
		foreach (ExceptionHandler exceptionHandler3 in body.ExceptionHandlers)
		{
			if (exceptionHandler3.TryEnd == null)
			{
				ScopeBlock scopeBlock6 = stack.Pop();
				Debug.Assert(scopeBlock6 == dictionary[exceptionHandler3].Item1);
			}
			if (exceptionHandler3.HandlerEnd == null)
			{
				ScopeBlock scopeBlock7 = stack.Pop();
				Debug.Assert(scopeBlock7 == dictionary[exceptionHandler3].Item2);
			}
		}
		Debug.Assert(stack.Count == 1);
		Validate(scopeBlock);
		return scopeBlock;
	}

	private static void Validate(ScopeBlock scope)
	{
		scope.Validate();
		foreach (ScopeBlock child in scope.Children)
		{
			Validate(child);
		}
	}

	private static void AddScopeBlock(ScopeBlock block, ScopeBlock child)
	{
		if (block.Content.Count > 0)
		{
			ScopeBlock scopeBlock = new ScopeBlock();
			foreach (IBasicBlock item in block.Content)
			{
				scopeBlock.Content.Add(item);
			}
			block.Content.Clear();
			block.Children.Add(scopeBlock);
		}
		block.Children.Add(child);
	}

	private static void AddBasicBlock(ScopeBlock block, BasicBlock<CILInstrList> child)
	{
		if (block.Children.Count > 0)
		{
			ScopeBlock scopeBlock = block.Children.Last();
			if (scopeBlock.Type != 0)
			{
				scopeBlock = new ScopeBlock();
				block.Children.Add(scopeBlock);
			}
			block = scopeBlock;
		}
		Debug.Assert(block.Children.Count == 0);
		block.Content.Add(child);
	}
}
