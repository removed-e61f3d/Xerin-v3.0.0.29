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

public class AntiDebug : Protection
{
	private static newInjector inj;

	private static MethodDef Initialize;

	private static MethodDef Detector;

	public override string name => "Anti Debug";

	public override int number => 9;

	public override void Execute(XContext context)
	{
		inj = new newInjector(context.Module, typeof(AntiDebugRuntime));
		Initialize = inj.FindMember("Initialize") as MethodDef;
		Detector = inj.FindMember("Detector") as MethodDef;
		MethodDef methodDef = context.Module.GlobalType.FindOrCreateStaticConstructor();
		methodDef.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(Initialize));
		foreach (Instruction item in Initialize.Body.Instructions.Where((Instruction V) => V.OpCode == OpCodes.Call && V.Operand.ToString().Contains("Kill")))
		{
			item.Operand = XCore.Terminator.Terminator.Kill;
		}
		foreach (Instruction item2 in Detector.Body.Instructions.Where((Instruction V) => V.OpCode == OpCodes.Call && V.Operand.ToString().Contains("Kill")))
		{
			item2.Operand = XCore.Terminator.Terminator.Kill;
		}
		inj.Rename();
		if (Helper.isEnabled)
		{
			Helper.AddProtection(Initialize.Name);
			Helper.AddProtection(Detector.Name);
		}
	}
}
