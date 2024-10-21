using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class VcallHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.VCALL;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		if (instr.Operand2 != null)
		{
			tr.PushOperand(instr.Operand2);
		}
		tr.PushOperand(instr.Operand1);
		tr.Instructions.Add(new ILInstruction(ILOpCode.VCALL));
	}
}
