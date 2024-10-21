using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdnullHandler : ITranslationHandler
{
	public Code ILCode => Code.Ldnull;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		return IRConstant.Null();
	}
}
