#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;

namespace KoiVM.Core.VMIR.Translation;

public class SwitchHandler : ITranslationHandler
{
	public Code ILCode => Code.Switch;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand operand = tr.Translate(expr.Arguments[0]);
		tr.Instructions.Add(new IRInstruction(IROpCode.SWT)
		{
			Operand1 = new IRJumpTable((IBasicBlock[])expr.Operand),
			Operand2 = operand
		});
		return null;
	}
}
