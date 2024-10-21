using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using KoiVM.Core.CFG;
using KoiVM.Core.Helpers;
using KoiVM.Core.RT;
using KoiVM.Core.RT.Mutation;
using KoiVM.Core.Services;
using KoiVM.Core.VMIL;
using XVM.Runtime;

namespace KoiVM.Core;

public class Virtualizer : IVMSettings
{
	private ModuleDef EXECModule;

	private MethodVirtualizer MDVirtualizer;

	private HashSet<MethodDef> methodList = new HashSet<MethodDef>();

	private HashSet<ModuleDef> processed = new HashSet<ModuleDef>();

	public VMRuntime Runtime { get; private set; }

	int IVMSettings.Seed => new RandomGenerator(32).NextInt32();

	public Virtualizer(ModuleDefMD module, string rtOUTLocation, string newRtName)
	{
		ModuleDefMD moduleDefMD = ModuleDefMD.Load(typeof(VMEntry).Module);
		moduleDefMD.Assembly.Name = newRtName;
		if (Path.GetExtension(newRtName) == ".dll")
		{
			moduleDefMD.Assembly.Name = Path.GetFileNameWithoutExtension(newRtName);
			moduleDefMD.Name = newRtName;
		}
		else
		{
			moduleDefMD.Name = newRtName + ".dll";
		}
		EXECModule = module;
		Runtime = new VMRuntime(this, moduleDefMD, new SaveRuntime(rtOUTLocation, newRtName));
		MDVirtualizer = new MethodVirtualizer(Runtime);
		AssemblyResolver assemblyResolver = new AssemblyResolver
		{
			EnableTypeDefCache = true
		};
		ModuleContext context = (assemblyResolver.DefaultModuleContext = new ModuleContext(assemblyResolver));
		EXECModule.Context = context;
		foreach (AssemblyRef assemblyRef in EXECModule.GetAssemblyRefs())
		{
			try
			{
				if (assemblyRef != null)
				{
					AssemblyDef assemblyDef = assemblyResolver.Resolve(assemblyRef.FullName, EXECModule);
				}
			}
			catch
			{
			}
		}
		MutationHelper.Field2IntIndex = MutationHelper.Original_Field2IntIndex;
		MutationHelper.Field2LongIndex = MutationHelper.Original_Field2LongIndex;
		MutationHelper.Field2LdstrIndex = MutationHelper.Original_Field2LdstrIndex;
		RTMap.Mutation = "Mutation";
		RTMap.Mutation_Placeholder = "Placeholder";
		RTMap.Mutation_Value_T = "Value";
		RTMap.Mutation_Value_T_Arg0 = "Value";
		RTMap.Mutation_Crypt = "Crypt";
		Runtime.RTSearch = new RuntimeSearch(Runtime.RTModule, Runtime).Search();
		Runtime.RTMutator.MutateRuntime();
	}

	public void AddMethod(MethodDef method)
	{
		if (method.HasBody && !method.HasGenericParameters)
		{
			methodList.Add(method);
		}
	}

	public IEnumerable<MethodDef> GetMethods()
	{
		return methodList;
	}

	public void ProcessMethods(Action<int, int> progress = null)
	{
		try
		{
			if (processed.Contains(EXECModule))
			{
				throw new InvalidOperationException("Module already processed.");
			}
			if (progress == null)
			{
				progress = delegate
				{
				};
			}
			List<MethodDef> list = methodList.Where((MethodDef method) => method.Module == EXECModule).ToList();
			for (int i = 0; i < list.Count; i++)
			{
				MethodDef method2 = list[i];
				ProcessMethod(method2);
				progress(i, list.Count);
			}
			progress(list.Count, list.Count);
			processed.Add(EXECModule);
		}
		catch
		{
		}
	}

	public void CommitModule(Metadata rtmtd)
	{
		try
		{
			MethodDef[] array = methodList.Where((MethodDef method) => method.Module == EXECModule).ToArray();
			foreach (MethodDef method2 in array)
			{
				PostProcessMethod(method2);
			}
			Runtime.RTMutator.CommitModule(EXECModule, rtmtd);
		}
		catch
		{
		}
	}

	private void ProcessMethod(MethodDef method)
	{
		MDVirtualizer.Run(method);
	}

	private void PostProcessMethod(MethodDef method)
	{
		ScopeBlock rootScope = Runtime.LookupMethod(method);
		ILPostTransformer iLPostTransformer = new ILPostTransformer(method, rootScope, Runtime);
		iLPostTransformer.Transform();
	}

	bool IVMSettings.IsVirtualized(MethodDef method)
	{
		return methodList.Contains(method);
	}
}
