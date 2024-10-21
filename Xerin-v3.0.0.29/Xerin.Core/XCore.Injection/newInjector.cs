using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using XCore.Utils;

namespace XCore.Injection;

public class newInjector
{
	private List<IDnlibDef> Members { get; set; }

	private Type RuntimeType { get; set; }

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

	public newInjector(ModuleDefMD module, Type type, bool injectType = true)
	{
		RuntimeType = type;
		Members = new List<IDnlibDef>();
		if (injectType)
		{
			InjectType(module);
		}
	}

	public void InjectType(ModuleDefMD module)
	{
		ModuleDefMD moduleDefMD = ModuleDefMD.Load(RuntimeType.Module);
		TypeDef typeDef = moduleDefMD.ResolveTypeDef(MDToken.ToRID(RuntimeType.MetadataToken));
		Members.AddRange(InjectHelper.Inject(typeDef, module.GlobalType, module).ToList());
	}

	public void injectMethod(string Namespace, string Name, ModuleDefMD module, MethodDef method)
	{
		TypeDef typeDef = new TypeDefUser(Namespace, Name, module.CorLibTypes.Object.TypeDefOrRef)
		{
			Attributes = TypeAttributes.Public
		};
		module.Types.Add(typeDef);
		method.DeclaringType = null;
		typeDef.Methods.Add(method);
	}

	public void injectMethods(string Namespace, string Name, ModuleDefMD module, MethodDef[] methods)
	{
		TypeDef typeDef = new TypeDefUser(Namespace, Name, module.CorLibTypes.Object.TypeDefOrRef)
		{
			Attributes = TypeAttributes.Public
		};
		module.Types.Add(typeDef);
		foreach (MethodDef methodDef in methods)
		{
			methodDef.DeclaringType = null;
			typeDef.Methods.Add(methodDef);
		}
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
