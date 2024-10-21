using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class EntryHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.__ENTRY;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.Instructions.Add(new ILInstruction(ILOpCode.__ENTRY));
	}
}
