using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdcI4Handler : ITranslationHandler
{
	public Code ILCode => Code.Ldc_I4;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		return IRConstant.FromI4((int)expr.Operand);
	}
}
