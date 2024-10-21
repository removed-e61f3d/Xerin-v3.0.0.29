using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Utils;

namespace XCore.Mover;

public static class forDynamic
{
	public static void Execute(ModuleDefMD moduleDefMD, MethodDef method)
	{
		MethodDef entryPoint = moduleDefMD.EntryPoint;
		if (entryPoint == null)
		{
			return;
		}
		MethodDef methodDef = XCore.Utils.Utils.CreateMethod(moduleDefMD);
		methodDef.DeclaringType = null;
		int index = moduleDefMD.EntryPoint.DeclaringType.Methods.IndexOf(entryPoint);
		entryPoint.DeclaringType.Methods.Insert(index, methodDef);
		MethodDef methodDef2 = moduleDefMD.GlobalType.FindStaticConstructor();
		MethodDef methodDef3 = null;
		foreach (TypeDef type in moduleDefMD.GetTypes())
		{
			foreach (MethodDef method3 in type.Methods)
			{
				if (method3 == method)
				{
					methodDef3 = method3;
				}
			}
		}
		methodDef.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(methodDef3));
		Instruction instruction = null;
		for (int i = 0; i < methodDef2.Body.Instructions.Count; i++)
		{
			Instruction instruction2 = methodDef2.Body.Instructions[i];
			if (instruction2.OpCode == OpCodes.Call && instruction2.Operand is IMethod method2 && method2 == method)
			{
				instruction = instruction2;
				break;
			}
		}
		if (instruction != null)
		{
			methodDef2.Body.Instructions.Remove(instruction);
		}
		methodDef2.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(methodDef));
		methodDef3.Name = XCore.Utils.Utils.MethodsRenamig();
	}
}
