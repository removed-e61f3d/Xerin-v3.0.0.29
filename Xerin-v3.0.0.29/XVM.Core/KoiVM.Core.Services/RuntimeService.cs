using System.IO;
using System.Reflection;
using dnlib.DotNet;

namespace KoiVM.Core.Services;

internal class RuntimeService
{
	private static ModuleDef rtModule;

	public static TypeDef GetRuntimeType(string runtimeDllName, string fullName)
	{
		if (rtModule == null)
		{
			LoadConfuserRuntimeModule(runtimeDllName);
		}
		return rtModule.Find(fullName, isReflectionName: true);
	}

	public static TypeDef GetRuntimeType(Module runtimeDllModule, string fullName)
	{
		if (rtModule == null)
		{
			LoadConfuserRuntimeModule(runtimeDllModule);
		}
		return rtModule.Find(fullName, isReflectionName: true);
	}

	private static void LoadConfuserRuntimeModule(string runtimeDllName)
	{
		Module manifestModule = typeof(RuntimeService).Assembly.ManifestModule;
		string text = runtimeDllName;
		ModuleCreationOptions options = new ModuleCreationOptions
		{
			TryToLoadPdbFromDisk = true
		};
		if (manifestModule.FullyQualifiedName[0] != '<')
		{
			text = Path.Combine(Path.GetDirectoryName(manifestModule.FullyQualifiedName), text);
			if (File.Exists(text))
			{
				try
				{
					rtModule = ModuleDefMD.Load(text, options);
				}
				catch (IOException)
				{
				}
			}
			if (rtModule == null)
			{
				text = runtimeDllName;
			}
		}
		if (rtModule == null)
		{
			rtModule = ModuleDefMD.Load(text, options);
		}
		rtModule.EnableTypeDefFindCache = true;
	}

	private static void LoadConfuserRuntimeModule(Module runtimeDllModule)
	{
		ModuleCreationOptions options = new ModuleCreationOptions
		{
			TryToLoadPdbFromDisk = true
		};
		if (rtModule == null)
		{
			try
			{
				rtModule = ModuleDefMD.Load(runtimeDllModule, options);
			}
			catch (IOException)
			{
			}
		}
		rtModule.EnableTypeDefFindCache = true;
	}
}
