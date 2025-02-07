using System.Collections;
using System.Runtime.InteropServices;

namespace KoiVM.Core.Helpers.System.Collections;

[ComVisible(true)]
public interface IList : ICollection, IEnumerable
{
	object this[int index] { get; set; }

	bool IsReadOnly { get; }

	bool IsFixedSize { get; }

	int Add(object value);

	bool Contains(object value);

	void Clear();

	int IndexOf(object value);

	void Insert(int index, object value);

	void Remove(object value);

	void RemoveAt(int index);
}
