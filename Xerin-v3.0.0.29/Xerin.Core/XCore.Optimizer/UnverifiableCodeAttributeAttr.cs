using dnlib.DotNet;

namespace XCore.Optimizer;

public class UnverifiableCodeAttributeAttr
{
	public static bool attr;

	public static void setAttr(ModuleDefMD module)
	{
		module.CustomAttributes.Add(new CustomAttribute(new MemberRefUser(module, ".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void), module.CorLibTypes.GetTypeRef("System.Security", "UnverifiableCodeAttribute"))));
	}
}
