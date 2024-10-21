using System.Collections.Generic;
using System.Linq;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VM;

namespace KoiVM.Core.VMIL.Transforms;

public class SaveRegistersTransform : IPostTransform
{
	private HashSet<VMRegisters> saveRegs;

	public void Initialize(ILPostTransformer tr)
	{
		saveRegs = tr.Runtime.Descriptor.Data.LookupInfo(tr.Method).UsedRegister;
	}

	public void Transform(ILPostTransformer tr)
	{
		tr.Instructions.VisitInstrs(VisitInstr, tr);
	}

	private void VisitInstr(ILInstrList instrs, ILInstruction instr, ref int index, ILPostTransformer tr)
	{
		if (instr.OpCode != ILOpCode.__BEGINCALL && instr.OpCode != ILOpCode.__ENDCALL)
		{
			return;
		}
		InstrCallInfo instrCallInfo = (InstrCallInfo)instr.Annotation;
		if (instrCallInfo.IsECall)
		{
			instrs.RemoveAt(index);
			index--;
			return;
		}
		HashSet<VMRegisters> hashSet = new HashSet<VMRegisters>(saveRegs);
		IRVariable iRVariable = (IRVariable)instrCallInfo.ReturnValue;
		if (iRVariable != null)
		{
			if (instrCallInfo.ReturnSlot == null)
			{
				VMRegisters register = instrCallInfo.ReturnRegister.Register;
				hashSet.Remove(register);
				if (register != 0)
				{
					hashSet.Add(VMRegisters.R0);
				}
			}
			else
			{
				hashSet.Add(VMRegisters.R0);
			}
		}
		else
		{
			hashSet.Add(VMRegisters.R0);
		}
		if (instr.OpCode == ILOpCode.__BEGINCALL)
		{
			instrs.Replace(index, hashSet.Select((VMRegisters reg) => new ILInstruction(ILOpCode.PUSHR_OBJECT, ILRegister.LookupRegister(reg), instr)));
		}
		else
		{
			instrs.Replace(index, hashSet.Select((VMRegisters reg) => new ILInstruction(ILOpCode.POP, ILRegister.LookupRegister(reg), instr)).Reverse());
		}
		index--;
	}
}
