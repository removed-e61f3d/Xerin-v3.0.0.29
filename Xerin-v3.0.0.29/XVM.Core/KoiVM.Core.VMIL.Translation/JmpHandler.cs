using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class JmpHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.JMP;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.PushOperand(instr.Operand1);
		tr.Instructions.Add(new ILInstruction(ILOpCode.JMP)
		{
			Annotation = InstrAnnotation.JUMP
		});
	}
}
