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

public class AntiEmulate : Protection
{
	private static newInjector inj;

	private static MethodDef Initialize;

	public override string name => "Anti Emulate";

	public override int number => 10;

	public override void Execute(XContext context)
	{
		inj = new newInjector(context.Module, typeof(AntiEmulating));
		Initialize = inj.FindMember("Initialize") as MethodDef;
		MethodDef methodDef = context.Module.GlobalType.FindOrCreateStaticConstructor();
		methodDef.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(Initialize));
		foreach (Instruction item in Initialize.Body.Instructions.Where((Instruction I) => I.OpCode == OpCodes.Ldstr))
		{
			if (item.Operand.ToString() == "you are gay :)")
			{
				item.Operand = "keyauth.win";
			}
		}
		foreach (Instruction item2 in Initialize.Body.Instructions.Where((Instruction V) => V.OpCode == OpCodes.Call && V.Operand.ToString().Contains("Kill")))
		{
			item2.Operand = XCore.Terminator.Terminator.Kill;
		}
		inj.Rename();
		if (Helper.isEnabled)
		{
			Helper.AddProtection(Initialize.Name);
		}
	}
}
