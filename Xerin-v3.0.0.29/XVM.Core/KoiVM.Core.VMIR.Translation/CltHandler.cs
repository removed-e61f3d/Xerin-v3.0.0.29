#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class CltHandler : ITranslationHandler
{
	public Code ILCode => Code.Clt;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 2);
		tr.Instructions.Add(new IRInstruction(IROpCode.CMP)
		{
			Operand1 = tr.Translate(expr.Arguments[0]),
			Operand2 = tr.Translate(expr.Arguments[1])
		});
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.I4);
		IRVariable iRVariable2 = tr.Context.AllocateVRegister(ASTType.I4);
		tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
		{
			Operand1 = iRVariable2,
			Operand2 = IRConstant.FromI4((1 << tr.Arch.Flags.OVERFLOW) | (1 << tr.Arch.Flags.SIGN))
		});
		tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
		{
			Operand1 = iRVariable,
			Operand2 = iRVariable2
		});
		TranslationHelpers.EmitCompareEq(tr, ASTType.I4, iRVariable, IRConstant.FromI4((1 << tr.Arch.Flags.OVERFLOW) | (1 << tr.Arch.Flags.SIGN)));
		tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
		{
			Operand1 = iRVariable,
			Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.ZERO)
		});
		tr.Instructions.Add(new IRInstruction(IROpCode.__AND)
		{
			Operand1 = iRVariable2,
			Operand2 = iRVariable2
		});
		tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
		{
			Operand1 = iRVariable2,
			Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.ZERO)
		});
		tr.Instructions.Add(new IRInstruction(IROpCode.__OR)
		{
			Operand1 = iRVariable,
			Operand2 = iRVariable2
		});
		tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
		{
			Operand1 = iRVariable,
			Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.ZERO)
		});
		return iRVariable;
	}
}
