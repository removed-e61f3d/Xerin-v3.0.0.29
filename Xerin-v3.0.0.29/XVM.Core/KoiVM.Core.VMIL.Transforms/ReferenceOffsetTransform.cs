using KoiVM.Core.AST.IL;

namespace KoiVM.Core.VMIL.Transforms;

public class ReferenceOffsetTransform : ITransform
{
	public void Initialize(ILTransformer tr)
	{
	}

	public void Transform(ILTransformer tr)
	{
		tr.Instructions.VisitInstrs(VisitInstr, tr);
	}

	private void VisitInstr(ILInstrList instrs, ILInstruction instr, ref int index, ILTransformer tr)
	{
		if (instr.OpCode == ILOpCode.PUSHI_DWORD && instr.Operand is IHasOffset)
		{
			ILInstruction iLInstruction = new ILInstruction(ILOpCode.PUSHR_QWORD, ILRegister.IP, instr);
			instr.OpCode = ILOpCode.PUSHI_DWORD;
			instr.Operand = new ILRelReference((IHasOffset)instr.Operand, iLInstruction);
			instrs.Replace(index, new ILInstruction[3]
			{
				iLInstruction,
				instr,
				new ILInstruction(ILOpCode.ADD_QWORD, null, instr)
			});
		}
	}
}
