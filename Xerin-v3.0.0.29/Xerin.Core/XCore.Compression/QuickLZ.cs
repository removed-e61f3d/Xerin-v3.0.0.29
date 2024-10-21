using System;
using System.IO;
using System.IO.Compression;

namespace XCore.Compression;

public static class QuickLZ
{
	public const int QLZ_VERSION_MINOR = 5;

	public const int QLZ_STREAMING_BUFFER = 0;

	private const int HASH_VALUES = 4096;

	private const int UNCONDITIONAL_MATCHLEN = 6;

	private const int CWORD_LEN = 4;

	private const int QLZ_POINTERS_1 = 1;

	public static byte[] CompressBytes2(byte[] data)
	{
		MemoryStream memoryStream = new MemoryStream();
		using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionLevel.Optimal))
		{
			deflateStream.Write(data, 0, data.Length);
		}
		return memoryStream.ToArray();
	}

	private static int headerLen(byte[] source)
	{
		return ((source[0] & 2) == 2) ? 9 : 3;
	}

	public static int sizeCompressed(byte[] source)
	{
		if (headerLen(source) == 9)
		{
			return source[1] | (source[2] << 8) | (source[3] << 16) | (source[4] << 24);
		}
		return source[1];
	}

	private static void write_header(byte[] dst, int level, bool compressible, int size_compressed, int size_decompressed)
	{
		dst[0] = (byte)(2u | (compressible ? 1u : 0u));
		dst[0] |= (byte)(level << 2);
		dst[0] |= 64;
		dst[0] |= 0;
		fast_write(dst, 1, size_decompressed, 4);
		fast_write(dst, 5, size_compressed, 4);
	}

	public static byte[] CompressBytes(byte[] source)
	{
		int num = 3;
		int i = 0;
		int num2 = 13;
		uint num3 = 2147483648u;
		int i2 = 9;
		byte[] array = new byte[source.Length + 400];
		int[] array2 = new int[4096];
		byte[] array3 = new byte[4096];
		int num4 = 0;
		int num5 = source.Length - 6 - 4 - 1;
		int num6 = 0;
		int[,] array4 = new int[4096, 16];
		if (source.Length == 0)
		{
			return new byte[0];
		}
		if (i <= num5)
		{
			num4 = source[i] | (source[i + 1] << 8) | (source[i + 2] << 16);
		}
		byte[] array5;
		while (true)
		{
			if (i <= num5)
			{
				if ((num3 & 1) == 1)
				{
					if (i > source.Length >> 1 && num2 > i - (i >> 5))
					{
						break;
					}
					fast_write(array, i2, (int)(num3 >> 1) | int.MinValue, 4);
					i2 = num2;
					num2 += 4;
					num3 = 2147483648u;
				}
				if (num == 1)
				{
					int num7 = ((num4 >> 12) ^ num4) & 0xFFF;
					int num8 = array4[num7, 0];
					int num9 = array2[num7] ^ num4;
					array2[num7] = num4;
					array4[num7, 0] = i;
					if (num9 == 0 && array3[num7] != 0 && (i - num8 > 2 || (i == num8 + 1 && num6 >= 3 && i > 3 && source[i] == source[i - 3] && source[i] == source[i - 2] && source[i] == source[i - 1] && source[i] == source[i + 1] && source[i] == source[i + 2])))
					{
						num3 = (num3 >> 1) | 0x80000000u;
						if (source[num8 + 3] != source[i + 3])
						{
							int num10 = 1 | (num7 << 4);
							array[num2] = (byte)num10;
							array[num2 + 1] = (byte)(num10 >> 8);
							i += 3;
							num2 += 2;
						}
						else
						{
							int num11 = i;
							int num12 = ((source.Length - 4 - i + 1 - 1 > 255) ? 255 : (source.Length - 4 - i + 1 - 1));
							i += 4;
							if (source[num8 + i - num11] == source[i])
							{
								i++;
								if (source[num8 + i - num11] == source[i])
								{
									for (i++; source[num8 + (i - num11)] == source[i] && i - num11 < num12; i++)
									{
									}
								}
							}
							int num13 = i - num11;
							num7 <<= 4;
							if (num13 < 18)
							{
								int num14 = num7 | (num13 - 2);
								array[num2] = (byte)num14;
								array[num2 + 1] = (byte)(num14 >> 8);
								num2 += 2;
							}
							else
							{
								fast_write(array, num2, num7 | (num13 << 16), 3);
								num2 += 3;
							}
						}
						num4 = source[i] | (source[i + 1] << 8) | (source[i + 2] << 16);
						num6 = 0;
					}
					else
					{
						num6++;
						array3[num7] = 1;
						array[num2] = source[i];
						num3 >>= 1;
						i++;
						num2++;
						num4 = ((num4 >> 8) & 0xFFFF) | (source[i + 2] << 16);
					}
					continue;
				}
				num4 = source[i] | (source[i + 1] << 8) | (source[i + 2] << 16);
				int num15 = ((source.Length - 4 - i + 1 - 1 > 255) ? 255 : (source.Length - 4 - i + 1 - 1));
				int num16 = ((num4 >> 12) ^ num4) & 0xFFF;
				byte b = array3[num16];
				int num17 = 0;
				int num18 = 0;
				int num19;
				for (int j = 0; j < 16 && b > j; j++)
				{
					num19 = array4[num16, j];
					if ((byte)num4 == source[num19] && (byte)(num4 >> 8) == source[num19 + 1] && (byte)(num4 >> 16) == source[num19 + 2] && num19 < i - 2)
					{
						int k;
						for (k = 3; source[num19 + k] == source[i + k] && k < num15; k++)
						{
						}
						if (k > num17 || (k == num17 && num19 > num18))
						{
							num18 = num19;
							num17 = k;
						}
					}
				}
				num19 = num18;
				array4[num16, b & 0xF] = i;
				b++;
				array3[num16] = b;
				if (num17 >= 3 && i - num19 < 131071)
				{
					int num20 = i - num19;
					for (int l = 1; l < num17; l++)
					{
						num4 = source[i + l] | (source[i + l + 1] << 8) | (source[i + l + 2] << 16);
						num16 = ((num4 >> 12) ^ num4) & 0xFFF;
						array4[num16, array3[num16]++ & 0xF] = i + l;
					}
					i += num17;
					num3 = (num3 >> 1) | 0x80000000u;
					if (num17 == 3 && num20 <= 63)
					{
						fast_write(array, num2, num20 << 2, 1);
						num2++;
					}
					else if (num17 == 3 && num20 <= 16383)
					{
						fast_write(array, num2, (num20 << 2) | 1, 2);
						num2 += 2;
					}
					else if (num17 <= 18 && num20 <= 1023)
					{
						fast_write(array, num2, (num17 - 3 << 2) | (num20 << 6) | 2, 2);
						num2 += 2;
					}
					else if (num17 <= 33)
					{
						fast_write(array, num2, (num17 - 2 << 2) | (num20 << 7) | 3, 3);
						num2 += 3;
					}
					else
					{
						fast_write(array, num2, (num17 - 3 << 7) | (num20 << 15) | 3, 4);
						num2 += 4;
					}
					num6 = 0;
				}
				else
				{
					array[num2] = source[i];
					num3 >>= 1;
					i++;
					num2++;
				}
				continue;
			}
			while (i <= source.Length - 1)
			{
				if ((num3 & 1) == 1)
				{
					fast_write(array, i2, (int)(num3 >> 1) | int.MinValue, 4);
					i2 = num2;
					num2 += 4;
					num3 = 2147483648u;
				}
				array[num2] = source[i];
				i++;
				num2++;
				num3 >>= 1;
			}
			while ((num3 & 1) != 1)
			{
				num3 >>= 1;
			}
			fast_write(array, i2, (int)(num3 >> 1) | int.MinValue, 4);
			write_header(array, num, compressible: true, source.Length, num2);
			array5 = new byte[num2];
			Array.Copy(array, array5, num2);
			return array5;
		}
		array5 = new byte[source.Length + 9];
		write_header(array5, num, compressible: false, source.Length, source.Length + 9);
		Array.Copy(source, 0, array5, 9, source.Length);
		return array5;
	}

	private static void fast_write(byte[] a, int i, int value, int numbytes)
	{
		for (int j = 0; j < numbytes; j++)
		{
			a[i + j] = (byte)(value >> j * 8);
		}
	}
}
