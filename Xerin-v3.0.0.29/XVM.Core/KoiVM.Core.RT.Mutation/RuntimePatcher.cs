using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace KoiVM.Core.RT.Mutation;

internal static class RuntimePatcher
{
	public static void Patch(ModuleDef runtime)
	{
		PatchDispatcher(runtime);
	}

	private static void PatchDispatcher(ModuleDef runtime)
	{
		TypeDef typeDef = runtime.Find(RTMap.VMDispatcher, isReflectionName: true);
		MethodDef methodDef = typeDef.FindMethod(RTMap.VMEntry_Run);
		foreach (ExceptionHandler exceptionHandler in methodDef.Body.ExceptionHandlers)
		{
			if (exceptionHandler.HandlerType == ExceptionHandlerType.Catch)
			{
				exceptionHandler.CatchType = runtime.CorLibTypes.Object.ToTypeDefOrRef();
			}
		}
		PatchDoThrow(typeDef.FindMethod(RTMap.VMDispatcher_DoThrow).Body);
		typeDef.Methods.Remove(typeDef.FindMethod(RTMap.VMDispatcher_Throw));
	}

	private static void PatchDoThrow(CilBody body)
	{
		for (int i = 0; i < body.Instructions.Count; i++)
		{
			if (body.Instructions[i].Operand is IMethod method && method.Name == RTMap.VMDispatcher_Throw)
			{
				body.Instructions.RemoveAt(i);
			}
		}
	}
}
