#define DEBUG
using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class ConvOvfI8Handler : ITranslationHandler
{
	public Code ILCode => Code.Conv_Ovf_I8;

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
		int cKOVERFLOW = tr.VM.Runtime.VMCall.CKOVERFLOW;
		switch (type)
		{
		case ASTType.I4:
			tr.Instructions.Add(new IRInstruction(IROpCode.SX, iRVariable, iIROperand));
			break;
		case ASTType.Ptr:
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, iRVariable, iIROperand));
			break;
		case ASTType.R4:
		case ASTType.R8:
		{
			IRVariable iRVariable2 = tr.Context.AllocateVRegister(ASTType.I4);
			tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, iRVariable, iIROperand));
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = iRVariable2,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.OVERFLOW)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(cKOVERFLOW), iRVariable2));
			break;
		}
		default:
			throw new NotSupportedException();
		}
		return iRVariable;
	}
}
