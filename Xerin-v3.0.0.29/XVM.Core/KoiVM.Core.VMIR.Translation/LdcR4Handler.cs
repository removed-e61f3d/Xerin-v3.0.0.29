using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdcR4Handler : ITranslationHandler
{
	public Code ILCode => Code.Ldc_R4;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		return IRConstant.FromR4((float)expr.Operand);
	}
}
