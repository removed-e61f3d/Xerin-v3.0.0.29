using System.IO;
using dnlib.DotNet;
using KoiVM.Core;
using XCore.Generator;

namespace KoiVM.Internal;

public class KVMTask
{
	public void Exceute(ModuleDefMD module, string outPath, string snPath, string snPass)
	{
		MemoryStream memoryStream = new MemoryStream();
		InitializePhase initializePhase = null;
		try
		{
			initializePhase = new InitializePhase(module)
			{
				RT_OUT_Directory = Path.GetDirectoryName(outPath),
				RTName = GGeneration.RandomString(5),
				SNK_File = snPath,
				SNK_Password = snPass
			};
			initializePhase.Initialize();
			using (memoryStream)
			{
				module.Write(memoryStream, Utils.ExecuteModuleWriterOptions);
				File.WriteAllBytes(outPath, memoryStream.ToArray());
			}
			module.Dispose();
		}
		finally
		{
			initializePhase.Dispose();
			memoryStream.Dispose();
		}
	}
}
