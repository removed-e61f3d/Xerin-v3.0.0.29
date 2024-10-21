#define DEBUG
using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class ConvRUnHandler : ITranslationHandler
{
	public Code ILCode => Code.Conv_R_Un;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand iIROperand = tr.Translate(expr.Arguments[0]);
		ASTType type = iIROperand.Type;
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.R8);
		switch (type)
		{
		case ASTType.I4:
		{
			IRVariable iRVariable2 = tr.Context.AllocateVRegister(ASTType.I8);
			tr.Instructions.Add(new IRInstruction(IROpCode.SX, iRVariable2, iIROperand));
			tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
			{
				Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.FCONV, iRVariable, iRVariable2));
			break;
		}
		case ASTType.I8:
			tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
			{
				Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.FCONV, iRVariable, iIROperand));
			break;
		default:
			throw new NotSupportedException();
		}
		return iRVariable;
	}
}
