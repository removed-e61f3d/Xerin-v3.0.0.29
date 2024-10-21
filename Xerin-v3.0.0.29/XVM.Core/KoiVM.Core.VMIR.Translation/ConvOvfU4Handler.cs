#define DEBUG
using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class ConvOvfU4Handler : ITranslationHandler
{
	public Code ILCode => Code.Conv_Ovf_U4;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand iIROperand = tr.Translate(expr.Arguments[0]);
		ASTType type = iIROperand.Type;
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.I4);
		int rANGECHK = tr.VM.Runtime.VMCall.RANGECHK;
		int cKOVERFLOW = tr.VM.Runtime.VMCall.CKOVERFLOW;
		switch (type)
		{
		case ASTType.I4:
		case ASTType.I8:
		case ASTType.Ptr:
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(0L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(2147483647L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(rANGECHK), iIROperand));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(cKOVERFLOW)));
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, iRVariable, iIROperand));
			return iRVariable;
		case ASTType.R4:
		case ASTType.R8:
		{
			IRVariable iRVariable2 = tr.Context.AllocateVRegister(ASTType.I8);
			IRVariable iRVariable3 = tr.Context.AllocateVRegister(ASTType.I4);
			tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, iRVariable2, iIROperand));
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = iRVariable3,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.OVERFLOW)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(cKOVERFLOW), iRVariable3));
			iIROperand = iRVariable2;
			goto case ASTType.I4;
		}
		default:
			throw new NotSupportedException();
		}
	}
}
