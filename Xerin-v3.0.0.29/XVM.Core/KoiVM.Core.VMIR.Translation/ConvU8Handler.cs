#define DEBUG
using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class ConvU8Handler : ITranslationHandler
{
	public Code ILCode => Code.Conv_U8;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand iIROperand = tr.Translate(expr.Arguments[0]);
		ASTType type = iIROperand.Type;
		if (type == ASTType.I8)
		{
			return iIROperand;
		}
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.I8);
		switch (type)
		{
		case ASTType.I4:
		case ASTType.Ptr:
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, iRVariable, iIROperand));
			break;
		case ASTType.R4:
		case ASTType.R8:
		{
			IRVariable op = tr.Context.AllocateVRegister(ASTType.I8);
			tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
			{
				Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, op, iIROperand));
			break;
		}
		default:
			throw new NotSupportedException();
		}
		return iRVariable;
	}
}
