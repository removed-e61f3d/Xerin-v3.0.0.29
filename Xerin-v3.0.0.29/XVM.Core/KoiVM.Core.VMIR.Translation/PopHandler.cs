#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class PopHandler : ITranslationHandler
{
	public Code ILCode => Code.Pop;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		tr.Translate(expr.Arguments[0]);
		return null;
	}
}
