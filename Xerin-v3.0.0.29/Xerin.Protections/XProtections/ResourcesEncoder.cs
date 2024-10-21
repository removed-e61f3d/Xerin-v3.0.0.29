using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.MD;
using dnlib.DotNet.Writer;
using XCore.Compression;
using XCore.Context;
using XCore.Decompression;
using XCore.Injection;
using XCore.Mutation;
using XCore.Protections;
using XCore.Utils;
using XCore.VirtHelper;
using XRuntime;

namespace XProtections;

public class ResourcesEncoder : Protection
{
	public override string name => "Resources Encoding";

	public override int number => 15;

	public override void Execute(XContext context)
	{
		string text = Utils.MethodsRenamig() + ".resources";
		int key = Utils.RandomTinyInt32();
		ModuleDefMD moduleDefMD = ModuleDefMD.Load(typeof(ResRuntime).Module);
		TypeDef typeDef = moduleDefMD.ResolveTypeDef(MDToken.ToRID(typeof(ResRuntime).MetadataToken));
		IEnumerable<IDnlibDef> enumerable = InjectHelper.Inject(typeDef, context.Module.GlobalType, context.Module);
		MethodDef methodDef = (MethodDef)enumerable.Single((IDnlibDef method) => method.Name == "Initialize");
		MethodDef methodDef2 = context.Module.GlobalType.FindOrCreateStaticConstructor();
		XMutationHelper xMutationHelper = new XMutationHelper("XMutationClass");
		xMutationHelper.InjectKey(methodDef, 6, text);
		xMutationHelper.InjectKey(methodDef, 5, key);
		foreach (Instruction item in methodDef.Body.Instructions.Where((Instruction V) => V.OpCode == OpCodes.Call && V.Operand.ToString().Contains("decompress")))
		{
			item.Operand = XCore.Decompression.QuickLZ.QLZDecompression;
		}
		methodDef2.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(methodDef));
		foreach (IDnlibDef item2 in enumerable)
		{
			if (!(item2 is MethodDef methodDef3) || (!methodDef3.HasImplMap && !methodDef3.DeclaringType.IsDelegate))
			{
				Utils.MethodsRenamig(item2);
			}
		}
		if (Helper.isEnabled)
		{
			Helper.AddProtection(methodDef.Name);
		}
		string text2 = Utils.MethodsRenamig();
		AssemblyDefUser assemblyDefUser = new AssemblyDefUser(text2, new Version(0, 0));
		assemblyDefUser.Modules.Add(new ModuleDefUser(text2));
		ModuleDef manifestModule = assemblyDefUser.ManifestModule;
		assemblyDefUser.ManifestModule.Kind = ModuleKind.Dll;
		AssemblyRefUser asmRef = new AssemblyRefUser(manifestModule.Assembly);
		for (int i = 0; i < context.Module.Resources.Count; i++)
		{
			if (context.Module.Resources[i] is EmbeddedResource)
			{
				context.Module.Resources[i].Attributes = ManifestResourceAttributes.Private;
				manifestModule.Resources.Add(context.Module.Resources[i] as EmbeddedResource);
				context.Module.Resources.Add(new AssemblyLinkedResource(context.Module.Resources[i].Name, asmRef, context.Module.Resources[i].Attributes));
				context.Module.Resources.RemoveAt(i);
				i--;
			}
		}
		byte[] data;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			ModuleWriterOptions moduleWriterOptions = new ModuleWriterOptions(manifestModule);
			moduleWriterOptions.MetadataOptions.Flags = MetadataFlags.PreserveAll;
			moduleWriterOptions.Cor20HeaderOptions.Flags = ComImageFlags.ILOnly;
			moduleWriterOptions.ModuleKind = ModuleKind.Dll;
			manifestModule.Write(memoryStream, moduleWriterOptions);
			byte[] plainBytes = XCore.Compression.QuickLZ.CompressBytes(memoryStream.ToArray());
			data = Encrypt(plainBytes, key);
		}
		context.Module.Resources.Add(new EmbeddedResource(text, data));
		new HashSet<MethodDef>().Add(methodDef);
		SpamResources.Execute(context);
	}

	private static byte[] Encrypt(byte[] plainBytes, int key)
	{
		Rijndael rijndael = Rijndael.Create();
		rijndael.Key = SHA256.Create().ComputeHash(BitConverter.GetBytes(key));
		rijndael.IV = new byte[16];
		rijndael.Mode = CipherMode.CBC;
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
		cryptoStream.Write(plainBytes, 0, plainBytes.Length);
		cryptoStream.FlushFinalBlock();
		byte[] result = memoryStream.ToArray();
		memoryStream.Close();
		cryptoStream.Close();
		return result;
	}
}
