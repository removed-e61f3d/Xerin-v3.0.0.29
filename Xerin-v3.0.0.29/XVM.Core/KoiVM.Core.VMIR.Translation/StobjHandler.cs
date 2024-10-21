#define DEBUG
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class StobjHandler : ITranslationHandler
{
	public Code ILCode => Code.Stobj;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 2);
		IIROperand op = tr.Translate(expr.Arguments[0]);
		IIROperand op2 = tr.Translate(expr.Arguments[1]);
		tr.Instructions.Add(new IRInstruction(IROpCode.__STOBJ, op, op2)
		{
			Annotation = new PointerInfo("STOBJ", (ITypeDefOrRef)expr.Operand)
		});
		return null;
	}
}
