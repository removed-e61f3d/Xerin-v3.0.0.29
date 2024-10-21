using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Generator;
using XCore.Protections;

namespace XProtections;

public class AntiDecompiler : Protection
{
	public override string name => "Anti Decompiler";

	public override int number => 16;

	public override void Execute(XContext ctx)
	{
		ModuleDef manifestModule = ctx.Module.Assembly.ManifestModule;
		TypeRef typeRef = manifestModule.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "SuppressIldasmAttribute");
		MemberRefUser ctor = new MemberRefUser(manifestModule, ".ctor", MethodSig.CreateInstance(manifestModule.CorLibTypes.Void), typeRef);
		CustomAttribute item = new CustomAttribute(ctor);
		manifestModule.CustomAttributes.Add(item);
		ModuleDefMD module = ctx.Module;
		MethodDef methodDef = module.GlobalType.FindOrCreateStaticConstructor();
		for (int i = 0; i < 100000; i++)
		{
			methodDef.Body.Instructions.Insert(0, new Instruction(OpCodes.Nop));
		}
		methodDef.Body.Instructions.Add(new Instruction(OpCodes.Ret));
		Random random = new Random();
		InterfaceImpl item2 = new InterfaceImplUser(module.GlobalType);
		TypeDef typeDef = new TypeDefUser("", GGeneration.GenerateGuidStartingWithLetter(), module.CorLibTypes.GetTypeRef("System", "Attribute"));
		InterfaceImpl item3 = new InterfaceImplUser(typeDef);
		module.Types.Add(typeDef);
		typeDef.Interfaces.Add(item3);
		typeDef.Interfaces.Add(item2);
		for (int j = 0; j < random.Next(4, 15); j++)
		{
			TypeDef typeDef2 = new TypeDefUser("", GGeneration.GenerateGuidStartingWithLetter(), module.CorLibTypes.GetTypeRef("System", "Attribute"));
			InterfaceImpl item4 = new InterfaceImplUser(typeDef2);
			module.Types.Add(typeDef2);
			typeDef2.Interfaces.Add(item4);
			typeDef2.Interfaces.Add(item2);
			typeDef2.Interfaces.Add(item3);
		}
	}
}
