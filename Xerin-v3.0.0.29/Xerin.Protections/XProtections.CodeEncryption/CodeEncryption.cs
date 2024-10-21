#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using XCore.CE;
using XCore.Context;
using XCore.Generator;
using XCore.Mutation;
using XCore.Protections;
using XCore.Utils;

namespace XProtections.CodeEncryption;

public class CodeEncryption : Protection
{
	private static IKeyDeriver deriver;

	private static List<MethodDef> methods;

	private static uint name1;

	private static uint name2;

	private static XCore.Generator.RandomGenerator random;

	private static readonly uint[] c = new uint[1];

	private static readonly uint[] v = new uint[1];

	private static readonly uint[] x = new uint[1];

	private static readonly uint[] z = new uint[1];

	public override string name => "Code Encryption";

	public override int number => 17;

	public override void Execute(XContext ctx)
	{
		random = new XCore.Generator.RandomGenerator(GGeneration.RandomString(1), GGeneration.RandomString(1));
		z[0] = random.NextUInt32();
		x[0] = random.NextUInt32();
		c[0] = random.NextUInt32();
		v[0] = random.NextUInt32();
		name1 = random.NextUInt32() & 0x7F7F7F7Fu;
		name2 = random.NextUInt32() & 0x7F7F7F7Fu;
		deriver = new NormalDeriver();
		deriver.Init();
		methods = (from x in ctx.Module.GetTypes().SelectMany((TypeDef sd) => sd.Methods).ToList()
			where x.HasBody && x.DeclaringType != x.Module.GlobalType
			select x).ToList();
		AssemblyDef assemblyDef = AssemblyDef.Load(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Xerin.Runtime.dll");
		TypeDef typeDef = assemblyDef.ManifestModule.Find("XRuntime.CodeEncryptionRuntime", isReflectionName: false);
		IEnumerable<IDnlibDef> enumerable = InjectHelper.Inject(typeDef, ctx.Module.GlobalType, ctx.Module);
		MethodDef methodDef = enumerable.OfType<MethodDef>().Single((MethodDef method) => method.Name == "Initialize");
		methodDef.Body.SimplifyMacros(methodDef.Parameters);
		List<Instruction> list = methodDef.Body.Instructions.ToList();
		for (int i = 0; i < list.Count; i++)
		{
			Instruction instruction = list[i];
			if (instruction.OpCode == OpCodes.Ldtoken)
			{
				instruction.Operand = ctx.Module.GlobalType;
			}
			else if (instruction.OpCode == OpCodes.Call)
			{
				IMethod method2 = (IMethod)instruction.Operand;
				if (method2.DeclaringType.Name == "XMutationClass" && method2.Name == "Crypt")
				{
					Instruction instruction2 = list[i - 2];
					Instruction instruction3 = list[i - 1];
					Debug.Assert(instruction2.OpCode == OpCodes.Ldloc && instruction3.OpCode == OpCodes.Ldloc);
					list.RemoveAt(i);
					list.RemoveAt(i - 1);
					list.RemoveAt(i - 2);
					list.InsertRange(i - 2, deriver.EmitDerivation(methodDef, (Local)instruction2.Operand, (Local)instruction3.Operand));
				}
			}
		}
		methodDef.Body.Instructions.Clear();
		foreach (Instruction item in list)
		{
			methodDef.Body.Instructions.Add(item);
		}
		XMutationHelper xMutationHelper = new XMutationHelper("XMutationClass");
		xMutationHelper.InjectKeys(methodDef, new int[5] { 1, 2, 3, 4, 5 }, new int[5]
		{
			(int)(name1 * name2),
			(int)z[0],
			(int)x[0],
			(int)c[0],
			(int)v[0]
		});
		MethodDef methodDef2 = ctx.Module.GlobalType.FindOrCreateStaticConstructor();
		methodDef2.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(methodDef));
		foreach (IDnlibDef item2 in enumerable)
		{
			item2.Name = XCore.Utils.Utils.MethodsRenamig();
		}
	}

	public void CreateSections(ModuleWriterBase writer)
	{
		byte[] bytes = new byte[8]
		{
			(byte)name1,
			(byte)(name1 >> 8),
			(byte)(name1 >> 16),
			(byte)(name1 >> 24),
			(byte)name2,
			(byte)(name2 >> 8),
			(byte)(name2 >> 16),
			(byte)(name2 >> 24)
		};
		PESection pESection = new PESection(Encoding.ASCII.GetString(bytes), 3758096448u);
		writer.Sections.Insert(0, pESection);
		uint value = writer.TextSection.Remove(writer.Metadata).Value;
		writer.TextSection.Add(writer.Metadata, value);
		value = writer.TextSection.Remove(writer.NetResources).Value;
		writer.TextSection.Add(writer.NetResources, value);
		value = writer.TextSection.Remove(writer.Constants).Value;
		pESection.Add(writer.Constants, value);
		PESection pESection2 = new PESection(GGeneration.RandomString(5), 1610612768u);
		bool flag = false;
		if (writer.StrongNameSignature != null)
		{
			value = writer.TextSection.Remove(writer.StrongNameSignature).Value;
			pESection2.Add(writer.StrongNameSignature, value);
			flag = true;
		}
		if (writer is ModuleWriter moduleWriter)
		{
			if (moduleWriter.ImportAddressTable != null)
			{
				value = writer.TextSection.Remove(moduleWriter.ImportAddressTable).Value;
				pESection2.Add(moduleWriter.ImportAddressTable, value);
				flag = true;
			}
			if (moduleWriter.StartupStub != null)
			{
				value = writer.TextSection.Remove(moduleWriter.StartupStub).Value;
				pESection2.Add(moduleWriter.StartupStub, value);
				flag = true;
			}
		}
		if (flag)
		{
			writer.Sections.AddAfterText(pESection2);
		}
		else
		{
			writer.Sections.AddAfterText(pESection2);
		}
		MethodBodyChunks methodBodyChunks = new MethodBodyChunks(writer.TheOptions.ShareMethodBodies);
		pESection.Add(methodBodyChunks, 4u);
		foreach (MethodDef method in methods)
		{
			if (method.HasBody)
			{
				dnlib.DotNet.Writer.MethodBody methodBody = writer.Metadata.GetMethodBody(method);
				writer.MethodBodies.Remove(methodBody);
				methodBodyChunks.Add(methodBody);
			}
		}
		pESection.Add(new ByteArrayChunk(new byte[4]), 4u);
	}

	public void EncryptSection(ModuleWriterBase writer)
	{
		Stream destinationStream = writer.DestinationStream;
		BinaryReader binaryReader = new BinaryReader(writer.DestinationStream);
		destinationStream.Position = 60L;
		destinationStream.Position = binaryReader.ReadUInt32();
		destinationStream.Position += 6L;
		ushort num = binaryReader.ReadUInt16();
		destinationStream.Position += 12L;
		ushort num2 = binaryReader.ReadUInt16();
		destinationStream.Position += 2 + num2;
		uint num3 = 0u;
		uint num4 = 0u;
		int num5 = -1;
		if (writer is NativeModuleWriter && writer.Module is ModuleDefMD moduleDefMD)
		{
			num5 = moduleDefMD.Metadata.PEImage.ImageSectionHeaders.Count;
		}
		for (int i = 0; i < num; i++)
		{
			uint num6;
			if (num5 > 0)
			{
				num5--;
				destinationStream.Write(new byte[8], 0, 8);
				num6 = 0u;
			}
			else
			{
				num6 = binaryReader.ReadUInt32() * binaryReader.ReadUInt32();
			}
			destinationStream.Position += 8L;
			if (num6 == name1 * name2)
			{
				num4 = binaryReader.ReadUInt32();
				num3 = binaryReader.ReadUInt32();
			}
			else if (num6 != 0)
			{
				uint size = binaryReader.ReadUInt32();
				uint offset = binaryReader.ReadUInt32();
				Hash(destinationStream, binaryReader, offset, size);
			}
			else
			{
				destinationStream.Position += 8L;
			}
			destinationStream.Position += 16L;
		}
		uint[] array = DeriveKey();
		num4 >>= 2;
		destinationStream.Position = num3;
		uint[] array2 = new uint[num4];
		for (uint num7 = 0u; num7 < num4; num7++)
		{
			uint num8 = binaryReader.ReadUInt32();
			array2[num7] = num8 ^ array[num7 & 0xF];
			array[num7 & 0xF] = (array[num7 & 0xF] ^ num8) + 1035675673;
		}
		byte[] array3 = new byte[num4 << 2];
		Buffer.BlockCopy(array2, 0, array3, 0, array3.Length);
		destinationStream.Position = num3;
		destinationStream.Write(array3, 0, array3.Length);
	}

	private static void Hash(Stream stream, BinaryReader reader, uint offset, uint size)
	{
		long position = stream.Position;
		stream.Position = offset;
		size >>= 2;
		for (uint num = 0u; num < size; num++)
		{
			uint num2 = reader.ReadUInt32();
			uint num3 = (z[0] ^ num2) + x[0] + c[0] * v[0];
			z[0] = x[0];
			x[0] = c[0];
			x[0] = v[0];
			v[0] = num3;
		}
		stream.Position = position;
	}

	private static uint[] DeriveKey()
	{
		uint[] array = new uint[16];
		uint[] array2 = new uint[16];
		for (int i = 0; i < 16; i++)
		{
			array[i] = v[0];
			array2[i] = x[0];
			z[0] = (x[0] >> 5) | (x[0] << 27);
			x[0] = (c[0] >> 3) | (c[0] << 29);
			c[0] = (v[0] >> 7) | (v[0] << 25);
			v[0] = (z[0] >> 11) | (z[0] << 21);
		}
		return deriver.DeriveKey(array, array2);
	}
}
