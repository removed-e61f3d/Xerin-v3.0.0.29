#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;
using KoiVM.Core.VM;

namespace KoiVM.Core.VMIR.RegAlloc;

public class RegisterAllocator
{
	private struct StackSlot
	{
		public readonly int Offset;

		public readonly IRVariable Variable;

		public StackSlot(int offset, IRVariable var)
		{
			Offset = offset;
			Variable = var;
		}
	}

	private class RegisterPool
	{
		private const int NumRegisters = 8;

		private IRVariable[] regAlloc;

		private Dictionary<IRVariable, StackSlot> spillVars;

		public int SpillOffset { get; set; }

		private static VMRegisters ToRegister(int regId)
		{
			return (VMRegisters)regId;
		}

		private static int FromRegister(VMRegisters reg)
		{
			return (int)reg;
		}

		public static RegisterPool Create(int baseOffset, Dictionary<IRVariable, StackSlot> globalVars)
		{
			RegisterPool registerPool = new RegisterPool();
			registerPool.regAlloc = new IRVariable[8];
			registerPool.spillVars = new Dictionary<IRVariable, StackSlot>(globalVars);
			registerPool.SpillOffset = baseOffset;
			return registerPool;
		}

		public VMRegisters? Allocate(IRVariable var)
		{
			for (int i = 0; i < regAlloc.Length; i++)
			{
				if (regAlloc[i] == null)
				{
					regAlloc[i] = var;
					return ToRegister(i);
				}
			}
			return null;
		}

		public void Deallocate(IRVariable var, VMRegisters reg)
		{
			Debug.Assert(regAlloc[FromRegister(reg)] == var);
			regAlloc[FromRegister(reg)] = null;
		}

		public void CheckLiveness(HashSet<IRVariable> live)
		{
			for (int i = 0; i < regAlloc.Length; i++)
			{
				if (regAlloc[i] != null && !live.Contains(regAlloc[i]))
				{
					regAlloc[i].Annotation = null;
					regAlloc[i] = null;
				}
			}
		}

		public StackSlot SpillVariable(IRVariable var)
		{
			StackSlot stackSlot = new StackSlot(SpillOffset++, var);
			spillVars[var] = stackSlot;
			return stackSlot;
		}

		public StackSlot? CheckSpill(IRVariable var)
		{
			if (!spillVars.TryGetValue(var, out var value))
			{
				return null;
			}
			return value;
		}
	}

	private Dictionary<IRVariable, object> allocation;

	private int baseOffset;

	private Dictionary<IRVariable, StackSlot> globalVars;

	private Dictionary<BasicBlock<IRInstrList>, BlockLiveness> liveness;

	private readonly IRTransformer transformer;

	public int LocalSize { get; set; }

	public RegisterAllocator(IRTransformer transformer)
	{
		this.transformer = transformer;
	}

	public void Initialize()
	{
		List<BasicBlock<IRInstrList>> blocks = transformer.RootScope.GetBasicBlocks().Cast<BasicBlock<IRInstrList>>().ToList();
		liveness = LivenessAnalysis.ComputeLiveness(blocks);
		HashSet<IRVariable> hashSet = new HashSet<IRVariable>();
		foreach (KeyValuePair<BasicBlock<IRInstrList>, BlockLiveness> item in liveness)
		{
			foreach (IRInstruction item2 in item.Key.Content)
			{
				if (item2.OpCode == IROpCode.__LEA)
				{
					IRVariable iRVariable = (IRVariable)item2.Operand2;
					if (iRVariable.VariableType != IRVariableType.Argument)
					{
						hashSet.Add(iRVariable);
					}
				}
			}
			hashSet.UnionWith(item.Value.OutLive);
		}
		int offset = 1;
		globalVars = hashSet.ToDictionary((IRVariable var) => var, (IRVariable var) => new StackSlot(offset++, var));
		baseOffset = offset;
		LocalSize = baseOffset - 1;
		offset = -2;
		IRVariable[] parameters = transformer.Context.GetParameters();
		for (int num = parameters.Length - 1; num >= 0; num--)
		{
			IRVariable iRVariable2 = parameters[num];
			globalVars[iRVariable2] = new StackSlot(offset--, iRVariable2);
		}
		allocation = ((IEnumerable<KeyValuePair<IRVariable, StackSlot>>)globalVars).ToDictionary((Func<KeyValuePair<IRVariable, StackSlot>, IRVariable>)((KeyValuePair<IRVariable, StackSlot> pair) => pair.Key), (Func<KeyValuePair<IRVariable, StackSlot>, object>)((KeyValuePair<IRVariable, StackSlot> pair) => pair.Value));
	}

	public void Allocate(BasicBlock<IRInstrList> block)
	{
		BlockLiveness blockLiveness = liveness[block];
		Dictionary<IRInstruction, HashSet<IRVariable>> dictionary = LivenessAnalysis.ComputeLiveness(block, blockLiveness);
		RegisterPool registerPool = RegisterPool.Create(baseOffset, globalVars);
		for (int i = 0; i < block.Content.Count; i++)
		{
			IRInstruction iRInstruction = block.Content[i];
			registerPool.CheckLiveness(dictionary[iRInstruction]);
			if (iRInstruction.Operand1 != null)
			{
				iRInstruction.Operand1 = AllocateOperand(iRInstruction.Operand1, registerPool);
			}
			if (iRInstruction.Operand2 != null)
			{
				iRInstruction.Operand2 = AllocateOperand(iRInstruction.Operand2, registerPool);
			}
		}
		if (registerPool.SpillOffset - 1 > LocalSize)
		{
			LocalSize = registerPool.SpillOffset - 1;
		}
		baseOffset = registerPool.SpillOffset;
	}

	private IIROperand AllocateOperand(IIROperand operand, RegisterPool pool)
	{
		if (operand is IRVariable)
		{
			IRVariable iRVariable = (IRVariable)operand;
			StackSlot? stackSlot;
			VMRegisters? vMRegisters = AllocateVariable(pool, iRVariable, out stackSlot);
			if (vMRegisters.HasValue)
			{
				return new IRRegister(vMRegisters.Value)
				{
					SourceVariable = iRVariable,
					Type = iRVariable.Type
				};
			}
			iRVariable.Annotation = stackSlot.Value;
			return new IRPointer
			{
				Register = IRRegister.BP,
				Offset = stackSlot.Value.Offset,
				SourceVariable = iRVariable,
				Type = iRVariable.Type
			};
		}
		return operand;
	}

	private VMRegisters? AllocateVariable(RegisterPool pool, IRVariable var, out StackSlot? stackSlot)
	{
		stackSlot = pool.CheckSpill(var);
		if (!stackSlot.HasValue)
		{
			VMRegisters? result = ((var.Annotation == null) ? null : new VMRegisters?((VMRegisters)var.Annotation));
			if (!result.HasValue)
			{
				result = pool.Allocate(var);
			}
			if (result.HasValue)
			{
				if (var.Annotation == null)
				{
					var.Annotation = result.Value;
				}
				return result;
			}
			stackSlot = pool.SpillVariable(var);
		}
		return null;
	}
}
