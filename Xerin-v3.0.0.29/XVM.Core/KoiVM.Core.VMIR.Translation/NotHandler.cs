#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class NotHandler : ITranslationHandler
{
	public Code ILCode => Code.Not;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
		{
			Operand1 = iRVariable,
			Operand2 = tr.Translate(expr.Arguments[0])
		});
		tr.Instructions.Add(new IRInstruction(IROpCode.__NOT)
		{
			Operand1 = iRVariable
		});
		return iRVariable;
	}
}
