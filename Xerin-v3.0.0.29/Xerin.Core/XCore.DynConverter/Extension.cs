using System.IO;
using dnlib.DotNet;

namespace XCore.DynConverter;

public static class Extension
{
	public static void ConvertToBytes(this BinaryWriter writer, MethodDef method)
	{
		new XConverter(method, writer).ConvertToBytes();
	}
}
