using System.Collections;
using System.Collections.Generic;

namespace KoiVM.Core.Helpers.System.Collections.Generic;

internal abstract class IReadOnlyCollectionContract<T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
{
	int IReadOnlyCollection<T>.Count => 0;

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return null;
	}
}
