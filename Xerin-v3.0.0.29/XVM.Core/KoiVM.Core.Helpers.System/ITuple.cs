using System.Collections;
using System.Text;

namespace KoiVM.Core.Helpers.System;

internal interface ITuple
{
	int Size { get; }

	string ToString(StringBuilder sb);

	int GetHashCode(IEqualityComparer comparer);
}
