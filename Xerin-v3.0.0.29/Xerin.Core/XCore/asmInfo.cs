using System.Reflection;

namespace XCore;

public class asmInfo
{
	public static string asmName(string asm)
	{
		string text = "";
		try
		{
			Assembly assembly = Assembly.ReflectionOnlyLoadFrom(asm);
			return assembly.GetName().Name;
		}
		catch
		{
			return "Failed to read info";
		}
	}

	public static string asmArch(string asm)
	{
		string text = "";
		try
		{
			Assembly assembly = Assembly.ReflectionOnlyLoadFrom(asm);
			assembly.ManifestModule.GetPEKind(out var peKind, out var machine);
			if (peKind.HasFlag(PortableExecutableKinds.PE32Plus))
			{
				return "x64";
			}
			if (machine == ImageFileMachine.I386 && peKind.HasFlag(PortableExecutableKinds.Required32Bit))
			{
				return "x86";
			}
			if (machine == ImageFileMachine.I386 && !peKind.HasFlag(PortableExecutableKinds.Required32Bit))
			{
				return "Any CPU";
			}
			return "Unknown";
		}
		catch
		{
			return "Failed to read info";
		}
	}
}
