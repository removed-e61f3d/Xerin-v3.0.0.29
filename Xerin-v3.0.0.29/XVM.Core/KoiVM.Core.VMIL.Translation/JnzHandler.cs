using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class JnzHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.JNZ;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.PushOperand(instr.Operand2);
		tr.PushOperand(instr.Operand1);
		tr.Instructions.Add(new ILInstruction(ILOpCode.JNZ)
		{
			Annotation = InstrAnnotation.JUMP
		});
	}
}
