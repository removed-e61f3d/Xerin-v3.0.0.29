#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Confuser.DynCipher.AST;

namespace Confuser.DynCipher.Generation;

public class x86CodeGen
{
	private List<x86Instruction> instrs;

	private bool[] usedRegs;

	public IList<x86Instruction> Instructions => instrs;

	public int MaxUsedRegister { get; private set; }

	public x86Register? GenerateX86(Expression expression, Func<Variable, x86Register, IEnumerable<x86Instruction>> loadArg)
	{
		instrs = new List<x86Instruction>();
		usedRegs = new bool[8];
		MaxUsedRegister = -1;
		usedRegs[5] = true;
		usedRegs[4] = true;
		try
		{
			return ((x86RegisterOperand)Emit(expression, loadArg)).Register;
		}
		catch (Exception ex)
		{
			if (!(ex.Message == "Register overflowed."))
			{
				throw;
			}
			return null;
		}
	}

	private x86Register GetFreeRegister()
	{
		int num = 0;
		while (true)
		{
			if (num < 8)
			{
				if (!usedRegs[num])
				{
					break;
				}
				num++;
				continue;
			}
			throw new Exception("Register overflowed.");
		}
		return (x86Register)num;
	}

	private void TakeRegister(x86Register reg)
	{
		usedRegs[(int)reg] = true;
		if ((int)reg > MaxUsedRegister)
		{
			MaxUsedRegister = (int)reg;
		}
	}

	private void ReleaseRegister(x86Register reg)
	{
		usedRegs[(int)reg] = false;
	}

	private x86Register Normalize(x86Instruction instr)
	{
		if (instr.Operands.Length == 2 && instr.Operands[0] is x86ImmediateOperand && instr.Operands[1] is x86ImmediateOperand)
		{
			x86Register freeRegister = GetFreeRegister();
			instrs.Add(x86Instruction.Create((x86OpCode)0, new x86RegisterOperand(freeRegister), instr.Operands[0]));
			instr.Operands[0] = new x86RegisterOperand(freeRegister);
			instrs.Add(instr);
			return freeRegister;
		}
		if (instr.Operands.Length == 1 && instr.Operands[0] is x86ImmediateOperand)
		{
			x86Register freeRegister2 = GetFreeRegister();
			instrs.Add(x86Instruction.Create((x86OpCode)0, new x86RegisterOperand(freeRegister2), instr.Operands[0]));
			instr.Operands[0] = new x86RegisterOperand(freeRegister2);
			instrs.Add(instr);
			return freeRegister2;
		}
		if (instr.OpCode == (x86OpCode)2 && instr.Operands[0] is x86ImmediateOperand && instr.Operands[1] is x86RegisterOperand)
		{
			x86Register register = ((x86RegisterOperand)instr.Operands[1]).Register;
			instrs.Add(x86Instruction.Create(x86OpCode.NEG, new x86RegisterOperand(register)));
			instr.OpCode = x86OpCode.ADD;
			instr.Operands[1] = instr.Operands[0];
			instr.Operands[0] = new x86RegisterOperand(register);
			instrs.Add(instr);
			return register;
		}
		if (instr.Operands.Length == 2 && instr.Operands[0] is x86ImmediateOperand && instr.Operands[1] is x86RegisterOperand)
		{
			x86Register register2 = ((x86RegisterOperand)instr.Operands[1]).Register;
			instr.Operands[1] = instr.Operands[0];
			instr.Operands[0] = new x86RegisterOperand(register2);
			instrs.Add(instr);
			return register2;
		}
		Debug.Assert(instr.Operands.Length != 0);
		Debug.Assert(instr.Operands[0] is x86RegisterOperand);
		if (instr.Operands.Length == 2 && instr.Operands[1] is x86RegisterOperand)
		{
			ReleaseRegister(((x86RegisterOperand)instr.Operands[1]).Register);
		}
		instrs.Add(instr);
		return ((x86RegisterOperand)instr.Operands[0]).Register;
	}

	private Ix86Operand Emit(Expression exp, Func<Variable, x86Register, IEnumerable<x86Instruction>> loadArg)
	{
		if (exp is BinOpExpression)
		{
			BinOpExpression binOpExpression = (BinOpExpression)exp;
			x86Register reg = binOpExpression.Operation switch
			{
				(BinOps)0 => Normalize(x86Instruction.Create(x86OpCode.ADD, Emit(binOpExpression.Left, loadArg), Emit(binOpExpression.Right, loadArg))), 
				BinOps.Sub => Normalize(x86Instruction.Create((x86OpCode)2, Emit(binOpExpression.Left, loadArg), Emit(binOpExpression.Right, loadArg))), 
				BinOps.Mul => Normalize(x86Instruction.Create(x86OpCode.IMUL, Emit(binOpExpression.Left, loadArg), Emit(binOpExpression.Right, loadArg))), 
				(BinOps)6 => Normalize(x86Instruction.Create(x86OpCode.XOR, Emit(binOpExpression.Left, loadArg), Emit(binOpExpression.Right, loadArg))), 
				_ => throw new NotSupportedException(), 
			};
			TakeRegister(reg);
			return new x86RegisterOperand(reg);
		}
		if (exp is UnaryOpExpression)
		{
			UnaryOpExpression unaryOpExpression = (UnaryOpExpression)exp;
			x86Register reg2 = unaryOpExpression.Operation switch
			{
				UnaryOps.Negate => Normalize(x86Instruction.Create(x86OpCode.NEG, Emit(unaryOpExpression.Value, loadArg))), 
				(UnaryOps)0 => Normalize(x86Instruction.Create((x86OpCode)6, Emit(unaryOpExpression.Value, loadArg))), 
				_ => throw new NotSupportedException(), 
			};
			TakeRegister(reg2);
			return new x86RegisterOperand(reg2);
		}
		if (exp is LiteralExpression)
		{
			return new x86ImmediateOperand((int)((LiteralExpression)exp).Value);
		}
		if (exp is VariableExpression)
		{
			x86Register freeRegister = GetFreeRegister();
			TakeRegister(freeRegister);
			instrs.AddRange(loadArg(((VariableExpression)exp).Variable, freeRegister));
			return new x86RegisterOperand(freeRegister);
		}
		throw new NotSupportedException();
	}

	public override string ToString()
	{
		return string.Join("\r\n", instrs.Select((x86Instruction instr) => instr.ToString()).ToArray());
	}
}
