using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdcI8Handler : ITranslationHandler
{
	public Code ILCode => Code.Ldc_I8;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		return IRConstant.FromI8((long)expr.Operand);
	}
}
