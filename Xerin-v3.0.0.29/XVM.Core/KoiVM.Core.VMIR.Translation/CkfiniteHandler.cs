#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class CkfiniteHandler : ITranslationHandler
{
	public Code ILCode => Code.Ckfinite;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand iIROperand = tr.Translate(expr.Arguments[0]);
		int cKFINITE = tr.VM.Runtime.VMCall.CKFINITE;
		if (iIROperand.Type == ASTType.R4)
		{
			tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
			{
				Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
			});
		}
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(cKFINITE), iIROperand));
		return iIROperand;
	}
}
