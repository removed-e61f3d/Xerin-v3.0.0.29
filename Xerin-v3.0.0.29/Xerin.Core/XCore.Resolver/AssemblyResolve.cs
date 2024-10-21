using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using dnlib.DotNet;

namespace XCore.Resolver;

public static class AssemblyResolve
{
	public static void LoadAssemblies(ModuleDefMD module)
	{
		AssemblyResolver assemblyResolver = new AssemblyResolver
		{
			EnableTypeDefCache = true
		};
		ModuleContext context = (assemblyResolver.DefaultModuleContext = new ModuleContext(assemblyResolver));
		if (Type.GetType("Mono.Runtime") != null)
		{
			try
			{
				string monoGacPath = GetMonoGacPath();
				if (!string.IsNullOrEmpty(monoGacPath))
				{
					assemblyResolver.PostSearchPaths.Add(Path.Combine(monoGacPath, "gac"));
					MessageBox.Show("Mono GAC path detected and added: " + Path.Combine(monoGacPath, "gac"));
				}
				else
				{
					assemblyResolver.PostSearchPaths.Add("/usr/lib/mono/gac");
					assemblyResolver.PostSearchPaths.Add("/usr/local/lib/mono/gac");
					MessageBox.Show("Fallback to common Mono GAC paths.");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error detecting Mono GAC paths: " + ex.Message);
			}
		}
		module.Context = context;
		List<AssemblyRef> list = module.GetAssemblyRefs().ToList();
		foreach (AssemblyRef item in list)
		{
			try
			{
				if (item != null)
				{
					AssemblyDef assemblyDef = assemblyResolver.Resolve(item.FullName, module);
					if (assemblyDef != null)
					{
						module.Context.AssemblyResolver.Resolve(assemblyDef, module);
					}
				}
			}
			catch (Exception arg)
			{
				throw new Exception($"Failed to resolve assembly {item.FullName}: {arg}");
			}
		}
	}

	private static string GetMonoGacPath()
	{
		try
		{
			Process process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "mono",
					Arguments = "--version",
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};
			process.Start();
			string text = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			if (text.Contains("Mono"))
			{
				string text2 = "/usr/lib/mono";
				if (Directory.Exists(text2))
				{
					return text2;
				}
				text2 = "/usr/local/lib/mono";
				if (Directory.Exists(text2))
				{
					return text2;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Error detecting Mono installation: " + ex.Message);
		}
		return null;
	}
}
