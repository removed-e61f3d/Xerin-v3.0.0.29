using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class NopHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.NOP;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.Instructions.Add(new ILInstruction(ILOpCode.NOP));
	}
}
