using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class BeginCallHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.__BEGINCALL;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.Instructions.Add(new ILInstruction(ILOpCode.__BEGINCALL)
		{
			Annotation = instr.Annotation
		});
	}
}
