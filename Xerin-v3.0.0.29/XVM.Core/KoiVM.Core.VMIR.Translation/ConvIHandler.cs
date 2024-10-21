#define DEBUG
using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class ConvIHandler : ITranslationHandler
{
	public Code ILCode => Code.Conv_I;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand iIROperand = tr.Translate(expr.Arguments[0]);
		ASTType type = iIROperand.Type;
		if (type == ASTType.Ptr || type == ASTType.I4)
		{
			return iIROperand;
		}
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.Ptr);
		switch (type)
		{
		case ASTType.I8:
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, iRVariable, iIROperand));
			break;
		case ASTType.R4:
		case ASTType.R8:
		{
			IRVariable iRVariable2 = tr.Context.AllocateVRegister(ASTType.I8);
			tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, iRVariable2, iIROperand));
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, iRVariable, iRVariable2));
			break;
		}
		default:
			throw new NotSupportedException();
		}
		return iRVariable;
	}
}
