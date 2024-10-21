#define DEBUG
using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class ConvI1Handler : ITranslationHandler
{
	public Code ILCode => Code.Conv_I1;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand iIROperand = tr.Translate(expr.Arguments[0]);
		ASTType type = iIROperand.Type;
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.I4);
		IRVariable iRVariable2 = tr.Context.AllocateVRegister(ASTType.I4);
		iRVariable.RawType = tr.Context.Method.Module.CorLibTypes.SByte;
		switch (type)
		{
		case ASTType.I4:
		case ASTType.I8:
		case ASTType.Ptr:
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, iRVariable, iIROperand));
			tr.Instructions.Add(new IRInstruction(IROpCode.SX, iRVariable2, iRVariable));
			break;
		case ASTType.R4:
		case ASTType.R8:
		{
			IRVariable iRVariable3 = tr.Context.AllocateVRegister(ASTType.I8);
			tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, iRVariable3, iIROperand));
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, iRVariable, iRVariable3));
			tr.Instructions.Add(new IRInstruction(IROpCode.SX, iRVariable2, iRVariable));
			break;
		}
		default:
			throw new NotSupportedException();
		}
		return iRVariable2;
	}
}
