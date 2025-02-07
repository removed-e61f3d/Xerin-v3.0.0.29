#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class CltUnHandler : ITranslationHandler
{
	public Code ILCode => Code.Clt_Un;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 2);
		tr.Instructions.Add(new IRInstruction(IROpCode.CMP)
		{
			Operand1 = tr.Translate(expr.Arguments[0]),
			Operand2 = tr.Translate(expr.Arguments[1])
		});
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.I4);
		tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
		{
			Operand1 = iRVariable,
			Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.CARRY)
		});
		return iRVariable;
	}
}
