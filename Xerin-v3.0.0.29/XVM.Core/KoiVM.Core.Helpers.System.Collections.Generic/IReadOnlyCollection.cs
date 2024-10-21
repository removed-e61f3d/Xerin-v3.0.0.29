using System.Collections;
using System.Collections.Generic;
using KoiVM.Core.Helpers.System.Runtime.CompilerServices;

namespace KoiVM.Core.Helpers.System.Collections.Generic;

[TypeDependency("System.SZArrayHelper")]
public interface IReadOnlyCollection<T> : IEnumerable<T>, IEnumerable
{
	int Count { get; }
}
