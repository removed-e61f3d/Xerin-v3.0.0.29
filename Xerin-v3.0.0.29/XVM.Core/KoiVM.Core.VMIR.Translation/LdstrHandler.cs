using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdstrHandler : ITranslationHandler
{
	public Code ILCode => Code.Ldstr;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		return IRConstant.FromString((string)expr.Operand);
	}
}
