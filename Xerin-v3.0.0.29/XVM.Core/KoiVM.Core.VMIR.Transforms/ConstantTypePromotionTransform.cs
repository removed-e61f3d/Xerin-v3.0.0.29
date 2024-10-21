#define DEBUG
using System;
using System.Diagnostics;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Transforms;

public class ConstantTypePromotionTransform : ITransform
{
	public void Initialize(IRTransformer tr)
	{
	}

	public void Transform(IRTransformer tr)
	{
		tr.Instructions.VisitInstrs(VisitInstr, tr);
	}

	private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
	{
		switch (instr.OpCode)
		{
		case IROpCode.MOV:
		case IROpCode.NOR:
		case IROpCode.CMP:
		case IROpCode.ADD:
		case IROpCode.MUL:
		case IROpCode.DIV:
		case IROpCode.REM:
		case IROpCode.__AND:
		case IROpCode.__OR:
		case IROpCode.__XOR:
		case IROpCode.__GETF:
			Debug.Assert(instr.Operand1 != null && instr.Operand2 != null);
			if (instr.Operand1 is IRConstant)
			{
				instr.Operand1 = PromoteConstant((IRConstant)instr.Operand1, instr.Operand2.Type);
			}
			if (instr.Operand2 is IRConstant)
			{
				instr.Operand2 = PromoteConstant((IRConstant)instr.Operand2, instr.Operand1.Type);
			}
			break;
		}
	}

	private static IIROperand PromoteConstant(IRConstant value, ASTType type)
	{
		return type switch
		{
			ASTType.I8 => PromoteConstantI8(value), 
			ASTType.R4 => PromoteConstantR4(value), 
			ASTType.R8 => PromoteConstantR8(value), 
			_ => value, 
		};
	}

	private static IIROperand PromoteConstantI8(IRConstant value)
	{
		if (value.Type.Value == ASTType.I4)
		{
			value.Type = ASTType.I8;
			value.Value = (long)(int)value.Value;
		}
		else if (value.Type.Value != ASTType.I8)
		{
			throw new InvalidProgramException();
		}
		return value;
	}

	private static IIROperand PromoteConstantR4(IRConstant value)
	{
		if (value.Type.Value == ASTType.I4)
		{
			value.Type = ASTType.R4;
			value.Value = (float)(int)value.Value;
		}
		else if (value.Type.Value != ASTType.R4)
		{
			throw new InvalidProgramException();
		}
		return value;
	}

	private static IIROperand PromoteConstantR8(IRConstant value)
	{
		if (value.Type.Value == ASTType.I4)
		{
			value.Type = ASTType.R8;
			value.Value = (double)(int)value.Value;
		}
		else if (value.Type.Value == ASTType.I8)
		{
			value.Type = ASTType.R8;
			value.Value = (double)(long)value.Value;
		}
		else if (value.Type.Value == ASTType.R4)
		{
			value.Type = ASTType.R8;
			value.Value = (double)(float)value.Value;
		}
		else if (value.Type.Value != ASTType.R8)
		{
			throw new InvalidProgramException();
		}
		return value;
	}
}
