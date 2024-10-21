using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Compression;
using XCore.Context;
using XCore.Decompression;
using XCore.Generator;
using XCore.Injection;
using XCore.Protections;
using XCore.Utils;
using XCore.VirtHelper;
using XProtections.Strings;
using XRuntime;

namespace XProtections;

public class eConstants : Protection
{
	private static readonly List<string> stringsList = new List<string>();

	private static string resName = "";

	private static string Key = "";

	private static string IV = "";

	private static MethodDef streamToByteArray = null;

	private static MethodDef extractResources = null;

	private static MethodDef decryptStrings = null;

	private static FieldDef stringsListFld = null;

	private static FieldDef strdic = null;

	private static newInjector inj = null;

	public static string[] ToIgnore = new string[3] { "Call", "Extract", "DecompressBytes" };

	public override string name => "Strings Encoding";

	public override int number => 6;

	private static byte[] convertListToByteArray(List<string> list)
	{
		string s = string.Join(Environment.NewLine, list);
		return Encoding.UTF8.GetBytes(s);
	}

	private static void Inject(ModuleDefMD Module)
	{
		inj = new newInjector(Module, typeof(StringsRuntime));
		streamToByteArray = inj.FindMember("Read") as MethodDef;
		extractResources = inj.FindMember("Extract") as MethodDef;
		foreach (Instruction item in extractResources.Body.Instructions.Where((Instruction V) => V.OpCode == OpCodes.Call && V.Operand.ToString().Contains("decompress")))
		{
			item.Operand = XCore.Decompression.QuickLZ.QLZDecompression;
		}
		foreach (Instruction item2 in extractResources.Body.Instructions.Where((Instruction I) => I.OpCode == OpCodes.Ldstr))
		{
			if (item2.Operand.ToString() == "CallMeInx")
			{
				item2.Operand = resName;
			}
		}
		decryptStrings = inj.FindMember("Call") as MethodDef;
		stringsListFld = inj.FindMember("stringsList") as FieldDef;
		strdic = inj.FindMember("hTable") as FieldDef;
		foreach (Instruction item3 in decryptStrings.Body.Instructions.Where((Instruction I) => I.OpCode == OpCodes.Ldstr))
		{
			if (item3.Operand.ToString() == "Key")
			{
				item3.Operand = Key;
			}
			if (item3.Operand.ToString() == "IV")
			{
				item3.Operand = IV;
			}
		}
		MethodDef[] methods = new MethodDef[3] { streamToByteArray, extractResources, decryptStrings };
		inj.injectMethods(string.Empty, GGeneration.GenerateGuidStartingWithLetter(), Module, methods);
		new replaceObfuscator(Module, replaceObfuscator.Mode.Simple).ExecuteFor(decryptStrings);
		new replaceObfuscator(Module, replaceObfuscator.Mode.Simple).ExecuteFor(extractResources);
	}

	private static string Encrypt(string plainText)
	{
		Aes aes = Aes.Create();
		aes.Key = Encoding.UTF8.GetBytes(Key);
		aes.IV = Encoding.UTF8.GetBytes(IV);
		ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
		byte[] inArray;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
			{
				using StreamWriter streamWriter = new StreamWriter(stream);
				streamWriter.Write(plainText);
			}
			inArray = memoryStream.ToArray();
		}
		return Convert.ToBase64String(inArray);
	}

	public override void Execute(XContext xContext)
	{
		PreStrings.Execute(xContext);
		Key = Utils.RandomString(32);
		IV = Utils.RandomString(16);
		resName = Utils.MethodsRenamig();
		Inject(xContext.Module);
		HashSet<string> hashSet = new HashSet<string>();
		foreach (TypeDef item3 in from x in xContext.Module.GetTypes()
			where x.HasMethods && !x.IsGlobalModuleType && x.Namespace != "Costura"
			select x)
		{
			foreach (FieldDef item4 in item3.Fields.Where((FieldDef f) => f.IsStatic && f.FieldType.FullName == "System.String" && f.IsLiteral && f.HasConstant))
			{
				item4.Constant.Value = Encrypt(item4.Constant.Value.ToString());
			}
			foreach (MethodDef item5 in item3.Methods.Where((MethodDef x) => x.HasBody && x.Body.HasInstructions))
			{
				if (ToIgnore.Contains(item5.Name.ToString()))
				{
					continue;
				}
				item5.Body.SimplifyMacros(item5.Parameters);
				item5.Body.SimplifyBranches();
				IList<Instruction> instructions = item5.Body.Instructions;
				for (int i = 0; i < instructions.Count; i++)
				{
					if (instructions[i].OpCode == OpCodes.Ldstr)
					{
						string text = instructions[i].Operand.ToString();
						if (!hashSet.Contains(text))
						{
							string item = Encrypt(text);
							hashSet.Add(item);
							stringsList.Add(item);
							item5.Body.Instructions[i].OpCode = OpCodes.Ldsfld;
							item5.Body.Instructions[i].Operand = stringsListFld;
							item5.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Ldsfld, strdic));
							item5.Body.Instructions.Insert(i + 2, new Instruction(OpCodes.Ldc_I4, stringsList.LastIndexOf(item)));
							item5.Body.Instructions.Insert(i + 3, new Instruction(OpCodes.Ldc_I4, 1));
							item5.Body.Instructions.Insert(i + 4, new Instruction(OpCodes.Mul));
							item5.Body.Instructions.Insert(i + 5, new Instruction(OpCodes.Ldc_I4, 0));
							item5.Body.Instructions.Insert(i + 6, new Instruction(OpCodes.Shl));
							item5.Body.Instructions.Insert(i + 7, new Instruction(OpCodes.Call, decryptStrings));
							i += 7;
						}
					}
				}
				item5.Body.OptimizeMacros();
			}
		}
		byte[] data = XCore.Compression.QuickLZ.CompressBytes(convertListToByteArray(stringsList));
		EmbeddedResource item2 = new EmbeddedResource(resName, data);
		xContext.Module.Resources.Add(item2);
		MethodDef methodDef = xContext.Module.GlobalType.FindOrCreateStaticConstructor();
		methodDef.Body.Instructions.Insert(methodDef.Body.Instructions.Count - 1, OpCodes.Call.ToInstruction(extractResources));
		stringsList.Clear();
		inj.Rename();
		if (Helper.isEnabled)
		{
			Helper.AddProtection(decryptStrings.Name);
		}
	}
}
