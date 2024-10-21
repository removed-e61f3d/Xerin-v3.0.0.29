using dnlib.DotNet;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class StobjHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.__STOBJ;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.PushOperand(instr.Operand2);
		tr.PushOperand(instr.Operand1);
		TypeSig rawType = ((PointerInfo)instr.Annotation).PointerType.ToTypeSig();
		tr.Instructions.Add(new ILInstruction(TranslationHelpers.GetSIND(instr.Operand2.Type, rawType)));
	}
}
