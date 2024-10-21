using System;
using System.Text;

namespace Confuser.DynCipher.Generation;

public class x86Instruction
{
	public x86OpCode OpCode { get; set; }

	public Ix86Operand[] Operands { get; set; }

	public static x86Instruction Create(x86OpCode opCode, params Ix86Operand[] operands)
	{
		x86Instruction x86Instruction2 = new x86Instruction();
		x86Instruction2.OpCode = opCode;
		x86Instruction2.Operands = operands;
		return x86Instruction2;
	}

	public byte[] Assemble()
	{
		switch (OpCode)
		{
		case (x86OpCode)0:
		{
			if (Operands.Length != 2)
			{
				throw new InvalidOperationException();
			}
			if (Operands[0] is x86RegisterOperand && Operands[1] is x86RegisterOperand)
			{
				byte[] array2 = new byte[2] { 137, 192 };
				array2[1] |= (byte)((int)(Operands[1] as x86RegisterOperand).Register << 3);
				array2[1] |= (byte)(Operands[0] as x86RegisterOperand).Register;
				return array2;
			}
			if (!(Operands[0] is x86RegisterOperand) || !(Operands[1] is x86ImmediateOperand))
			{
				throw new NotSupportedException();
			}
			byte[] array3 = new byte[5] { 184, 0, 0, 0, 0 };
			array3[0] |= (byte)(Operands[0] as x86RegisterOperand).Register;
			Buffer.BlockCopy(BitConverter.GetBytes((Operands[1] as x86ImmediateOperand).Immediate), 0, array3, 1, 4);
			return array3;
		}
		case x86OpCode.ADD:
		{
			if (Operands.Length != 2)
			{
				throw new InvalidOperationException();
			}
			if (Operands[0] is x86RegisterOperand && Operands[1] is x86RegisterOperand)
			{
				byte[] array12 = new byte[2] { 1, 192 };
				array12[1] |= (byte)((int)(Operands[1] as x86RegisterOperand).Register << 3);
				array12[1] |= (byte)(Operands[0] as x86RegisterOperand).Register;
				return array12;
			}
			if (!(Operands[0] is x86RegisterOperand) || !(Operands[1] is x86ImmediateOperand))
			{
				throw new NotSupportedException();
			}
			byte[] array13 = new byte[6] { 129, 192, 0, 0, 0, 0 };
			array13[1] |= (byte)(Operands[0] as x86RegisterOperand).Register;
			Buffer.BlockCopy(BitConverter.GetBytes((Operands[1] as x86ImmediateOperand).Immediate), 0, array13, 2, 4);
			return array13;
		}
		case (x86OpCode)2:
		{
			if (Operands.Length != 2)
			{
				throw new InvalidOperationException();
			}
			if (Operands[0] is x86RegisterOperand && Operands[1] is x86RegisterOperand)
			{
				byte[] array4 = new byte[2] { 41, 192 };
				array4[1] |= (byte)((int)(Operands[1] as x86RegisterOperand).Register << 3);
				array4[1] |= (byte)(Operands[0] as x86RegisterOperand).Register;
				return array4;
			}
			if (!(Operands[0] is x86RegisterOperand) || !(Operands[1] is x86ImmediateOperand))
			{
				throw new NotSupportedException();
			}
			byte[] array5 = new byte[6] { 129, 232, 0, 0, 0, 0 };
			array5[1] |= (byte)(Operands[0] as x86RegisterOperand).Register;
			Buffer.BlockCopy(BitConverter.GetBytes((Operands[1] as x86ImmediateOperand).Immediate), 0, array5, 2, 4);
			return array5;
		}
		case x86OpCode.IMUL:
		{
			if (Operands.Length != 2)
			{
				throw new InvalidOperationException();
			}
			if (Operands[0] is x86RegisterOperand && Operands[1] is x86RegisterOperand)
			{
				byte[] array7 = new byte[3];
				array7[0] = 15;
				array7[1] = 175;
				array7[1] = 192;
				array7[1] |= (byte)((int)(Operands[1] as x86RegisterOperand).Register << 3);
				array7[1] |= (byte)(Operands[0] as x86RegisterOperand).Register;
				return array7;
			}
			if (!(Operands[0] is x86RegisterOperand) || !(Operands[1] is x86ImmediateOperand))
			{
				throw new NotSupportedException();
			}
			byte[] array8 = new byte[6] { 105, 192, 0, 0, 0, 0 };
			array8[1] |= (byte)((int)(Operands[0] as x86RegisterOperand).Register << 3);
			array8[1] |= (byte)(Operands[0] as x86RegisterOperand).Register;
			Buffer.BlockCopy(BitConverter.GetBytes((Operands[1] as x86ImmediateOperand).Immediate), 0, array8, 2, 4);
			return array8;
		}
		default:
			throw new NotSupportedException();
		case x86OpCode.NEG:
		{
			if (Operands.Length != 1)
			{
				throw new InvalidOperationException();
			}
			if (!(Operands[0] is x86RegisterOperand))
			{
				throw new NotSupportedException();
			}
			byte[] array6 = new byte[2] { 247, 216 };
			array6[1] |= (byte)(Operands[0] as x86RegisterOperand).Register;
			return array6;
		}
		case (x86OpCode)6:
		{
			if (Operands.Length != 1)
			{
				throw new InvalidOperationException();
			}
			if (!(Operands[0] is x86RegisterOperand))
			{
				throw new NotSupportedException();
			}
			byte[] array11 = new byte[2] { 247, 208 };
			array11[1] |= (byte)(Operands[0] as x86RegisterOperand).Register;
			return array11;
		}
		case x86OpCode.XOR:
		{
			if (Operands.Length != 2)
			{
				throw new InvalidOperationException();
			}
			if (Operands[0] is x86RegisterOperand && Operands[1] is x86RegisterOperand)
			{
				byte[] array9 = new byte[2] { 49, 192 };
				array9[1] |= (byte)((int)(Operands[1] as x86RegisterOperand).Register << 3);
				array9[1] |= (byte)(Operands[0] as x86RegisterOperand).Register;
				return array9;
			}
			if (!(Operands[0] is x86RegisterOperand) || !(Operands[1] is x86ImmediateOperand))
			{
				throw new NotSupportedException();
			}
			byte[] array10 = new byte[6] { 129, 240, 0, 0, 0, 0 };
			array10[1] |= (byte)(Operands[0] as x86RegisterOperand).Register;
			Buffer.BlockCopy(BitConverter.GetBytes((Operands[1] as x86ImmediateOperand).Immediate), 0, array10, 2, 4);
			return array10;
		}
		case (x86OpCode)8:
		{
			if (Operands.Length != 1)
			{
				throw new InvalidOperationException();
			}
			if (!(Operands[0] is x86RegisterOperand))
			{
				throw new NotSupportedException();
			}
			byte[] array = new byte[1] { 88 };
			array[0] |= (byte)(Operands[0] as x86RegisterOperand).Register;
			return array;
		}
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(OpCode);
		for (int i = 0; i < Operands.Length; i++)
		{
			stringBuilder.AppendFormat("{0}{1}", (i == 0) ? " " : ", ", Operands[i]);
		}
		return stringBuilder.ToString();
	}
}
