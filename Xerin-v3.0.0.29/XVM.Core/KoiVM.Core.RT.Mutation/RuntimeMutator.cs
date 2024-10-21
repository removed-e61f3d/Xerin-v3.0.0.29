using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using KoiVM.Core.VM;

namespace KoiVM.Core.RT.Mutation;

internal class RuntimeMutator
{
	private Metadata RTMetadata;

	private VMRuntime VMRT;

	internal RTConstants Constants;

	public RuntimeMutator(VMRuntime rt)
	{
		VMRT = rt;
		Constants = new RTConstants();
		Constants.ReadConstants(rt.Descriptor);
	}

	public void MutateRuntime()
	{
		RuntimePatcher.Patch(VMRT.RTModule);
		Constants.ReadConstants(VMRT.Descriptor);
		VMRT.RNMService.Process();
	}

	public void EndMutateRuntime()
	{
	}

	public void CommitModule(ModuleDef module, Metadata metadata)
	{
		RTMetadata = metadata;
		ImportReferences(module);
		MutateMetadata();
		EndMutateRuntime();
		VMRT.OnKoiRequested();
		VMRT.SaveRT.Save();
		VMRT.ResetData();
	}

	private void ImportReferences(ModuleDef module)
	{
		List<KeyValuePair<IMemberRef, uint>> list = VMRT.Descriptor.Data.refMap.ToList();
		VMRT.Descriptor.Data.refMap.Clear();
		foreach (KeyValuePair<IMemberRef, uint> item2 in list)
		{
			object obj = ((!(item2.Key is ITypeDefOrRef)) ? ((object)((!(item2.Key is MemberRef)) ? ((!(item2.Key is MethodDef)) ? ((!(item2.Key is MethodSpec)) ? ((!(item2.Key is FieldDef)) ? item2.Key : module.Import((FieldDef)item2.Key)) : module.Import((MethodSpec)item2.Key)) : module.Import((MethodDef)item2.Key)) : module.Import((MemberRef)item2.Key))) : ((object)module.Import((ITypeDefOrRef)item2.Key)));
			VMRT.Descriptor.Data.refMap.Add((IMemberRef)obj, item2.Value);
		}
		foreach (FuncSigDesc sig in VMRT.Descriptor.Data.sigs)
		{
			MethodSig signature = sig.Signature;
			FuncSig funcSig = sig.FuncSig;
			if (signature.HasThis)
			{
				funcSig.Flags |= VMRT.Descriptor.Runtime.RTFlags.INSTANCE;
			}
			List<ITypeDefOrRef> list2 = new List<ITypeDefOrRef>();
			if (signature.HasThis && !signature.ExplicitThis)
			{
				IType type = ((!sig.DeclaringType.IsValueType) ? module.Import(sig.DeclaringType) : module.Import(new ByRefSig(sig.DeclaringType.ToTypeSig()).ToTypeDefOrRef()));
				list2.Add((ITypeDefOrRef)type);
			}
			foreach (TypeSig param in signature.Params)
			{
				ITypeDefOrRef item = (ITypeDefOrRef)module.Import(param.ToTypeDefOrRef());
				list2.Add(item);
			}
			funcSig.ParamSigs = list2.ToArray();
			ITypeDefOrRef retType = (ITypeDefOrRef)module.Import(signature.RetType.ToTypeDefOrRef());
			funcSig.RetType = retType;
		}
	}

	private void MutateMetadata()
	{
		foreach (KeyValuePair<IMemberRef, uint> item in VMRT.Descriptor.Data.refMap)
		{
			item.Key.Rid = RTMetadata.GetToken(item.Key).Rid;
		}
		foreach (FuncSigDesc sig in VMRT.Descriptor.Data.sigs)
		{
			FuncSig funcSig = sig.FuncSig;
			ITypeDefOrRef[] paramSigs = funcSig.ParamSigs;
			foreach (ITypeDefOrRef typeDefOrRef in paramSigs)
			{
				typeDefOrRef.Rid = RTMetadata.GetToken(typeDefOrRef).Rid;
			}
			funcSig.RetType.Rid = RTMetadata.GetToken(funcSig.RetType).Rid;
		}
	}
}
