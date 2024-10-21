using dnlib.DotNet;

namespace XCore.Optimizer;

internal class EqualityComparer
{
	private static EqualityComparer _singleton = new EqualityComparer();

	public static EqualityComparer Singleton => _singleton;

	public bool Equals(IMethod mrefA, IMethod mrefB)
	{
		return mrefA.Equals(mrefB);
	}

	public int GetHashCode(IMethod mref)
	{
		return mref.FullName.GetHashCode();
	}
}
