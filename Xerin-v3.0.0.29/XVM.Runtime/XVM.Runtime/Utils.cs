using XVM.Runtime.Execution;

namespace XVM.Runtime;

internal static class Utils
{
	public unsafe static uint ReadCompressedUInt(ref byte* ptr)
	{
		uint num = 0u;
		int num2 = 0;
		do
		{
			num |= (uint)((*ptr & 0x7F) << num2);
			num2 += 7;
		}
		while ((*(ptr++) & 0x80u) != 0);
		return num;
	}

	public static uint FromCodedToken(uint codedToken)
	{
		uint num = codedToken >> 3;
		return (codedToken & 7) switch
		{
			1u => num | 0x2000000u, 
			2u => num | 0x1000000u, 
			3u => num | 0x1B000000u, 
			4u => num | 0xA000000u, 
			5u => num | 0x6000000u, 
			6u => num | 0x4000000u, 
			7u => num | 0x2B000000u, 
			_ => num, 
		};
	}

	public static void UpdateFL(VMContext ctx, ulong op1, ulong op2, ulong flResult, ulong result, ref byte fl, byte mask)
	{
		byte b = 0;
		if (result == 0)
		{
			b |= ctx.Data.Constants.FL_ZERO;
		}
		if ((result & 0x80000000u) != 0)
		{
			b |= ctx.Data.Constants.FL_SIGN;
		}
		if (((op1 ^ flResult) & (op2 ^ flResult) & 0x80000000u) != 0)
		{
			b |= ctx.Data.Constants.FL_OVERFLOW;
		}
		if (((op1 ^ ((op1 ^ op2) & (op2 ^ flResult))) & 0x80000000u) != 0)
		{
			b |= ctx.Data.Constants.FL_CARRY;
		}
		fl = (byte)((fl & ~mask) | (b & mask));
	}
}
