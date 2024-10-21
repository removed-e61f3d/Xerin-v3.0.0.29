using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class NopHandler : ITranslationHandler
{
	public Code ILCode => Code.Nop;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		tr.Instructions.Add(new IRInstruction(IROpCode.NOP));
		return null;
	}
}
