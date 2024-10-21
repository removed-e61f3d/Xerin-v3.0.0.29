using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;

namespace KoiVM.Core.Helpers.System.Diagnostics.Contracts;

public static class Contract
{
	[Conditional("DEBUG")]
	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Assume(bool condition)
	{
		if (condition)
		{
		}
	}

	[Conditional("DEBUG")]
	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Assume(bool condition, string userMessage)
	{
		if (condition)
		{
		}
	}

	[Conditional("DEBUG")]
	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Assert(bool condition)
	{
		if (condition)
		{
		}
	}

	[Conditional("DEBUG")]
	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Assert(bool condition, string userMessage)
	{
		if (condition)
		{
		}
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Requires(bool condition)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Requires(bool condition, string userMessage)
	{
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Requires<TException>(bool condition) where TException : Exception
	{
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Requires<TException>(bool condition, string userMessage) where TException : Exception
	{
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Ensures(bool condition)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Ensures(bool condition, string userMessage)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void EnsuresOnThrow<TException>(bool condition) where TException : Exception
	{
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void EnsuresOnThrow<TException>(bool condition, string userMessage) where TException : Exception
	{
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static T Result<T>()
	{
		return default(T);
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static T ValueAtReturn<T>(out T value)
	{
		value = default(T);
		return value;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static T OldValue<T>(T value)
	{
		return default(T);
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Invariant(bool condition)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Invariant(bool condition, string userMessage)
	{
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static bool ForAll(int fromInclusive, int toExclusive, Predicate<int> predicate)
	{
		if (fromInclusive > toExclusive)
		{
			throw new ArgumentException("fromInclusive must be less than or equal to toExclusive.");
		}
		if (predicate == null)
		{
			throw new ArgumentNullException("predicate");
		}
		for (int i = fromInclusive; i < toExclusive; i++)
		{
			if (!predicate(i))
			{
				return false;
			}
		}
		return true;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static bool ForAll<T>(IEnumerable<T> collection, Predicate<T> predicate)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		if (predicate == null)
		{
			throw new ArgumentNullException("predicate");
		}
		foreach (T item in collection)
		{
			if (!predicate(item))
			{
				return false;
			}
		}
		return true;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static bool Exists(int fromInclusive, int toExclusive, Predicate<int> predicate)
	{
		if (fromInclusive > toExclusive)
		{
			throw new ArgumentException("fromInclusive must be less than or equal to toExclusive.");
		}
		if (predicate == null)
		{
			throw new ArgumentNullException("predicate");
		}
		for (int i = fromInclusive; i < toExclusive; i++)
		{
			if (predicate(i))
			{
				return true;
			}
		}
		return false;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static bool Exists<T>(IEnumerable<T> collection, Predicate<T> predicate)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		if (predicate == null)
		{
			throw new ArgumentNullException("predicate");
		}
		foreach (T item in collection)
		{
			if (predicate(item))
			{
				return true;
			}
		}
		return false;
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static void EndContractBlock()
	{
	}
}
