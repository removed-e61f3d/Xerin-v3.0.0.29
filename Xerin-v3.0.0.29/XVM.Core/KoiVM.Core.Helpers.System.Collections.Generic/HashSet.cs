#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using KoiVM.Core.Helpers.System.Diagnostics.Contracts;

namespace KoiVM.Core.Helpers.System.Collections.Generic;

[Serializable]
[DebuggerTypeProxy(typeof(HashSetDebugView<>))]
[DebuggerDisplay("Count = {Count}")]
[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public class HashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ISerializable, IDeserializationCallback, ISet<T>, IReadOnlyCollection<T>
{
	internal struct ElementCount
	{
		internal int uniqueCount;

		internal int unfoundCount;
	}

	internal struct Slot
	{
		internal int hashCode;

		internal int next;

		internal T value;
	}

	[Serializable]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
	{
		private HashSet<T> set;

		private int index;

		private int version;

		private T current;

		public T Current => current;

		object IEnumerator.Current
		{
			get
			{
				if (index == 0 || index == set.m_lastIndex + 1)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
				}
				return Current;
			}
		}

		internal Enumerator(HashSet<T> set)
		{
			this.set = set;
			index = 0;
			version = set.m_version;
			current = default(T);
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			if (version != set.m_version)
			{
				throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
			}
			while (index < set.m_lastIndex)
			{
				if (set.m_slots[index].hashCode >= 0)
				{
					current = set.m_slots[index].value;
					index++;
					return true;
				}
				index++;
			}
			index = set.m_lastIndex + 1;
			current = default(T);
			return false;
		}

		void IEnumerator.Reset()
		{
			if (version != set.m_version)
			{
				throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
			}
			index = 0;
			current = default(T);
		}
	}

	private const int Lower31BitMask = int.MaxValue;

	private const int StackAllocThreshold = 100;

	private const int ShrinkThreshold = 3;

	private const string CapacityName = "Capacity";

	private const string ElementsName = "Elements";

	private const string ComparerName = "Comparer";

	private const string VersionName = "Version";

	private int[] m_buckets;

	private Slot[] m_slots;

	private int m_count;

	private int m_lastIndex;

	private int m_freeList;

	private IEqualityComparer<T> m_comparer;

	private int m_version;

	private SerializationInfo m_siInfo;

	public T this[int index]
	{
		get
		{
			return m_slots[index].value;
		}
		set
		{
			m_slots[index].value = value;
			m_version++;
		}
	}

	public int Count => m_count;

	bool ICollection<T>.IsReadOnly => false;

	public IEqualityComparer<T> Comparer => m_comparer;

	public HashSet()
		: this((IEqualityComparer<T>)EqualityComparer<T>.Default)
	{
	}

	public HashSet(int capacity)
		: this(capacity, (IEqualityComparer<T>)EqualityComparer<T>.Default)
	{
	}

	public HashSet(IEqualityComparer<T> comparer)
	{
		if (comparer == null)
		{
			comparer = EqualityComparer<T>.Default;
		}
		m_comparer = comparer;
		m_lastIndex = 0;
		m_count = 0;
		m_freeList = -1;
		m_version = 0;
	}

	public HashSet(IEnumerable<T> collection)
		: this(collection, (IEqualityComparer<T>)EqualityComparer<T>.Default)
	{
	}

	public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
		: this(comparer)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		if (collection is HashSet<T> hashSet && AreEqualityComparersEqual(this, hashSet))
		{
			CopyFrom(hashSet);
			return;
		}
		Initialize((collection is ICollection<T> collection2) ? collection2.Count : 0);
		UnionWith(collection);
		if (m_count > 0 && m_slots.Length / m_count > 3)
		{
			TrimExcess();
		}
	}

	private void CopyFrom(HashSet<T> source)
	{
		int count = source.m_count;
		if (count == 0)
		{
			return;
		}
		int num = source.m_buckets.Length;
		int num2 = HashHelpers.ExpandPrime(count + 1);
		if (num2 >= num)
		{
			m_buckets = (int[])source.m_buckets.Clone();
			m_slots = (Slot[])source.m_slots.Clone();
			m_lastIndex = source.m_lastIndex;
			m_freeList = source.m_freeList;
		}
		else
		{
			int lastIndex = source.m_lastIndex;
			Slot[] slots = source.m_slots;
			Initialize(count);
			int num3 = 0;
			for (int i = 0; i < lastIndex; i++)
			{
				int hashCode = slots[i].hashCode;
				if (hashCode >= 0)
				{
					AddValue(num3, hashCode, slots[i].value);
					num3++;
				}
			}
			Debug.Assert(num3 == count);
			m_lastIndex = num3;
		}
		m_count = count;
	}

	protected HashSet(SerializationInfo info, StreamingContext context)
	{
		m_siInfo = info;
	}

	public HashSet(int capacity, IEqualityComparer<T> comparer)
		: this(comparer)
	{
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		if (capacity > 0)
		{
			Initialize(capacity);
		}
	}

	void ICollection<T>.Add(T item)
	{
		AddIfNotPresent(item);
	}

	public void Clear()
	{
		if (m_lastIndex > 0)
		{
			Debug.Assert(m_buckets != null, "m_buckets was null but m_lastIndex > 0");
			Array.Clear(m_slots, 0, m_lastIndex);
			Array.Clear(m_buckets, 0, m_buckets.Length);
			m_lastIndex = 0;
			m_count = 0;
			m_freeList = -1;
		}
		m_version++;
	}

	public bool Contains(T item)
	{
		if (m_buckets != null)
		{
			int num = InternalGetHashCode(item);
			for (int num2 = m_buckets[num % m_buckets.Length] - 1; num2 >= 0; num2 = m_slots[num2].next)
			{
				if (m_slots[num2].hashCode == num && m_comparer.Equals(m_slots[num2].value, item))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		CopyTo(array, arrayIndex, m_count);
	}

	public bool Remove(T item)
	{
		if (m_buckets != null)
		{
			int num = InternalGetHashCode(item);
			int num2 = num % m_buckets.Length;
			int num3 = -1;
			for (int num4 = m_buckets[num2] - 1; num4 >= 0; num4 = m_slots[num4].next)
			{
				if (m_slots[num4].hashCode == num && m_comparer.Equals(m_slots[num4].value, item))
				{
					if (num3 < 0)
					{
						m_buckets[num2] = m_slots[num4].next + 1;
					}
					else
					{
						m_slots[num3].next = m_slots[num4].next;
					}
					m_slots[num4].hashCode = -1;
					m_slots[num4].value = default(T);
					m_slots[num4].next = m_freeList;
					m_count--;
					m_version++;
					if (m_count == 0)
					{
						m_lastIndex = 0;
						m_freeList = -1;
					}
					else
					{
						m_freeList = num4;
					}
					return true;
				}
				num3 = num4;
			}
		}
		return false;
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(this);
	}

	[SecurityCritical]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("Version", m_version);
		info.AddValue("Comparer", m_comparer, typeof(IEqualityComparer<T>));
		info.AddValue("Capacity", (m_buckets != null) ? m_buckets.Length : 0);
		if (m_buckets != null)
		{
			T[] array = new T[m_count];
			CopyTo(array);
			info.AddValue("Elements", array, typeof(T[]));
		}
	}

	public virtual void OnDeserialization(object sender)
	{
		if (m_siInfo == null)
		{
			return;
		}
		int @int = m_siInfo.GetInt32("Capacity");
		m_comparer = (IEqualityComparer<T>)m_siInfo.GetValue("Comparer", typeof(IEqualityComparer<T>));
		m_freeList = -1;
		if (@int != 0)
		{
			m_buckets = new int[@int];
			m_slots = new Slot[@int];
			T[] array = (T[])m_siInfo.GetValue("Elements", typeof(T[]));
			if (array == null)
			{
				throw new SerializationException(SR.GetString("Serialization_MissingKeys"));
			}
			for (int i = 0; i < array.Length; i++)
			{
				AddIfNotPresent(array[i]);
			}
		}
		else
		{
			m_buckets = null;
		}
		m_version = m_siInfo.GetInt32("Version");
		m_siInfo = null;
	}

	public bool Add(T item)
	{
		return AddIfNotPresent(item);
	}

	public bool TryGetValue(T equalValue, out T actualValue)
	{
		if (m_buckets != null)
		{
			int num = InternalIndexOf(equalValue);
			if (num >= 0)
			{
				actualValue = m_slots[num].value;
				return true;
			}
		}
		actualValue = default(T);
		return false;
	}

	public void UnionWith(IEnumerable<T> other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		foreach (T item in other)
		{
			AddIfNotPresent(item);
		}
	}

	public void IntersectWith(IEnumerable<T> other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (m_count == 0)
		{
			return;
		}
		if (other is ICollection<T> collection)
		{
			if (collection.Count == 0)
			{
				Clear();
				return;
			}
			if (other is HashSet<T> hashSet && AreEqualityComparersEqual(this, hashSet))
			{
				IntersectWithHashSetWithSameEC(hashSet);
				return;
			}
		}
		IntersectWithEnumerable(other);
	}

	public void ExceptWith(IEnumerable<T> other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (m_count == 0)
		{
			return;
		}
		if (other == this)
		{
			Clear();
			return;
		}
		foreach (T item in other)
		{
			Remove(item);
		}
	}

	public void SymmetricExceptWith(IEnumerable<T> other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (m_count == 0)
		{
			UnionWith(other);
		}
		else if (other == this)
		{
			Clear();
		}
		else if (other is HashSet<T> hashSet && AreEqualityComparersEqual(this, hashSet))
		{
			SymmetricExceptWithUniqueHashSet(hashSet);
		}
		else
		{
			SymmetricExceptWithEnumerable(other);
		}
	}

	public bool IsSubsetOf(IEnumerable<T> other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (m_count == 0)
		{
			return true;
		}
		if (other is HashSet<T> hashSet && AreEqualityComparersEqual(this, hashSet))
		{
			if (m_count > hashSet.Count)
			{
				return false;
			}
			return IsSubsetOfHashSetWithSameEC(hashSet);
		}
		ElementCount elementCount = CheckUniqueAndUnfoundElements(other, returnIfUnfound: false);
		return elementCount.uniqueCount == m_count && elementCount.unfoundCount >= 0;
	}

	public bool IsProperSubsetOf(IEnumerable<T> other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (other is ICollection<T> collection)
		{
			if (m_count == 0)
			{
				return collection.Count > 0;
			}
			if (other is HashSet<T> hashSet && AreEqualityComparersEqual(this, hashSet))
			{
				if (m_count >= hashSet.Count)
				{
					return false;
				}
				return IsSubsetOfHashSetWithSameEC(hashSet);
			}
		}
		ElementCount elementCount = CheckUniqueAndUnfoundElements(other, returnIfUnfound: false);
		return elementCount.uniqueCount == m_count && elementCount.unfoundCount > 0;
	}

	public bool IsSupersetOf(IEnumerable<T> other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (other is ICollection<T> collection)
		{
			if (collection.Count == 0)
			{
				return true;
			}
			if (other is HashSet<T> hashSet && AreEqualityComparersEqual(this, hashSet) && hashSet.Count > m_count)
			{
				return false;
			}
		}
		return ContainsAllElements(other);
	}

	public bool IsProperSupersetOf(IEnumerable<T> other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (m_count == 0)
		{
			return false;
		}
		if (other is ICollection<T> collection)
		{
			if (collection.Count == 0)
			{
				return true;
			}
			if (other is HashSet<T> hashSet && AreEqualityComparersEqual(this, hashSet))
			{
				if (hashSet.Count >= m_count)
				{
					return false;
				}
				return ContainsAllElements(hashSet);
			}
		}
		ElementCount elementCount = CheckUniqueAndUnfoundElements(other, returnIfUnfound: true);
		return elementCount.uniqueCount < m_count && elementCount.unfoundCount == 0;
	}

	public bool Overlaps(IEnumerable<T> other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (m_count == 0)
		{
			return false;
		}
		foreach (T item in other)
		{
			if (Contains(item))
			{
				return true;
			}
		}
		return false;
	}

	public bool SetEquals(IEnumerable<T> other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (other is HashSet<T> hashSet && AreEqualityComparersEqual(this, hashSet))
		{
			if (m_count != hashSet.Count)
			{
				return false;
			}
			return ContainsAllElements(hashSet);
		}
		if (other is ICollection<T> collection && m_count == 0 && collection.Count > 0)
		{
			return false;
		}
		ElementCount elementCount = CheckUniqueAndUnfoundElements(other, returnIfUnfound: true);
		return elementCount.uniqueCount == m_count && elementCount.unfoundCount == 0;
	}

	public void CopyTo(T[] array)
	{
		CopyTo(array, 0, m_count);
	}

	public void CopyTo(T[] array, int arrayIndex, int count)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (arrayIndex < 0)
		{
			throw new ArgumentOutOfRangeException("arrayIndex", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
		}
		if (arrayIndex > array.Length || count > array.Length - arrayIndex)
		{
			throw new ArgumentException(SR.GetString("Arg_ArrayPlusOffTooSmall"));
		}
		int num = 0;
		for (int i = 0; i < m_lastIndex; i++)
		{
			if (num >= count)
			{
				break;
			}
			if (m_slots[i].hashCode >= 0)
			{
				array[arrayIndex + num] = m_slots[i].value;
				num++;
			}
		}
	}

	public int RemoveWhere(Predicate<T> match)
	{
		if (match == null)
		{
			throw new ArgumentNullException("match");
		}
		int num = 0;
		for (int i = 0; i < m_lastIndex; i++)
		{
			if (m_slots[i].hashCode >= 0)
			{
				T value = m_slots[i].value;
				if (match(value) && Remove(value))
				{
					num++;
				}
			}
		}
		return num;
	}

	public void TrimExcess()
	{
		Debug.Assert(m_count >= 0, "m_count is negative");
		if (m_count == 0)
		{
			m_buckets = null;
			m_slots = null;
			m_version++;
			return;
		}
		Debug.Assert(m_buckets != null, "m_buckets was null but m_count > 0");
		int prime = HashHelpers.GetPrime(m_count);
		Slot[] array = new Slot[prime];
		int[] array2 = new int[prime];
		int num = 0;
		for (int i = 0; i < m_lastIndex; i++)
		{
			if (m_slots[i].hashCode >= 0)
			{
				array[num] = m_slots[i];
				int num2 = array[num].hashCode % prime;
				array[num].next = array2[num2] - 1;
				array2[num2] = num + 1;
				num++;
			}
		}
		Debug.Assert(array.Length <= m_slots.Length, "capacity increased after TrimExcess");
		m_lastIndex = num;
		m_slots = array;
		m_buckets = array2;
		m_freeList = -1;
	}

	public static IEqualityComparer<HashSet<T>> CreateSetComparer()
	{
		return new HashSetEqualityComparer<T>();
	}

	private void Initialize(int capacity)
	{
		Debug.Assert(m_buckets == null, "Initialize was called but m_buckets was non-null");
		int prime = HashHelpers.GetPrime(capacity);
		m_buckets = new int[prime];
		m_slots = new Slot[prime];
	}

	private void IncreaseCapacity()
	{
		Debug.Assert(m_buckets != null, "IncreaseCapacity called on a set with no elements");
		int num = HashHelpers.ExpandPrime(m_count);
		if (num <= m_count)
		{
			throw new ArgumentException(SR.GetString("Arg_HSCapacityOverflow"));
		}
		SetCapacity(num, forceNewHashCodes: false);
	}

	private void SetCapacity(int newSize, bool forceNewHashCodes)
	{
		Contract.Assert(HashHelpers.IsPrime(newSize), "New size is not prime!");
		Contract.Assert(m_buckets != null, "SetCapacity called on a set with no elements");
		Slot[] array = new Slot[newSize];
		if (m_slots != null)
		{
			Array.Copy(m_slots, 0, array, 0, m_lastIndex);
		}
		if (forceNewHashCodes)
		{
			for (int i = 0; i < m_lastIndex; i++)
			{
				if (array[i].hashCode != -1)
				{
					array[i].hashCode = InternalGetHashCode(array[i].value);
				}
			}
		}
		int[] array2 = new int[newSize];
		for (int j = 0; j < m_lastIndex; j++)
		{
			int num = array[j].hashCode % newSize;
			array[j].next = array2[num] - 1;
			array2[num] = j + 1;
		}
		m_slots = array;
		m_buckets = array2;
	}

	private bool AddIfNotPresent(T value)
	{
		if (m_buckets == null)
		{
			Initialize(0);
		}
		int num = InternalGetHashCode(value);
		int num2 = num % m_buckets.Length;
		for (int num3 = m_buckets[num % m_buckets.Length] - 1; num3 >= 0; num3 = m_slots[num3].next)
		{
			if (m_slots[num3].hashCode == num && m_comparer.Equals(m_slots[num3].value, value))
			{
				return false;
			}
		}
		int num4;
		if (m_freeList >= 0)
		{
			num4 = m_freeList;
			m_freeList = m_slots[num4].next;
		}
		else
		{
			if (m_lastIndex == m_slots.Length)
			{
				IncreaseCapacity();
				num2 = num % m_buckets.Length;
			}
			num4 = m_lastIndex;
			m_lastIndex++;
		}
		m_slots[num4].hashCode = num;
		m_slots[num4].value = value;
		m_slots[num4].next = m_buckets[num2] - 1;
		m_buckets[num2] = num4 + 1;
		m_count++;
		m_version++;
		return true;
	}

	private void AddValue(int index, int hashCode, T value)
	{
		int num = hashCode % m_buckets.Length;
		Debug.Assert(InternalGetHashCode(value) == hashCode);
		for (int num2 = m_buckets[num] - 1; num2 >= 0; num2 = m_slots[num2].next)
		{
			Debug.Assert(!m_comparer.Equals(m_slots[num2].value, value));
		}
		Debug.Assert(m_freeList == -1);
		m_slots[index].hashCode = hashCode;
		m_slots[index].value = value;
		m_slots[index].next = m_buckets[num] - 1;
		m_buckets[num] = index + 1;
	}

	private bool ContainsAllElements(IEnumerable<T> other)
	{
		foreach (T item in other)
		{
			if (!Contains(item))
			{
				return false;
			}
		}
		return true;
	}

	private bool IsSubsetOfHashSetWithSameEC(HashSet<T> other)
	{
		using (Enumerator enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				T current = enumerator.Current;
				if (!other.Contains(current))
				{
					return false;
				}
			}
		}
		return true;
	}

	private void IntersectWithHashSetWithSameEC(HashSet<T> other)
	{
		for (int i = 0; i < m_lastIndex; i++)
		{
			if (m_slots[i].hashCode >= 0)
			{
				T value = m_slots[i].value;
				if (!other.Contains(value))
				{
					Remove(value);
				}
			}
		}
	}

	[SecuritySafeCritical]
	private unsafe void IntersectWithEnumerable(IEnumerable<T> other)
	{
		Debug.Assert(m_buckets != null, "m_buckets shouldn't be null; callers should check first");
		int lastIndex = m_lastIndex;
		int num = BitHelper.ToIntArrayLength(lastIndex);
		BitHelper bitHelper;
		if (num <= 100)
		{
			int* bitArrayPtr = stackalloc int[num];
			bitHelper = new BitHelper(bitArrayPtr, num);
		}
		else
		{
			int[] bitArray = new int[num];
			bitHelper = new BitHelper(bitArray, num);
		}
		foreach (T item in other)
		{
			int num2 = InternalIndexOf(item);
			if (num2 >= 0)
			{
				bitHelper.MarkBit(num2);
			}
		}
		for (int i = 0; i < lastIndex; i++)
		{
			if (m_slots[i].hashCode >= 0 && !bitHelper.IsMarked(i))
			{
				Remove(m_slots[i].value);
			}
		}
	}

	private int InternalIndexOf(T item)
	{
		Debug.Assert(m_buckets != null, "m_buckets was null; callers should check first");
		int num = InternalGetHashCode(item);
		for (int num2 = m_buckets[num % m_buckets.Length] - 1; num2 >= 0; num2 = m_slots[num2].next)
		{
			if (m_slots[num2].hashCode == num && m_comparer.Equals(m_slots[num2].value, item))
			{
				return num2;
			}
		}
		return -1;
	}

	private void SymmetricExceptWithUniqueHashSet(HashSet<T> other)
	{
		foreach (T item in other)
		{
			if (!Remove(item))
			{
				AddIfNotPresent(item);
			}
		}
	}

	[SecuritySafeCritical]
	private unsafe void SymmetricExceptWithEnumerable(IEnumerable<T> other)
	{
		int lastIndex = m_lastIndex;
		int num = BitHelper.ToIntArrayLength(lastIndex);
		BitHelper bitHelper;
		BitHelper bitHelper2;
		if (num <= 50)
		{
			int* bitArrayPtr = stackalloc int[num];
			bitHelper = new BitHelper(bitArrayPtr, num);
			int* bitArrayPtr2 = stackalloc int[num];
			bitHelper2 = new BitHelper(bitArrayPtr2, num);
		}
		else
		{
			int[] bitArray = new int[num];
			bitHelper = new BitHelper(bitArray, num);
			int[] bitArray2 = new int[num];
			bitHelper2 = new BitHelper(bitArray2, num);
		}
		foreach (T item in other)
		{
			int location = 0;
			if (AddOrGetLocation(item, out location))
			{
				bitHelper2.MarkBit(location);
			}
			else if (location < lastIndex && !bitHelper2.IsMarked(location))
			{
				bitHelper.MarkBit(location);
			}
		}
		for (int i = 0; i < lastIndex; i++)
		{
			if (bitHelper.IsMarked(i))
			{
				Remove(m_slots[i].value);
			}
		}
	}

	private bool AddOrGetLocation(T value, out int location)
	{
		Debug.Assert(m_buckets != null, "m_buckets is null, callers should have checked");
		int num = InternalGetHashCode(value);
		int num2 = num % m_buckets.Length;
		for (int num3 = m_buckets[num % m_buckets.Length] - 1; num3 >= 0; num3 = m_slots[num3].next)
		{
			if (m_slots[num3].hashCode == num && m_comparer.Equals(m_slots[num3].value, value))
			{
				location = num3;
				return false;
			}
		}
		int num4;
		if (m_freeList >= 0)
		{
			num4 = m_freeList;
			m_freeList = m_slots[num4].next;
		}
		else
		{
			if (m_lastIndex == m_slots.Length)
			{
				IncreaseCapacity();
				num2 = num % m_buckets.Length;
			}
			num4 = m_lastIndex;
			m_lastIndex++;
		}
		m_slots[num4].hashCode = num;
		m_slots[num4].value = value;
		m_slots[num4].next = m_buckets[num2] - 1;
		m_buckets[num2] = num4 + 1;
		m_count++;
		m_version++;
		location = num4;
		return true;
	}

	[SecuritySafeCritical]
	private unsafe ElementCount CheckUniqueAndUnfoundElements(IEnumerable<T> other, bool returnIfUnfound)
	{
		ElementCount result = default(ElementCount);
		if (m_count == 0)
		{
			int num = 0;
			using (IEnumerator<T> enumerator = other.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					T current = enumerator.Current;
					num++;
				}
			}
			result.uniqueCount = 0;
			result.unfoundCount = num;
			return result;
		}
		Debug.Assert(m_buckets != null && m_count > 0, "m_buckets was null but count greater than 0");
		int lastIndex = m_lastIndex;
		int num2 = BitHelper.ToIntArrayLength(lastIndex);
		BitHelper bitHelper;
		if (num2 <= 100)
		{
			int* bitArrayPtr = stackalloc int[num2];
			bitHelper = new BitHelper(bitArrayPtr, num2);
		}
		else
		{
			int[] bitArray = new int[num2];
			bitHelper = new BitHelper(bitArray, num2);
		}
		int num3 = 0;
		int num4 = 0;
		foreach (T item in other)
		{
			int num5 = InternalIndexOf(item);
			if (num5 >= 0)
			{
				if (!bitHelper.IsMarked(num5))
				{
					bitHelper.MarkBit(num5);
					num4++;
				}
			}
			else
			{
				num3++;
				if (returnIfUnfound)
				{
					break;
				}
			}
		}
		result.uniqueCount = num4;
		result.unfoundCount = num3;
		return result;
	}

	internal T[] ToArray()
	{
		T[] array = new T[Count];
		CopyTo(array);
		return array;
	}

	internal static bool HashSetEquals(HashSet<T> set1, HashSet<T> set2, IEqualityComparer<T> comparer)
	{
		if (set1 == null)
		{
			return set2 == null;
		}
		if (set2 == null)
		{
			return false;
		}
		if (AreEqualityComparersEqual(set1, set2))
		{
			if (set1.Count != set2.Count)
			{
				return false;
			}
			foreach (T item in set2)
			{
				if (!set1.Contains(item))
				{
					return false;
				}
			}
			return true;
		}
		foreach (T item2 in set2)
		{
			bool flag = false;
			foreach (T item3 in set1)
			{
				if (comparer.Equals(item2, item3))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	private static bool AreEqualityComparersEqual(HashSet<T> set1, HashSet<T> set2)
	{
		return set1.Comparer.Equals(set2.Comparer);
	}

	private int InternalGetHashCode(T item)
	{
		if (item == null)
		{
			return 0;
		}
		return m_comparer.GetHashCode(item) & 0x7FFFFFFF;
	}
}
