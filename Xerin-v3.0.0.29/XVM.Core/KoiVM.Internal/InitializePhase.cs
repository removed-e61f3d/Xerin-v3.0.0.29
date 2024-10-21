using System;
using System.Collections.Generic;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using KoiVM.Core;
using XCore.Generator;

namespace KoiVM.Internal;

public class InitializePhase : IDisposable
{
	private bool disposed = false;

	private Dictionary<IMemberRef, IMemberRef> refRepl;

	public static HashSet<string> mnames = new HashSet<string>();

	public static HashSet<MethodDef> methods = new HashSet<MethodDef>();

	public ModuleDefMD DFModule { get; private set; }

	public Virtualizer VR { get; private set; }

	public string RT_OUT_Directory { get; set; }

	public string RTName { get; set; }

	public string SNK_File { get; set; }

	public string SNK_Password { get; set; }

	public InitializePhase(ModuleDefMD module)
	{
		DFModule = module;
		refRepl = new Dictionary<IMemberRef, IMemberRef>();
	}

	public void Initialize()
	{
		foreach (string mname in mnames)
		{
			foreach (TypeDef type in DFModule.Types)
			{
				foreach (MethodDef method in type.Methods)
				{
					if (mname.Contains(method.MDToken.ToString()) || mname.Contains(method.Name))
					{
						methods.Add(method);
					}
				}
			}
		}
		VR = new Virtualizer(DFModule, RT_OUT_Directory, RTName);
		TypeDef globalType = DFModule.GlobalType;
		TypeDefUser typeDefUser = new TypeDefUser(globalType.Name);
		globalType.Name = GGeneration.RandomString(3);
		globalType.BaseType = DFModule.CorLibTypes.GetTypeRef("System", "Object");
		DFModule.Types.Insert(0, typeDefUser);
		MethodDef methodDef = globalType.FindOrCreateStaticConstructor();
		MethodDef methodDef2 = typeDefUser.FindOrCreateStaticConstructor();
		methodDef.Name = GGeneration.RandomString(3);
		methodDef.IsRuntimeSpecialName = false;
		methodDef.IsSpecialName = false;
		methodDef.Access = MethodAttributes.PrivateScope;
		methodDef2.Body = new CilBody(initLocals: true, new List<Instruction>
		{
			Instruction.Create(OpCodes.Call, methodDef),
			Instruction.Create(OpCodes.Ret)
		}, new List<ExceptionHandler>(), new List<Local>());
		for (int i = 0; i < globalType.Methods.Count; i++)
		{
			MethodDef methodDef3 = globalType.Methods[i];
			if (methodDef3.IsNative)
			{
				MethodDefUser methodDefUser = new MethodDefUser(methodDef3.Name, methodDef3.MethodSig.Clone())
				{
					Attributes = (MethodAttributes.MemberAccessMask | MethodAttributes.Static),
					ImplAttributes = MethodImplAttributes.IL,
					Body = new CilBody()
				};
				methodDefUser.Body.Instructions.Add(new Instruction(OpCodes.Jmp, methodDef3));
				methodDefUser.Body.Instructions.Add(new Instruction(OpCodes.Ret));
				globalType.Methods[i] = methodDefUser;
				typeDefUser.Methods.Add(methodDef3);
				refRepl[methodDef3] = methodDefUser;
			}
		}
		methods.Remove(methodDef);
		Dictionary<ModuleDef, List<MethodDef>> toProcess = new Dictionary<ModuleDef, List<MethodDef>>();
		foreach (MethodDef method2 in methods)
		{
			VR.AddMethod(method2);
			toProcess.AddListEntry(DFModule, method2);
		}
		Utils.ExecuteModuleWriterOptions = new ModuleWriterOptions(DFModule)
		{
			Logger = DummyLogger.NoThrowInstance,
			PdbOptions = PdbWriterOptions.None,
			WritePdb = false
		};
		if (!string.IsNullOrEmpty(SNK_File) && File.Exists(SNK_File))
		{
			StrongNameKey signatureKey = Utils.LoadSNKey(SNK_File, SNK_Password);
			Utils.ExecuteModuleWriterOptions.InitializeStrongNameSigning(DFModule, signatureKey);
		}
		Utils.ExecuteModuleWriterOptions.WriterEvent += delegate(object sender, ModuleWriterEventArgs e)
		{
			ModuleWriterBase moduleWriterBase = (ModuleWriterBase)sender;
			if (e.Event == ModuleWriterEvent.MDBeginWriteMethodBodies && toProcess.ContainsKey(DFModule))
			{
				VR.ProcessMethods();
				foreach (KeyValuePair<IMemberRef, IMemberRef> item in refRepl)
				{
					VR.Runtime.Descriptor.Data.ReplaceReference(item.Key, item.Value);
				}
				VR.CommitModule(moduleWriterBase.Metadata);
			}
		};
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				DFModule?.Dispose();
			}
			disposed = true;
		}
	}

	~InitializePhase()
	{
		Dispose(disposing: false);
	}
}
