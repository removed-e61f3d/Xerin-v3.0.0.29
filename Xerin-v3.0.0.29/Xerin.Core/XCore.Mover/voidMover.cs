using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Utils;

namespace XCore.Mover;

public static class voidMover
{
	public static void Execute(XContext context)
	{
		foreach (TypeDef type in context.Module.GetTypes())
		{
			MethodDef[] array = type.Methods.ToArray();
			foreach (MethodDef methodDef in array)
			{
				if (methodDef == context.Module.EntryPoint)
				{
					mover(context, methodDef);
				}
			}
		}
	}

	public static void mover(XContext ctx, MethodDef method)
	{
		ModuleDefMD module = ctx.Module;
		MethodDef entryPoint = module.EntryPoint;
		if (entryPoint == null)
		{
			return;
		}
		MethodDef methodDef = XCore.Utils.Utils.CreateMethod(module);
		methodDef.DeclaringType = null;
		int index = module.EntryPoint.DeclaringType.Methods.IndexOf(entryPoint);
		entryPoint.DeclaringType.Methods.Insert(index, methodDef);
		MethodDef methodDef2 = module.GlobalType.FindStaticConstructor();
		MethodDef methodDef3 = null;
		foreach (TypeDef type in module.GetTypes())
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
