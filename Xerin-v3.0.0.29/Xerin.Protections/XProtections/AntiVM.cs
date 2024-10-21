using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Injection;
using XCore.Protections;
using XCore.Terminator;
using XCore.VirtHelper;
using XRuntime;

namespace XProtections;

public class AntiVM : Protection
{
	private static newInjector inj;

	private static MethodDef Initialize;

	private static MethodDef MessageDictionary;

	public override string name => "Anti VM";

	public override int number => 12;

	public override void Execute(XContext context)
	{
		inj = new newInjector(context.Module, typeof(AntiVMRuntime));
		Initialize = inj.FindMember("Initialize") as MethodDef;
		MessageDictionary = inj.FindMember("MessageDictionary") as MethodDef;
		MethodDef methodDef = context.Module.GlobalType.FindOrCreateStaticConstructor();
		methodDef.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(Initialize));
		foreach (Instruction item in Initialize.Body.Instructions.Where((Instruction V) => V.OpCode == OpCodes.Call && V.Operand.ToString().Contains("Kill")))
		{
			item.Operand = XCore.Terminator.Terminator.Kill;
		}
		inj.Rename();
		MethodDef[] methods = new MethodDef[1] { MessageDictionary };
		stillWorkingOn2.EncodeFor(context, methods);
		if (Helper.isEnabled)
		{
			Helper.AddProtection(Initialize.Name);
		}
	}
}
