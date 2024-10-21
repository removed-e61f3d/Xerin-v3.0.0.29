using KoiVM.Core.AST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Transforms;

public class MarkReturnRegTransform : ITransform
{
	public void Initialize(IRTransformer tr)
	{
	}

	public void Transform(IRTransformer tr)
	{
		tr.Instructions.VisitInstrs(VisitInstr, tr);
	}

	private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
	{
		if (instr.Annotation is InstrCallInfo { ReturnValue: not null } instrCallInfo)
		{
			if (instr.Operand1 is IRRegister && ((IRRegister)instr.Operand1).SourceVariable == instrCallInfo.ReturnValue)
			{
				instrCallInfo.ReturnRegister = (IRRegister)instr.Operand1;
			}
			else if (instr.Operand1 is IRPointer && ((IRPointer)instr.Operand1).SourceVariable == instrCallInfo.ReturnValue)
			{
				instrCallInfo.ReturnSlot = (IRPointer)instr.Operand1;
			}
		}
	}
}
