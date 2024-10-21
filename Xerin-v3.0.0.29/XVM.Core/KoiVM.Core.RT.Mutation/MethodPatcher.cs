using System;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace KoiVM.Core.RT.Mutation;

internal class MethodPatcher
{
	public static void Patch(RuntimeSearch rtscr, MethodDef method, uint id)
	{
		CilBody cilBody2 = (method.Body = new CilBody());
		cilBody2.SimplifyMacros(method.Parameters);
		cilBody2.SimplifyBranches();
		cilBody2.Instructions.Add(Instruction.Create(OpCodes.Newobj, method.Module.Import(rtscr.VMEntry_Ctor)));
		if (method.Parameters.Count == 0)
		{
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldnull));
		}
		else
		{
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, method.Parameters.Count));
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Newarr, method.Module.CorLibTypes.Object.ToTypeDefOrRef()));
			foreach (Parameter parameter in method.Parameters)
			{
				cilBody2.Instructions.Add(Instruction.Create(OpCodes.Dup));
				cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, parameter.Index));
				cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
				if (parameter.Type.IsByRef)
				{
					cilBody2.Instructions.Add(Instruction.Create(OpCodes.Mkrefany, parameter.Type.Next.ToTypeDefOrRef()));
					cilBody2.Instructions.Add(Instruction.Create(OpCodes.Newobj, method.Module.Import(rtscr.TypedRef_Ctor)));
				}
				else if (parameter.Type.IsPointer)
				{
					cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldtoken, parameter.Type.ToTypeDefOrRef()));
					cilBody2.Instructions.Add(Instruction.Create(OpCodes.Call, method.Module.Import(typeof(Type).GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Public))));
					cilBody2.Instructions.Add(Instruction.Create(OpCodes.Call, method.Module.Import(typeof(Pointer).GetMethod("Box", BindingFlags.Static | BindingFlags.Public))));
				}
				else if (parameter.Type.IsValueType)
				{
					cilBody2.Instructions.Add(Instruction.Create(OpCodes.Box, parameter.Type.ToTypeDefOrRef()));
				}
				cilBody2.Instructions.Add(Instruction.Create(OpCodes.Stelem_Ref));
			}
		}
		cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldtoken, method.DeclaringType));
		cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4_1));
		cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, (int)id));
		cilBody2.Instructions.Add(Instruction.Create(OpCodes.Call, method.Module.Import(rtscr.VMEntry_Run)));
		TypeSig typeSig = null;
		typeSig = ((method.ReturnType.Next != null) ? method.ReturnType.Next : method.ReturnType);
		if (method.ReturnType.IsPointer || (typeSig.IsPointer && method.ReturnType.IsByRef))
		{
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Call, method.Module.Import(typeof(Pointer).GetMethod("Unbox", BindingFlags.Static | BindingFlags.Public))));
			if (method.ReturnType.IsByRef)
			{
				if (method.ReturnType.Next.ToTypeDefOrRef().FullName == "System.Void*")
				{
					typeSig = null;
				}
			}
			else if (method.ReturnType.ToTypeDefOrRef().FullName == "System.Void*")
			{
				typeSig = null;
			}
		}
		if (method.ReturnType.ElementType == ElementType.Void)
		{
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Pop));
		}
		else if (method.ReturnType.IsValueType)
		{
			if (method.ReturnType.ToTypeDefOrRef().FullName != "System.Object&" && typeSig != null)
			{
				if (method.ReturnType.IsByRef)
				{
					cilBody2.Instructions.Add(Instruction.Create(OpCodes.Unbox_Any, method.ReturnType.Next.ToTypeDefOrRef()));
				}
				else
				{
					cilBody2.Instructions.Add(Instruction.Create(OpCodes.Unbox_Any, method.ReturnType.ToTypeDefOrRef()));
				}
			}
		}
		else if (method.ReturnType.ToTypeDefOrRef().FullName != "System.Object&" && typeSig != null)
		{
			if (method.ReturnType.IsByRef)
			{
				cilBody2.Instructions.Add(Instruction.Create(OpCodes.Castclass, method.ReturnType.Next.ToTypeDefOrRef()));
			}
			else
			{
				cilBody2.Instructions.Add(Instruction.Create(OpCodes.Castclass, method.ReturnType.ToTypeDefOrRef()));
			}
		}
		if (method.ReturnType.ElementType != ElementType.Void && method.ReturnType.IsByRef)
		{
			Local local = new Local(method.ReturnType.Next);
			cilBody2.Variables.Add(local);
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldloca, local));
		}
		cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ret));
		cilBody2.OptimizeMacros();
		cilBody2.OptimizeBranches();
	}
}
