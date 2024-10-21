using System;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace KoiVM.Core.RT;

public class SaveRuntime
{
	internal VMRuntime Runtime { get; set; }

	public string RT_OUT_Directory { get; private set; }

	public string RTName { get; private set; }

	public string Location { get; set; }

	public SaveRuntime(string outdir, string newRTname)
	{
		try
		{
			RT_OUT_Directory = outdir;
			RTName = newRTname;
			if (Path.GetExtension(newRTname) == ".dll")
			{
				Location = Path.Combine(RT_OUT_Directory, RTName);
			}
			else
			{
				Location = Path.Combine(RT_OUT_Directory, RTName + ".dll");
			}
		}
		catch (Exception ex)
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			File.WriteAllText(folderPath + "\\error.txt", ex.ToString());
		}
	}

	public void Save()
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			ModuleWriterOptions options = new ModuleWriterOptions(Runtime.RTModule)
			{
				Logger = DummyLogger.NoThrowInstance,
				PdbOptions = PdbWriterOptions.None,
				WritePdb = false
			};
			Runtime.RTModule.Write(memoryStream, options);
			if (File.Exists(Location))
			{
				File.Delete(Location);
			}
			if (!Directory.Exists(RT_OUT_Directory))
			{
				Directory.CreateDirectory(RT_OUT_Directory);
			}
			File.WriteAllBytes(Location, memoryStream.ToArray());
		}
		catch (Exception ex)
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			File.WriteAllText(folderPath + "\\error.txt", ex.ToString());
		}
	}
}
