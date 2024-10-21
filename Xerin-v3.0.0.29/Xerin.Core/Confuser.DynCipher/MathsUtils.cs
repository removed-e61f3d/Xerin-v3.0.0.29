namespace Confuser.DynCipher;

public static class MathsUtils
{
	public static ulong modInv(ulong num, ulong mod)
	{
		ulong num2 = mod;
		ulong num3 = num % mod;
		ulong num4 = 0uL;
		ulong num5 = 1uL;
		while (true)
		{
			switch (num3)
			{
			default:
				num4 += num2 / num3 * num5;
				num2 %= num3;
				switch (num2)
				{
				default:
					goto IL_0052;
				case 0uL:
					break;
				case 1uL:
					return mod - num4;
				}
				break;
			case 1uL:
				return num5;
			case 0uL:
				break;
			}
			break;
			IL_0052:
			num5 += num3 / num2 * num4;
			num3 %= num2;
		}
		return 0uL;
	}

	public static uint modInv(uint num)
	{
		return (uint)modInv(num, 4294967296uL);
	}

	public static byte modInv(byte num)
	{
		return (byte)modInv(num, 256uL);
	}
}
