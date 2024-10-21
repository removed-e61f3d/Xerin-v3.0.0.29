using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class ExitHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.__EXIT;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.Instructions.Add(new ILInstruction(ILOpCode.__EXIT));
	}
}
