using System.Collections.Generic;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Compression;
using XCore.Context;
using XCore.Injection;
using XCore.Protections;
using XRuntime;

namespace XCore.Embed;

public class Embeder : Protection
{
	public static bool isEmbed = false;

	public static bool isEmptyList = false;

	public static List<string> dlls = new List<string>();

	private newInjector injector = null;

	public override string name => "Embed dlls";

	public override int number => 14;

	public override void Execute(XContext context)
	{
		if (isEmptyList)
		{
			return;
		}
		injector = new newInjector(context.Module, typeof(embedRuntime));
		MethodDef method = injector.FindMember("AppStart") as MethodDef;
		MethodDef methodDef = context.Module.GlobalType.FindOrCreateStaticConstructor();
		methodDef.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, method));
		foreach (string dll in dlls)
		{
			byte[] data = File.ReadAllBytes(dll);
			context.Module.Resources.Add(new EmbeddedResource(Path.GetFileNameWithoutExtension(dll), QuickLZ.CompressBytes2(data)));
		}
		injector.Rename();
	}
}
