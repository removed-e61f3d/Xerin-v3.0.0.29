#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;

namespace KoiVM.Core.VMIR.Translation;

public class BrtrueHandler : ITranslationHandler
{
	public Code ILCode => Code.Brtrue;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand a = tr.Translate(expr.Arguments[0]);
		TranslationHelpers.EmitCompareEq(tr, expr.Arguments[0].Type.Value, a, IRConstant.FromI4(0));
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.I4);
		tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
		{
			Operand1 = iRVariable,
			Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.ZERO)
		});
		tr.Instructions.Add(new IRInstruction(IROpCode.JZ)
		{
			Operand1 = new IRBlockTarget((IBasicBlock)expr.Operand),
			Operand2 = iRVariable
		});
		return null;
	}
}
