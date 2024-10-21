using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class CallHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.CALL;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.PushOperand(instr.Operand1);
		tr.Instructions.Add(new ILInstruction(ILOpCode.CALL)
		{
			Annotation = instr.Annotation
		});
	}
}
