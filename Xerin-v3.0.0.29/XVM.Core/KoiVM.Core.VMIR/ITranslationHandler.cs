using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR;

public interface ITranslationHandler
{
	Code ILCode { get; }

	IIROperand Translate(ILASTExpression expr, IRTranslator tr);
}
