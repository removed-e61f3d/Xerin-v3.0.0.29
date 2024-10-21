using System;
using System.Diagnostics;

namespace KoiVM.Core.Helpers.System.Collections.Generic;

internal class HashSetDebugView<T>
{
	private HashSet<T> set;

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	public T[] Items => set.ToArray();

	public HashSetDebugView(HashSet<T> set)
	{
		if (set == null)
		{
			throw new ArgumentNullException("set");
		}
		this.set = set;
	}
}
