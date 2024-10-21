using System;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class CalliHandler : ITranslationHandler
{
	public Code ILCode => Code.Calli;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		throw new NotSupportedException();
	}
}
