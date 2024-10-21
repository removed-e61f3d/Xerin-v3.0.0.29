using System.Collections;

namespace KoiVM.Core.Helpers.System.Collections;

public interface IStructuralComparable
{
	int CompareTo(object other, IComparer comparer);
}
