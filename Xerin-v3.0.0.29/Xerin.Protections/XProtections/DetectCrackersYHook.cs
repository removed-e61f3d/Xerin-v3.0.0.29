using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Injection;
using XCore.Mutation;
using XCore.Protections;
using XCore.Terminator;
using XCore.Utils;
using XCore.VirtHelper;
using XProtections.AntiCrack.Globals;
using XRuntime;

namespace XProtections;

public class DetectCrackersYHook : Protection
{
	private static newInjector inj;

	private static MethodDef Init;

	private static MethodDef DoWork;

	private static MethodDef SendMSG;

	private static MethodDef Capture;

	private static MethodDef CrossAppDomainSerializer;

	private static MethodDef GetMotherBoardSerialNo;

	private static MethodDef SendIMSG;

	private static MethodDef CalculateMD5Hash;

	private static MethodDef eBSOD;

	public override string name => "Anti Crack";

	public override int number => 7;

	public override void Execute(XContext context)
	{
		inj = new newInjector(context.Module, typeof(AntiCrackingWithHook));
		Init = inj.FindMember("Init") as MethodDef;
		DoWork = inj.FindMember("DoWork") as MethodDef;
		SendMSG = inj.FindMember("SendMSG") as MethodDef;
		Capture = inj.FindMember("Capture") as MethodDef;
		CrossAppDomainSerializer = inj.FindMember("CrossAppDomainSerializer") as MethodDef;
		GetMotherBoardSerialNo = inj.FindMember("GetMotherBoardSerialNo") as MethodDef;
		SendIMSG = inj.FindMember("SendIMSG") as MethodDef;
		CalculateMD5Hash = inj.FindMember("CalculateMD5Hash") as MethodDef;
		eBSOD = inj.FindMember("eBSOD") as MethodDef;
		XMutationHelper xMutationHelper = new XMutationHelper("XMutationClass");
		MethodDef methodDef = context.Module.GlobalType.FindOrCreateStaticConstructor();
		methodDef.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(Init));
		foreach (Instruction item in DoWork.Body.Instructions.Where((Instruction V) => V.OpCode == OpCodes.Call && V.Operand.ToString().Contains("Kill")))
		{
			item.Operand = XCore.Terminator.Terminator.Kill;
		}
		xMutationHelper.InjectKey(DoWork, 1, Utils.MethodsRenamig());
		if (Global.Normal)
		{
			xMutationHelper.InjectKey(DoWork, 2, "1");
		}
		else
		{
			xMutationHelper.InjectKey(DoWork, 2, "0");
		}
		if (Global.Exclude)
		{
			xMutationHelper.InjectKey(DoWork, 3, Global.excludeString);
		}
		else
		{
			xMutationHelper.InjectKey(DoWork, 3, "Hi'I'm_Empty_String;')");
		}
		xMutationHelper.InjectKey(DoWork, 4, Global.webhookLink);
		if (Global.Silent)
		{
			xMutationHelper.InjectKey(DoWork, 5, "1");
		}
		else
		{
			xMutationHelper.InjectKey(DoWork, 5, "0");
		}
		if (Global.Bsod)
		{
			xMutationHelper.InjectKey(DoWork, 0, "1");
		}
		else
		{
			xMutationHelper.InjectKey(DoWork, 0, "0");
		}
		if (!string.IsNullOrEmpty(Global.customMessage))
		{
			xMutationHelper.InjectKey(DoWork, 7, Global.customMessage);
		}
		else
		{
			xMutationHelper.InjectKey(DoWork, 7, "Detected cracking process or Cheat engine is detected!");
		}
		inj.Rename();
		MethodDef[] methods = new MethodDef[2] { DoWork, SendIMSG };
		DoWork.excludeMethod(context.Module);
		Init.excludeMethod(context.Module);
		SendMSG.excludeMethod(context.Module);
		Capture.excludeMethod(context.Module);
		GetMotherBoardSerialNo.excludeMethod(context.Module);
		CrossAppDomainSerializer.excludeMethod(context.Module);
		CalculateMD5Hash.excludeMethod(context.Module);
		SendIMSG.excludeMethod(context.Module);
		eBSOD.excludeMethod(context.Module);
		if (Helper.isEnabled)
		{
			stillWorkingOn2.EncodeFor(context, methods);
			SecondMutationStage.executeFor(DoWork);
			Helper.AddProtection(Init.Name);
			Helper.AddProtection(DoWork.Name);
			Helper.AddProtection(SendMSG.Name);
			Helper.AddProtection(Capture.Name);
			Helper.AddProtection(GetMotherBoardSerialNo.Name);
			Helper.AddProtection(CrossAppDomainSerializer.Name);
			Helper.AddProtection(CalculateMD5Hash.Name);
			Helper.AddProtection(SendIMSG.Name);
			Helper.AddProtection(eBSOD.Name);
		}
		else
		{
			stillWorkingOn2.EncodeFor(context, methods);
			SecondMutationStage.executeFor(DoWork);
			SecondMutationStage.executeFor(SendIMSG);
		}
	}
}
