using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Utils;

namespace XProtections;

public class IntsToMath
{
	private static readonly Random rnd = new Random();

	private MethodDef Method { get; set; }

	public IntsToMath(MethodDef method)
	{
		Method = method;
	}

	public void Execute(ref int i)
	{
		switch (rnd.Next(0, 10))
		{
		case 0:
			Neg(ref i);
			break;
		case 1:
			Not(ref i);
			break;
		case 2:
			Shr(ref i);
			break;
		case 3:
			Shl(ref i);
			break;
		case 4:
			Or(ref i);
			break;
		case 5:
			Rem(ref i);
			break;
		case 6:
			ConditionalMath(ref i);
			break;
		case 7:
			Add(ref i);
			break;
		case 8:
			Sub(ref i);
			break;
		case 9:
			Xor(ref i);
			break;
		}
	}

	private void Sub(ref int i)
	{
		int ldcI4Value = Method.Body.Instructions[i].GetLdcI4Value();
		int num = Utils.RandomBigInt32();
		if ((long)ldcI4Value - (long)num >= -2147483648L && (long)ldcI4Value + (long)num <= 2147483647L)
		{
			int num2 = ldcI4Value - num;
			Method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
			Method.Body.Instructions[i].Operand = num2;
			Method.Body.Instructions.Insert(++i, OpCodes.Ldc_I4.ToInstruction(num));
			Method.Body.Instructions.Insert(++i, OpCodes.Add.ToInstruction());
		}
	}

	private void Xor(ref int i)
	{
		int ldcI4Value = Method.Body.Instructions[i].GetLdcI4Value();
		int num = Utils.RandomBigInt32();
		if ((long)ldcI4Value - (long)num >= -2147483648L && (long)ldcI4Value + (long)num <= 2147483647L)
		{
			int num2 = ldcI4Value ^ num;
			Method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
			Method.Body.Instructions[i].Operand = num2;
			Method.Body.Instructions.Insert(++i, OpCodes.Ldc_I4.ToInstruction(num));
			Method.Body.Instructions.Insert(++i, OpCodes.Xor.ToInstruction());
		}
	}

	private void Add(ref int i)
	{
		int ldcI4Value = Method.Body.Instructions[i].GetLdcI4Value();
		int num = Utils.RandomBigInt32();
		if ((long)ldcI4Value - (long)num >= -2147483648L && (long)ldcI4Value + (long)num <= 2147483647L)
		{
			int num2 = ldcI4Value + num;
			Method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
			Method.Body.Instructions[i].Operand = num2;
			Method.Body.Instructions.Insert(++i, OpCodes.Ldc_I4.ToInstruction(num));
			Method.Body.Instructions.Insert(++i, OpCodes.Sub.ToInstruction());
		}
	}

	private void Neg(ref int i)
	{
		int ldcI4Value = Method.Body.Instructions[i].GetLdcI4Value();
		int num = Utils.RandomBigInt32();
		if ((long)ldcI4Value - (long)num >= -2147483648L && (long)ldcI4Value + (long)num <= 2147483647L)
		{
			int value = -num;
			Calculator calculator = new Calculator(ldcI4Value, value);
			Method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
			Method.Body.Instructions[i].Operand = calculator.getResult();
			Method.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(num));
			Method.Body.Instructions.Insert(i + 2, OpCodes.Neg.ToInstruction());
			Method.Body.Instructions.Insert(i + 3, calculator.getOpCode().ToInstruction());
			i += 3;
		}
	}

	private void Rem(ref int i)
	{
		int ldcI4Value = Method.Body.Instructions[i].GetLdcI4Value();
		int num = Utils.RandomBigInt32();
		int num2 = Utils.RandomBigInt32();
		int value = num2 % num;
		Calculator calculator = new Calculator(ldcI4Value, value);
		Method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
		Method.Body.Instructions[i].Operand = calculator.getResult();
		Method.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(num2));
		Method.Body.Instructions.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(num));
		Method.Body.Instructions.Insert(i + 3, OpCodes.Rem.ToInstruction());
		Method.Body.Instructions.Insert(i + 4, calculator.getOpCode().ToInstruction());
		i += 4;
	}

	private void Not(ref int i)
	{
		int ldcI4Value = Method.Body.Instructions[i].GetLdcI4Value();
		int num = Utils.RandomBigInt32();
		if ((long)ldcI4Value - (long)num >= -2147483648L && (long)ldcI4Value + (long)num <= 2147483647L)
		{
			int value = ~num;
			Calculator calculator = new Calculator(ldcI4Value, value);
			Method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
			Method.Body.Instructions[i].Operand = calculator.getResult();
			Method.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(num));
			Method.Body.Instructions.Insert(i + 2, OpCodes.Not.ToInstruction());
			Method.Body.Instructions.Insert(i + 3, calculator.getOpCode().ToInstruction());
			i += 3;
		}
	}

	private void Shl(ref int i)
	{
		int ldcI4Value = Method.Body.Instructions[i].GetLdcI4Value();
		int num = Utils.RandomBigInt32();
		int num2 = Utils.RandomBigInt32();
		int value = num2 << num;
		Calculator calculator = new Calculator(ldcI4Value, value);
		Method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
		Method.Body.Instructions[i].Operand = calculator.getResult();
		Method.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(num2));
		Method.Body.Instructions.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(num));
		Method.Body.Instructions.Insert(i + 3, OpCodes.Shl.ToInstruction());
		Method.Body.Instructions.Insert(i + 4, calculator.getOpCode().ToInstruction());
		i += 4;
	}

	private void Or(ref int i)
	{
		int ldcI4Value = Method.Body.Instructions[i].GetLdcI4Value();
		int num = Utils.RandomBigInt32();
		int num2 = Utils.RandomBigInt32();
		int value = num2 | num;
		Calculator calculator = new Calculator(ldcI4Value, value);
		Method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
		Method.Body.Instructions[i].Operand = calculator.getResult();
		Method.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(num2));
		Method.Body.Instructions.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(num));
		Method.Body.Instructions.Insert(i + 3, OpCodes.Or.ToInstruction());
		Method.Body.Instructions.Insert(i + 4, calculator.getOpCode().ToInstruction());
		i += 4;
	}

	private void Shr(ref int i)
	{
		int ldcI4Value = Method.Body.Instructions[i].GetLdcI4Value();
		int num = Utils.RandomBigInt32();
		int num2 = Utils.RandomBigInt32();
		int value = num2 >> num;
		Calculator calculator = new Calculator(ldcI4Value, value);
		Method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
		Method.Body.Instructions[i].Operand = calculator.getResult();
		Method.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(num2));
		Method.Body.Instructions.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(num));
		Method.Body.Instructions.Insert(i + 3, OpCodes.Shr.ToInstruction());
		Method.Body.Instructions.Insert(i + 4, calculator.getOpCode().ToInstruction());
		i += 4;
	}

	private void ConditionalMath(ref int i)
	{
		Instruction instruction = Method.Body.Instructions[i];
		Local local = new Local(Method.Module.ImportAsTypeSig(typeof(int)));
		int ldcI4Value = instruction.GetLdcI4Value();
		int num = Utils.RandomBigInt32();
		int num2 = Utils.RandomBigInt32();
		int value;
		int value2;
		if (num > num2)
		{
			value = ldcI4Value;
			value2 = ldcI4Value + ldcI4Value / 3;
		}
		else
		{
			value2 = ldcI4Value;
			value = ldcI4Value + ldcI4Value / 3;
		}
		Method.Body.Variables.Add(local);
		instruction.OpCode = OpCodes.Ldc_I4;
		instruction.Operand = num2;
		Method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Ldc_I4, num));
		Method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Nop));
		Method.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Ldc_I4, value));
		Method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Nop));
		Method.Body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Ldc_I4, value2));
		Method.Body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Stloc, local));
		Method.Body.Instructions.Insert(i + 7, Instruction.Create(OpCodes.Ldloc, local));
		Method.Body.Instructions[i + 2].OpCode = OpCodes.Bgt_S;
		Method.Body.Instructions[i + 2].Operand = Method.Body.Instructions[i + 5];
		Method.Body.Instructions[i + 4].OpCode = OpCodes.Br_S;
		Method.Body.Instructions[i + 4].Operand = Method.Body.Instructions[i + 6];
		i += 7;
	}
}
