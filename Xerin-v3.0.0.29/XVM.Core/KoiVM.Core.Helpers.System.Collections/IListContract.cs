using System;
using System.Collections;

namespace KoiVM.Core.Helpers.System.Collections;

internal abstract class IListContract : IList, ICollection, IEnumerable
{
	object IList.this[int index]
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	bool IList.IsFixedSize => false;

	bool IList.IsReadOnly => false;

	bool ICollection.IsSynchronized => false;

	int ICollection.Count => 0;

	object ICollection.SyncRoot => null;

	int IList.Add(object value)
	{
		return 0;
	}

	void IList.Clear()
	{
	}

	bool IList.Contains(object value)
	{
		return false;
	}

	void ICollection.CopyTo(Array array, int startIndex)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return null;
	}

	int IList.IndexOf(object value)
	{
		return 0;
	}

	void IList.Insert(int index, object value)
	{
	}

	void IList.Remove(object value)
	{
	}

	void IList.RemoveAt(int index)
	{
	}
}
