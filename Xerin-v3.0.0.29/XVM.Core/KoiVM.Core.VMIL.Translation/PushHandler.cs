using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class PushHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.PUSH;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.PushOperand(instr.Operand1);
	}
}
