using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using XCore.Context;
using XCore.Injection;
using XCore.Protections;
using XCore.Terminator;
using XCore.VirtHelper;
using XRuntime;

namespace XProtections;

public class IntegrityCheck : Protection
{
	private static newInjector inj;

	private static MethodDef Initialize;

	private static MethodDef MD5;

	public override string name => "Integrity Check";

	public override int number => 13;

	private static string GMD5(byte[] metin)
	{
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] array = mD5CryptoServiceProvider.ComputeHash(metin);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.Append(b.ToString("x2").ToLower());
		}
		return stringBuilder.ToString();
	}

	public static void HashFile(object sender, ModuleWriterEventArgs writer)
	{
		if (writer.Event == ModuleWriterEvent.End)
		{
			StreamReader streamReader = new StreamReader(writer.Writer.DestinationStream);
			BinaryReader binaryReader = new BinaryReader(streamReader.BaseStream);
			binaryReader.BaseStream.Position = 0L;
			byte[] metin = binaryReader.ReadBytes((int)streamReader.BaseStream.Length);
			string s = GMD5(metin);
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			writer.Writer.DestinationStream.Position = writer.Writer.DestinationStream.Length;
			writer.Writer.DestinationStream.Write(bytes, 0, bytes.Length);
		}
	}

	public override void Execute(XContext context)
	{
		context.ModOpts.WriterEvent += HashFile;
		inj = new newInjector(context.Module, typeof(IntegrityCheckRuntime));
		Initialize = inj.FindMember("Initialize") as MethodDef;
		MD5 = inj.FindMember("MD5") as MethodDef;
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
			Helper.AddProtection(MD5.Name);
		}
	}
}
