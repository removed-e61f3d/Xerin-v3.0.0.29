#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class ThrowHandler : ITranslationHandler
{
	public Code ILCode => Code.Throw;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		int tHROW = tr.VM.Runtime.VMCall.THROW;
		tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, tr.Translate(expr.Arguments[0])));
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL)
		{
			Operand1 = IRConstant.FromI4(tHROW),
			Operand2 = IRConstant.FromI4(0)
		});
		return null;
	}
}
