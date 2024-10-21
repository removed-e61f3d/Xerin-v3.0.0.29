using dnlib.DotNet;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class LdobjHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.__LDOBJ;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.PushOperand(instr.Operand1);
		TypeSig rawType = ((PointerInfo)instr.Annotation).PointerType.ToTypeSig();
		tr.Instructions.Add(new ILInstruction(TranslationHelpers.GetLIND(instr.Operand2.Type, rawType)));
		tr.PopOperand(instr.Operand2);
	}
}
