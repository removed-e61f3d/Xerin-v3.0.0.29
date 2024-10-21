#define DEBUG
using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class ConvOvfI2UnHandler : ITranslationHandler
{
	public Code ILCode => Code.Conv_Ovf_I2_Un;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand iIROperand = tr.Translate(expr.Arguments[0]);
		ASTType type = iIROperand.Type;
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.I4);
		IRVariable iRVariable2 = tr.Context.AllocateVRegister(ASTType.I4);
		iRVariable.RawType = tr.Context.Method.Module.CorLibTypes.Int16;
		int rANGECHK = tr.VM.Runtime.VMCall.RANGECHK;
		int cKOVERFLOW = tr.VM.Runtime.VMCall.CKOVERFLOW;
		switch (type)
		{
		case ASTType.I4:
		case ASTType.I8:
		case ASTType.Ptr:
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(0L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI8(32767L)));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(rANGECHK), iIROperand));
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(cKOVERFLOW)));
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV, iRVariable, iIROperand));
			tr.Instructions.Add(new IRInstruction(IROpCode.SX, iRVariable2, iRVariable));
			return iRVariable2;
		case ASTType.R4:
		case ASTType.R8:
		{
			IRVariable iRVariable3 = tr.Context.AllocateVRegister(ASTType.I8);
			IRVariable iRVariable4 = tr.Context.AllocateVRegister(ASTType.I4);
			tr.Instructions.Add(new IRInstruction(IROpCode.__SETF)
			{
				Operand1 = IRConstant.FromI4(1 << tr.Arch.Flags.UNSIGNED)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.ICONV, iRVariable3, iIROperand));
			tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
			{
				Operand1 = iRVariable4,
				Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.OVERFLOW)
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(cKOVERFLOW), iRVariable4));
			iIROperand = iRVariable3;
			goto case ASTType.I4;
		}
		default:
			throw new NotSupportedException();
		}
	}
}
