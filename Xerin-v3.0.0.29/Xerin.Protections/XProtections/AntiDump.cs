using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Injection;
using XCore.Protections;
using XRuntime;

namespace XProtections;

public class AntiDump : Protection
{
	private static newInjector inj;

	private static MethodDef Initialize;

	public override string name => "Anti Dump";

	public override int number => 8;

	public override void Execute(XContext context)
	{
		inj = new newInjector(context.Module, typeof(AntiDumpRuntime));
		Initialize = inj.FindMember("Initialize") as MethodDef;
		MethodDef methodDef = context.Module.GlobalType.FindOrCreateStaticConstructor();
		methodDef.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(Initialize));
		inj.Rename();
	}
}
