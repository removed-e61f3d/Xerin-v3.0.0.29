using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace XRuntime;

public static class embedRuntime
{
	private static void AppStart()
	{
		AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblies;
	}

	private static Assembly ResolveAssemblies(object sender, ResolveEventArgs args)
	{
		try
		{
			string text = (args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", ""));
			if (text.EndsWith("_resources"))
			{
				return null;
			}
			using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(text);
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			return Assembly.Load(Decompress(array));
		}
		catch
		{
			return null;
		}
	}

	private static byte[] Decompress(byte[] data)
	{
		MemoryStream stream = new MemoryStream(data);
		MemoryStream memoryStream = new MemoryStream();
		using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress))
		{
			deflateStream.CopyTo(memoryStream);
		}
		return memoryStream.ToArray();
	}
}
