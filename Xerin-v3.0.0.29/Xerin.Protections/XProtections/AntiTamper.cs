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

public class AntiTamper : Protection
{
	private static newInjector inj;

	private static MethodDef Initialize;

	private static MethodDef AnalyzeMethod;

	private static MethodDef CheckDynamicCalls;

	private static MethodDef DetectMethodModification;

	public override string name => "Anti Tamper";

	public override int number => 11;

	public override void Execute(XContext context)
	{
		inj = new newInjector(context.Module, typeof(XRuntime.AntiTamper));
		Initialize = inj.FindMember("Initialize") as MethodDef;
		AnalyzeMethod = inj.FindMember("AnalyzeMethod") as MethodDef;
		CheckDynamicCalls = inj.FindMember("CheckDynamicCalls") as MethodDef;
		DetectMethodModification = inj.FindMember("DetectMethodModification") as MethodDef;
		MethodDef methodDef = context.Module.GlobalType.FindOrCreateStaticConstructor();
		methodDef.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(Initialize));
		foreach (Instruction item in Initialize.Body.Instructions.Where((Instruction V) => V.OpCode == OpCodes.Call && V.Operand.ToString().Contains("Kill")))
		{
			item.Operand = XCore.Terminator.Terminator.Kill;
		}
		inj.Rename();
		if (Helper.isEnabled)
		{
			Helper.AddProtection(Initialize.Name);
		}
	}
}
