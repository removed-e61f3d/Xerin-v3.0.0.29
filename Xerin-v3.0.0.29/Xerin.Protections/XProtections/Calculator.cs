using System;
using dnlib.DotNet.Emit;

namespace XProtections;

public class Calculator
{
	public static Random rnd = new Random();

	private OpCode cOpCode = null;

	private int result = 0;

	public Calculator(int value, int value2)
	{
		result = Calculate(value, value2);
	}

	public int getResult()
	{
		return result;
	}

	public OpCode getOpCode()
	{
		return cOpCode;
	}

	private int Calculate(int num, int num2)
	{
		int num3 = 0;
		switch (rnd.Next(0, 3))
		{
		case 0:
			num3 = num + num2;
			cOpCode = OpCodes.Sub;
			break;
		case 1:
			num3 = num ^ num2;
			cOpCode = OpCodes.Xor;
			break;
		case 2:
			num3 = num - num2;
			cOpCode = OpCodes.Add;
			break;
		}
		return num3;
	}
}
