#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using dnlib.DotNet.Writer;

namespace KoiVM.Core.Services;

internal class RandomGenerator
{
	private static readonly byte[] primes = new byte[7] { 7, 11, 23, 37, 43, 59, 71 };

	private static readonly RNGCryptoServiceProvider _RNG = new RNGCryptoServiceProvider();

	private readonly SHA256Managed sha256 = new SHA256Managed();

	private int mixIndex;

	private byte[] state;

	private int stateFilled;

	private int seedLen;

	internal RandomGenerator()
	{
		byte[] array = new byte[32];
		_RNG.GetBytes(array);
		state = _SHA256((byte[])array.Clone());
		seedLen = array.Length;
		stateFilled = 32;
		mixIndex = 0;
	}

	internal RandomGenerator(int length)
	{
		byte[] array = new byte[(length == 0) ? 32 : length];
		_RNG.GetBytes(array);
		state = _SHA256((byte[])array.Clone());
		seedLen = array.Length;
		stateFilled = 32;
		mixIndex = 0;
	}

	internal RandomGenerator(string seed)
	{
		byte[] array = _SHA256((byte[])((!string.IsNullOrEmpty(seed)) ? Encoding.UTF8.GetBytes(seed) : Guid.NewGuid().ToByteArray()).Clone());
		for (int i = 0; i < 32; i++)
		{
			array[i] *= primes[i % primes.Length];
			array = _SHA256(array);
		}
		state = array;
		seedLen = array.Length;
		stateFilled = 32;
		mixIndex = 0;
	}

	internal RandomGenerator(byte[] seed)
	{
		state = (byte[])seed.Clone();
		seedLen = seed.Length;
		stateFilled = 32;
		mixIndex = 0;
	}

	private static byte[] _SHA256(byte[] buffer)
	{
		SHA256Managed sHA256Managed = new SHA256Managed();
		return sHA256Managed.ComputeHash(buffer);
	}

	private void NextState()
	{
		for (int i = 0; i < 32; i++)
		{
			state[i] ^= primes[mixIndex = (mixIndex + 1) % primes.Length];
		}
		state = sha256.ComputeHash(state);
		stateFilled = 32;
	}

	public void NextBytes(byte[] buffer, int offset, int length)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		if (buffer.Length - offset < length)
		{
			throw new ArgumentException("Invalid offset or length.");
		}
		while (length > 0)
		{
			if (length >= stateFilled)
			{
				Buffer.BlockCopy(state, 32 - stateFilled, buffer, offset, stateFilled);
				offset += stateFilled;
				length -= stateFilled;
				stateFilled = 0;
			}
			else
			{
				Buffer.BlockCopy(state, 32 - stateFilled, buffer, offset, length);
				stateFilled -= length;
				length = 0;
			}
			if (stateFilled == 0)
			{
				NextState();
			}
		}
	}

	public byte NextByte()
	{
		byte result = state[32 - stateFilled];
		stateFilled--;
		if (stateFilled == 0)
		{
			NextState();
		}
		return result;
	}

	public string NextString(int length)
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				char value = Convert.ToChar(Convert.ToInt32(Math.Floor(32m + (decimal)NextInt32(122) - 32m)));
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}
		catch
		{
		}
		return string.Empty;
	}

	public string NextHexString(int length, bool large = false)
	{
		if (length.ToString().Contains("5"))
		{
			throw new Exception("5 is an unacceptable number!");
		}
		try
		{
			string element = "qwertyuıopğüasdfghjklşizxcvbnmöçQWERTYUIOPĞÜASDFGHJKLŞİZXCVBNMÖÇ0123456789/*-.:,;!'^+%&/()=?_~|\\}][{½$#£>";
			string s2 = new string((from s in Enumerable.Repeat(element, length / 2)
				select s[NextInt32(s.Length)]).ToArray());
			if (!large)
			{
				return BitConverter.ToString(Encoding.Default.GetBytes(s2)).Replace("-", string.Empty).ToLower();
			}
			if (large)
			{
				return BitConverter.ToString(Encoding.Default.GetBytes(s2)).Replace("-", string.Empty);
			}
		}
		catch
		{
		}
		return string.Empty;
	}

	public string NextHexString(bool large = false)
	{
		return NextHexString(8, large);
	}

	public string NextString()
	{
		return NextString(seedLen);
	}

	public byte[] NextBytes(int length)
	{
		byte[] array = new byte[length];
		NextBytes(array, 0, length);
		return array;
	}

	public byte[] NextBytes()
	{
		byte[] array = new byte[seedLen];
		NextBytes(array, 0, seedLen);
		return array;
	}

	public int NextInt32()
	{
		return BitConverter.ToInt32(NextBytes(4), 0);
	}

	public int NextInt32(int max)
	{
		return (int)(NextUInt32() % max);
	}

	public int NextInt32(int min, int max)
	{
		if (max <= min)
		{
			return min;
		}
		return min + (int)(NextUInt32() % (max - min));
	}

	public uint NextUInt32()
	{
		return BitConverter.ToUInt32(NextBytes(4), 0);
	}

	public uint NextUInt32(uint max)
	{
		return NextUInt32() % max;
	}

	public double NextDouble()
	{
		return (double)NextUInt32() / 4294967296.0;
	}

	public double NextDouble(double min, double max)
	{
		return NextDouble() * (max - min) + min;
	}

	public bool NextBoolean()
	{
		byte b = state[32 - stateFilled];
		stateFilled--;
		if (stateFilled == 0)
		{
			NextState();
		}
		return b % 2 == 0;
	}

	public void Shuffle<T>(IList<T> list)
	{
		for (int num = list.Count - 1; num > 1; num--)
		{
			int index = NextInt32(num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	public void Shuffle<T>(MDTable<T> table) where T : struct
	{
		if (!table.IsEmpty)
		{
			for (uint num = (uint)table.Rows; num > 2; num--)
			{
				uint num2 = NextUInt32(num - 1) + 1;
				Debug.Assert(num2 >= 1, "k >= 1");
				Debug.Assert(num2 < num, "k < i");
				Debug.Assert(num2 <= table.Rows, "k <= table.Rows");
				T value = table[num2];
				table[num2] = table[num];
				table[num] = value;
			}
		}
	}
}
