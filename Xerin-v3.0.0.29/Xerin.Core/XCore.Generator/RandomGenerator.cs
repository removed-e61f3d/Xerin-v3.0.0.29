#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using dnlib.DotNet.Writer;

namespace XCore.Generator;

public class RandomGenerator
{
	private static readonly byte[] primes = new byte[7] { 7, 11, 23, 37, 43, 59, 71 };

	private readonly SHA256Managed sha256 = new SHA256Managed();

	private int mixIndex;

	private byte[] state;

	private int stateFilled;

	private readonly byte[] seed2;

	public string SeedString { get; }

	public RandomGenerator(string id, string seed)
	{
		SeedString = (string.IsNullOrEmpty(seed) ? Guid.NewGuid().ToString() : seed);
		seed2 = Seed(SeedString);
		if (string.IsNullOrEmpty(id))
		{
			throw new ArgumentNullException("id");
		}
		byte[] array = seed2;
		byte[] array2 = RandomGeneratorUtils.SHA256(Encoding.UTF8.GetBytes(id));
		for (int i = 0; i < 32; i++)
		{
			array[i] ^= array2[i];
		}
		state = (byte[])seed2.Clone();
		stateFilled = 32;
		mixIndex = 0;
	}

	internal static byte[] Seed(string seed)
	{
		byte[] array = RandomGeneratorUtils.SHA256((!string.IsNullOrEmpty(seed)) ? Encoding.UTF8.GetBytes(seed) : Guid.NewGuid().ToByteArray());
		for (int i = 0; i < 32; i++)
		{
			array[i] *= primes[i % primes.Length];
			array = RandomGeneratorUtils.SHA256(array);
		}
		return array;
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

	public byte[] NextBytes(int length)
	{
		byte[] array = new byte[length];
		NextBytes(array, 0, length);
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
