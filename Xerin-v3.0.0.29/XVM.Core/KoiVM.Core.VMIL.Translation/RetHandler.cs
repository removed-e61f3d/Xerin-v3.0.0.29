using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class RetHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.RET;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.Instructions.Add(new ILInstruction(ILOpCode.RET));
	}
}
