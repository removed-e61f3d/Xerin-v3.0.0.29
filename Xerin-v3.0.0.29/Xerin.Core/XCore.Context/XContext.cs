using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confuser.Core;
using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.DotNet.Writer;
using XCore.Generator;
using XCore.Optimizer;
using XCore.Resolver;
using XCore.Simplifier;
using XCore.Utils;
using XProtections.CodeEncryption;

namespace XCore.Context;

public class XContext : IDisposable
{
	private readonly RandomGenerator randomGenerator;

	private readonly ServiceRegistry registry = new ServiceRegistry();

	private bool disposed = false;

	private static readonly char[] asciiCharset = (from ord in Enumerable.Range(32, 95)
		select (char)ord).Except(new char[1] { '.' }).ToArray();

	public string tmp { get; set; }

	public Annotations Annotations { get; } = new Annotations();


	public ServiceRegistry Registry => registry;

	public ModuleWriterOptions ModOpts { get; set; }

	public string Path { get; set; }

	public string OutPutPath { get; set; }

	public string DirPath { get; set; }

	public ModuleDefMD Module { get; set; }

	public static bool CE { get; set; }

	public static bool AD { get; set; }

	public static bool presreve { get; set; }

	public static bool Simpilify { get; set; }

	public static bool Optimize { get; set; }

	public XContext(string assemblyPath)
	{
		randomGenerator = new RandomGenerator(GGeneration.GenerateGuidStartingWithLetter(), GGeneration.GenerateGuidStartingWithLetter());
		Path = assemblyPath;
		ModuleContext context = ModuleDef.CreateModuleContext();
		Module = ModuleDefMD.Load(assemblyPath, context);
		AssemblyResolve.LoadAssemblies(Module);
		ModOpts = new ModuleWriterOptions(Module)
		{
			MetadataLogger = DummyLogger.NoThrowInstance,
			PdbOptions = PdbWriterOptions.None,
			WritePdb = false
		};
		if (presreve)
		{
			ModOpts.MetadataOptions.Flags = MetadataFlags.PreserveMethodRids;
		}
	}

	private void SetCodeEncryption(object sender, ModuleWriterEventArgs e)
	{
		switch (e.Event)
		{
		case ModuleWriterEvent.BeginStrongNameSign:
			new CodeEncryption().EncryptSection(e.Writer);
			break;
		case ModuleWriterEvent.MDEndCreateTables:
			new CodeEncryption().CreateSections(e.Writer);
			break;
		}
	}

	private void Randomize<T>(MDTable<T> table) where T : struct
	{
		randomGenerator.Shuffle(table);
	}

	private void SetInvalidMD(object sender, ModuleWriterEventArgs e)
	{
		ModuleWriterBase moduleWriterBase = (ModuleWriterBase)sender;
		switch (e.Event)
		{
		case ModuleWriterEvent.MDEndCreateTables:
			fake(e.Writer);
			CustomizeMetadata(e.Writer);
			break;
		case ModuleWriterEvent.MDOnAllTablesSorted:
			moduleWriterBase.Metadata.TablesHeap.DeclSecurityTable.Add(new RawDeclSecurityRow(short.MaxValue, 4294934527u, 4294934527u));
			moduleWriterBase.Metadata.TablesHeap.DeclSecurityTable.Add(new RawDeclSecurityRow(short.MaxValue, 4294934527u, 4294934527u));
			break;
		case ModuleWriterEvent.PESectionsCreated:
			CreateCustomSection(e.Writer);
			break;
		}
	}

	private void CreateCustomSection(ModuleWriterBase writer)
	{
		PESection pESection = new PESection(".Invalid", 3758096448u);
		writer.AddSection(pESection);
		pESection.Add(new ByteArrayChunk(new byte[10]), 4u);
	}

	private void CustomizeMetadata(ModuleWriterBase writer)
	{
		PESection pESection = new PESection(".Xerin", 1073741888u);
		writer.AddSection(pESection);
		pESection.Add(new ByteArrayChunk(new byte[123]), 4u);
		pESection.Add(new ByteArrayChunk(new byte[10]), 4u);
		writer.Metadata.TablesHeap.ModuleTable.Add(new RawModuleRow(0, 2147450879u, 0u, 0u, 0u));
		writer.Metadata.TablesHeap.AssemblyTable.Add(new RawAssemblyRow(0u, 0, 0, 0, 0, 0u, 0u, 2147450879u, 0u));
		writer.TheOptions.MetadataOptions.TablesHeapOptions.ExtraData = randomGenerator.NextUInt32();
		writer.TheOptions.MetadataOptions.TablesHeapOptions.UseENC = false;
		writer.TheOptions.MetadataOptions.MetadataHeaderOptions.VersionString += "\0\0\0\0";
		RandomizeTableEntries(writer);
		int num = randomGenerator.NextInt32(8, 16);
		for (int i = 0; i < num; i++)
		{
			writer.Metadata.TablesHeap.ENCLogTable.Add(new RawENCLogRow(randomGenerator.NextUInt32(), randomGenerator.NextUInt32()));
		}
		num = randomGenerator.NextInt32(8, 16);
		for (int j = 0; j < num; j++)
		{
			writer.Metadata.TablesHeap.ENCMapTable.Add(new RawENCMapRow(randomGenerator.NextUInt32()));
		}
		Randomize(writer.Metadata.TablesHeap.ManifestResourceTable);
		writer.TheOptions.MetadataOptions.TablesHeapOptions.ExtraData = randomGenerator.NextUInt32();
		writer.TheOptions.MetadataOptions.MetadataHeaderOptions.VersionString += "\0\0\0\0";
		byte[] bytes = Encoding.ASCII.GetBytes("Xerin");
		writer.TheOptions.MetadataOptions.CustomHeaps.Add(new RawHeap("#XerinFuscator", bytes));
		pESection.Add(new ByteArrayChunk(bytes), 4u);
		writer.Metadata.TablesHeap.TypeSpecTable.Add(new RawTypeSpecRow((uint)(writer.Metadata.TablesHeap.TypeSpecTable.Rows + 1)));
		writer.TheOptions.MetadataOptions.CustomHeaps.Add(new RawHeap("#GUID", Guid.NewGuid().ToByteArray()));
		writer.TheOptions.MetadataOptions.CustomHeaps.Add(new RawHeap("#Strings", new byte[1]));
		writer.TheOptions.MetadataOptions.CustomHeaps.Add(new RawHeap("#Blob", new byte[1]));
		writer.TheOptions.MetadataOptions.CustomHeaps.Add(new RawHeap("#Schema", new byte[1]));
		writer.TheOptions.MetadataOptions.CustomHeaps.Add(new RawHeap("#GUID", Guid.NewGuid().ToByteArray()));
		writer.TheOptions.MetadataOptions.CustomHeaps.Add(new RawHeap("#<Module>", new byte[1]));
	}

	public static string EncodeString(byte[] buff, char[] charset)
	{
		int num = buff[0];
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 1; i < buff.Length; i++)
		{
			for (num = (num << 8) + buff[i]; num >= charset.Length; num /= charset.Length)
			{
				stringBuilder.Append(charset[num % charset.Length]);
			}
		}
		if (num != 0)
		{
			stringBuilder.Append(charset[num % charset.Length]);
		}
		return stringBuilder.ToString();
	}

	private void RandomizeTableEntries(ModuleWriterBase writer)
	{
		int num = randomGenerator.NextInt32(8, 16);
		for (int i = 0; i < num; i++)
		{
			writer.Metadata.TablesHeap.ENCLogTable.Add(new RawENCLogRow(randomGenerator.NextUInt32(), randomGenerator.NextUInt32()));
		}
		num = randomGenerator.NextInt32(8, 16);
		for (int j = 0; j < num; j++)
		{
			writer.Metadata.TablesHeap.ENCMapTable.Add(new RawENCMapRow(randomGenerator.NextUInt32()));
		}
		Randomize(writer.Metadata.TablesHeap.ManifestResourceTable);
	}

	private void fake(ModuleWriterBase writer)
	{
		PESection pESection = new PESection("Wrong", 1073741888u);
		writer.AddSection(pESection);
		pESection.Add(new ByteArrayChunk(new byte[123]), 4u);
		pESection.Add(new ByteArrayChunk(new byte[10]), 4u);
		string text = ".Wrong";
		string s = null;
		for (int i = 0; i < 80; i++)
		{
			text += GGeneration.RandomString(25);
		}
		for (int j = 0; j < 80; j++)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(text);
			s = EncodeString(bytes, asciiCharset);
		}
		byte[] bytes2 = Encoding.ASCII.GetBytes(s);
		writer.TheOptions.MetadataOptions.CustomHeaps.Add(new RawHeap("#Invalid Data", bytes2));
		pESection.Add(new ByteArrayChunk(bytes2), 4u);
	}

	public async Task Save()
	{
		if (UnverifiableCodeAttributeAttr.attr)
		{
			UnverifiableCodeAttributeAttr.setAttr(Module);
		}
		if (Simpilify)
		{
			Simplifying.Simplefy(Module);
		}
		if (Optimize)
		{
			Optimization.OptimizeAssembly(Module);
		}
		if (CE)
		{
			ModOpts.WriterEvent += SetCodeEncryption;
		}
		if (AD)
		{
			ModOpts.WriterEvent += SetInvalidMD;
		}
		AttrMarker.setAttr(Module);
		if (!Directory.Exists(DirPath))
		{
			Directory.CreateDirectory(DirPath);
		}
		if (Directory.Exists(DirPath))
		{
			await Task.Run(delegate
			{
				Module.Write(OutPutPath, ModOpts);
			});
		}
		Dispose();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
		if (tmp != null)
		{
			try
			{
				Directory.Delete(tmp, recursive: true);
			}
			catch
			{
			}
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		while (true)
		{
			uint num = 1758301855u;
			num = 3499850521u;
			while (true)
			{
				bool flag = !disposed;
				num = 902238203u;
				num = 647071759u;
				while (true)
				{
					if (!flag)
					{
						return;
					}
					num = 461628063u;
					num = 3108670324u;
					while (true)
					{
						num = 272533153u;
						num = 3480015389u;
						while (true)
						{
							bool flag2 = disposing;
							num = 377208498u;
							num = 2644645089u;
							while (true)
							{
								IL_010b:
								if (flag2)
								{
									num = 414745695u;
									num = 1107201870u;
									while (true)
									{
										IL_00fa:
										num = 1422392294u;
										num = 3785916043u;
										while (true)
										{
											ModuleDefMD module = Module;
											if (module == null)
											{
												break;
											}
											module.Dispose();
											int num2 = ((((int)(num + 1455221297 - 1455221297) + -2071082168) ^ -2004194193) + -2071082168 - -2071082168 >> 0) - 0;
											num = 1239590187u;
											num = 2223885128u;
											int num3 = num2 << 0;
											while (true)
											{
												num = 1758301855u;
												num = 2164529479u;
												switch ((num = (uint)(num3 + 0)) % 14)
												{
												case 3u:
													break;
												case 10u:
												{
													int num4 = (((((int)num + -645064961 - -645064961 + 1376848916) ^ -400418536) + 1376848916 - 1376848916) ^ 0) << 0;
													num = 1190796316u;
													num = 1376848916u;
													num3 = num4 - 0;
													continue;
												}
												default:
													return;
												case 8u:
													goto IL_00fa;
												case 7u:
													goto IL_010b;
												case 2u:
													goto end_IL_010b;
												case 11u:
													goto end_IL_0113;
												case 1u:
													goto end_IL_0123;
												case 13u:
													goto end_IL_0131;
												case 0u:
												case 9u:
													goto end_IL_0139;
												case 4u:
												case 12u:
													goto end_IL_0027;
												case 5u:
													goto IL_0167;
												case 6u:
													return;
												}
												break;
											}
											continue;
											end_IL_0027:
											break;
										}
										break;
									}
								}
								disposed = true;
								goto IL_0167;
								IL_0167:
								num = 1802649735u;
								num = 1559511532u;
								return;
								continue;
								end_IL_010b:
								break;
							}
							continue;
							end_IL_0113:
							break;
						}
						continue;
						end_IL_0123:
						break;
					}
					continue;
					end_IL_0131:
					break;
				}
				continue;
				end_IL_0139:
				break;
			}
		}
	}

	~XContext()
	{
		Dispose(disposing: false);
	}
}
