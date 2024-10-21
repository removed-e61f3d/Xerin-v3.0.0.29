using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using XCore.Utils;

namespace XCore.Injection;

public class Injector
{
	public ModuleDefMD TargetModule { get; }

	public Type RuntimeType { get; }

	public List<IDnlibDef> Members { get; }

	public Injector(ModuleDefMD targetModule, Type type, bool injectType = true)
	{
		TargetModule = targetModule;
		RuntimeType = type;
		Members = new List<IDnlibDef>();
		if (injectType)
		{
			InjectType();
		}
	}

	public void InjectType()
	{
		ModuleDefMD moduleDefMD = ModuleDefMD.Load(RuntimeType.Module);
		TypeDef typeDef = moduleDefMD.ResolveTypeDef(MDToken.ToRID(RuntimeType.MetadataToken));
		Members.AddRange(InjectHelper.Inject(typeDef, TargetModule.GlobalType, TargetModule).ToList());
	}

	public IDnlibDef FindMember(string name)
	{
		foreach (IDnlibDef member in Members)
		{
			if (member.Name == name)
			{
				return member;
			}
		}
		throw new Exception("Error to find member.");
	}

	public void Rename()
	{
		foreach (IDnlibDef member in Members)
		{
			if (!(member is MethodDef methodDef) || (!methodDef.HasImplMap && !methodDef.DeclaringType.IsDelegate))
			{
				XCore.Utils.Utils.MethodsRenamig(member);
			}
		}
	}
}
