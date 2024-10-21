using System;

namespace KoiVM.Core;

internal class UnreachableException : SystemException
{
	public UnreachableException()
		: base("Unreachable code reached.")
	{
	}
}
