using System.Reflection;

namespace XVM.Runtime.Data;

internal class RefInfo
{
	public Module Module;

	private MemberInfo Resolved;

	public int Token;

	public MemberInfo Member()
	{
		MemberInfo result;
		if ((result = Resolved) == null)
		{
			result = (Resolved = Module.ResolveMember(Token));
		}
		return result;
	}
}
