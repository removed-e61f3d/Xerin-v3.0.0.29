using dnlib.DotNet;
using XCore.Context;
using XCore.Injection;
using XRuntime;

namespace XCore.Terminator;

public class Terminator
{
	private static newInjector inj;

	public static MethodDef Kill;

	public static bool isNeeded;

	public void injectKill(XContext context)
	{
		inj = new newInjector(context.Module, typeof(XRuntime.Terminator));
		Kill = inj.FindMember("Kill") as MethodDef;
		inj.Rename();
	}
}
