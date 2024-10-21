#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VM;

namespace KoiVM.Core.VMIR.Translation;

public class RetHandler : ITranslationHandler
{
	public Code ILCode => Code.Ret;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		if (expr.Arguments.Length == 1)
		{
			IIROperand iIROperand = tr.Translate(expr.Arguments[0]);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = new IRRegister(VMRegisters.R0, iIROperand.Type),
				Operand2 = iIROperand
			});
		}
		else
		{
			Debug.Assert(expr.Arguments.Length == 0);
		}
		tr.Instructions.Add(new IRInstruction(IROpCode.RET));
		return null;
	}
}
