using System.IO;
using System.Threading.Tasks;
using dnlib.DotNet;
using KoiVM.Internal;

namespace XerinVM.Core;

public class Usage
{
	public static string normal = "";

	public static string vmOutput = "";

	public async Task ExceuteKoi(string input, string outPath, string snPath, string snPass)
	{
		using FileStream fs = new FileStream(input, FileMode.Open, FileAccess.Read);
		using MemoryStream ms = new MemoryStream();
		fs.CopyTo(ms);
		ms.Position = 0L;
		ModuleDefMD module = ModuleDefMD.Load(ms, new ModuleContext());
		try
		{
			await Task.Run(delegate
			{
				new KVMTask().Exceute(module, outPath, snPath, snPass);
			});
		}
		finally
		{
			fs.Dispose();
			ms.Dispose();
			module.Dispose();
		}
	}
}
