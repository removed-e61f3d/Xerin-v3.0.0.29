using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class PopHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.POP;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.PopOperand(instr.Operand1);
	}
}
