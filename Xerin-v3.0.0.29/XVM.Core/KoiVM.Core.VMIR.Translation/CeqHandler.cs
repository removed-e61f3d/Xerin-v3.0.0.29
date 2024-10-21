#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class CeqHandler : ITranslationHandler
{
	public Code ILCode => Code.Ceq;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 2);
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.I4);
		TranslationHelpers.EmitCompareEq(tr, expr.Arguments[0].Type.Value, tr.Translate(expr.Arguments[0]), tr.Translate(expr.Arguments[1]));
		tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
		{
			Operand1 = iRVariable,
			Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.ZERO)
		});
		return iRVariable;
	}
}
