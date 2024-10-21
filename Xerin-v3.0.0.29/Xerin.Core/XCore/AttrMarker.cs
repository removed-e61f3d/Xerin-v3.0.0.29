using System;
using System.Linq;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace XCore;

public class AttrMarker
{
	public static void setAttr(ModuleDefMD module)
	{
		TypeRef typeRef = module.CorLibTypes.GetTypeRef("System", "Attribute");
		TypeDef typeDef = module.FindNormal("XerinAtrribute");
		if (typeDef == null)
		{
			typeDef = new TypeDefUser(string.Empty, "XerinAtrribute", typeRef);
			module.Types.Add(typeDef);
		}
		MethodDef methodDef = typeDef.FindInstanceConstructors().FirstOrDefault((MethodDef m) => m.Parameters.Count == 1 && m.Parameters[0].Type == module.CorLibTypes.String);
		if (methodDef == null)
		{
			methodDef = new MethodDefUser(".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.String), dnlib.DotNet.MethodImplAttributes.IL, dnlib.DotNet.MethodAttributes.Public | dnlib.DotNet.MethodAttributes.HideBySig | dnlib.DotNet.MethodAttributes.SpecialName | dnlib.DotNet.MethodAttributes.RTSpecialName)
			{
				Body = new CilBody
				{
					MaxStack = 1
				}
			};
			methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
			methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Call, new MemberRefUser(module, ".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void), typeRef)));
			methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
			typeDef.Methods.Add(methodDef);
		}
		CustomAttribute customAttribute = new CustomAttribute(methodDef);
		Assembly entryAssembly = Assembly.GetEntryAssembly();
		Version version = entryAssembly.GetName().Version;
		customAttribute.ConstructorArguments.Add(new CAArgument(module.CorLibTypes.String, "XerinFuscator v" + version));
		module.CustomAttributes.Add(customAttribute);
	}
}
