using dnlib.DotNet;
using XCore.Context;
using XCore.Injection;
using XRuntime;

namespace XCore.Decompression;

public class QuickLZ
{
	private static newInjector inj;

	public static MethodDef QLZDecompression;

	public static bool isNeeded;

	public void injectQuickLZ(XContext context)
	{
		inj = new newInjector(context.Module, typeof(QuickLZDecompression));
		QLZDecompression = inj.FindMember("decompress") as MethodDef;
		inj.Rename();
	}
}
