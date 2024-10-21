using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Confuser.Core;

public class Annotations
{
	private class WeakReferenceComparer : IEqualityComparer<object>
	{
		public static readonly WeakReferenceComparer Instance = new WeakReferenceComparer();

		private WeakReferenceComparer()
		{
		}

		public new bool Equals(object x, object y)
		{
			if (y is WeakReferenceKey && !(x is WeakReference))
			{
				return Equals(y, x);
			}
			WeakReferenceKey weakReferenceKey = x as WeakReferenceKey;
			WeakReferenceKey weakReferenceKey2 = y as WeakReferenceKey;
			if (weakReferenceKey != null && weakReferenceKey2 != null)
			{
				return weakReferenceKey.IsAlive && weakReferenceKey2.IsAlive && weakReferenceKey.Target == weakReferenceKey2.Target;
			}
			if (weakReferenceKey != null && weakReferenceKey2 == null)
			{
				return weakReferenceKey.IsAlive && weakReferenceKey.Target == y;
			}
			if (weakReferenceKey == null && weakReferenceKey2 == null)
			{
				return weakReferenceKey.IsAlive && weakReferenceKey.Target == y;
			}
			throw new Exception();
		}

		public int GetHashCode(object obj)
		{
			if (obj is WeakReferenceKey)
			{
				return ((WeakReferenceKey)obj).HashCode;
			}
			return obj.GetHashCode();
		}
	}

	private class WeakReferenceKey : WeakReference
	{
		public int HashCode { get; private set; }

		public WeakReferenceKey(object target)
			: base(target)
		{
			HashCode = target.GetHashCode();
		}
	}

	private readonly Dictionary<object, ListDictionary> annotations = new Dictionary<object, ListDictionary>(WeakReferenceComparer.Instance);

	public TValue Get<TValue>(object obj, object key, TValue defValue = default(TValue))
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (!annotations.TryGetValue(obj, out var value))
		{
			return defValue;
		}
		if (!value.Contains(key))
		{
			return defValue;
		}
		Type typeFromHandle = typeof(TValue);
		if (typeFromHandle.IsValueType)
		{
			return (TValue)Convert.ChangeType(value[key], typeof(TValue));
		}
		return (TValue)value[key];
	}

	public TValue GetLazy<TValue>(object obj, object key, Func<object, TValue> defValueFactory)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (!annotations.TryGetValue(obj, out var value))
		{
			return defValueFactory(key);
		}
		if (!value.Contains(key))
		{
			return defValueFactory(key);
		}
		Type typeFromHandle = typeof(TValue);
		if (typeFromHandle.IsValueType)
		{
			return (TValue)Convert.ChangeType(value[key], typeof(TValue));
		}
		return (TValue)value[key];
	}

	public TValue GetOrCreate<TValue>(object obj, object key, Func<object, TValue> factory)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (!annotations.TryGetValue(obj, out var value))
		{
			ListDictionary listDictionary2 = (annotations[new WeakReferenceKey(obj)] = new ListDictionary());
			value = listDictionary2;
		}
		if (value.Contains(key))
		{
			Type typeFromHandle = typeof(TValue);
			if (typeFromHandle.IsValueType)
			{
				return (TValue)Convert.ChangeType(value[key], typeof(TValue));
			}
			return (TValue)value[key];
		}
		TValue result;
		value[key] = (result = factory(key));
		return result;
	}

	public void Set<TValue>(object obj, object key, TValue value)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (!annotations.TryGetValue(obj, out var value2))
		{
			ListDictionary listDictionary2 = (annotations[new WeakReferenceKey(obj)] = new ListDictionary());
			value2 = listDictionary2;
		}
		value2[key] = value;
	}

	public void Trim()
	{
		foreach (object item in from kvp in annotations
			where !((WeakReferenceKey)kvp.Key).IsAlive
			select kvp.Key)
		{
			annotations.Remove(item);
		}
	}
}
