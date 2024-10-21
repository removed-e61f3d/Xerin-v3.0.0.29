using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Generator;
using XCore.Utils;

namespace Confuser.Protections.ReferenceProxy;

internal class MildMode : RPMode
{
	private readonly Dictionary<Tuple<Code, TypeDef, IMethod>, MethodDef> proxies = new Dictionary<Tuple<Code, TypeDef, IMethod>, MethodDef>();

	public override void ProcessCall(RPContext ctx, int instrIndex)
	{
		Instruction instruction = ctx.Body.Instructions[instrIndex];
		IMethod method = (IMethod)instruction.Operand;
		if (method.DeclaringType.ResolveTypeDefThrow().IsValueType || (!method.ResolveThrow().IsPublic && !method.ResolveThrow().IsAssembly))
		{
			return;
		}
		Tuple<Code, TypeDef, IMethod> key = Tuple.Create(instruction.OpCode.Code, ctx.Method.DeclaringType, method);
		if (!proxies.TryGetValue(key, out var value))
		{
			MethodSig methodSig = RPMode.CreateProxySignature(ctx, method, instruction.OpCode.Code == Code.Newobj);
			value = new MethodDefUser(GGeneration.GenerateGuidStartingWithLetter(), methodSig);
			value.Attributes = MethodAttributes.Static;
			value.ImplAttributes = MethodImplAttributes.IL;
			ctx.Method.DeclaringType.Methods.Add(value);
			if (instruction.OpCode.Code == Code.Call && method.ResolveThrow().IsVirtual)
			{
				value.IsStatic = false;
				methodSig.HasThis = true;
				methodSig.Params.RemoveAt(0);
			}
			value.Body = new CilBody();
			for (int i = 0; i < value.Parameters.Count; i++)
			{
				value.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg, value.Parameters[i]));
			}
			value.Body.Instructions.Add(Instruction.Create(instruction.OpCode, method));
			value.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
			IMethod operand = method.Module.Import(typeof(Debug).GetMethod("Assert", new Type[1] { typeof(bool) }));
			value.Body.Instructions.Insert(0, new Instruction(OpCodes.Call, operand));
			value.Body.Instructions.Insert(0, new Instruction(OpCodes.Ldc_I4_1));
			if (value.ReturnType.ToString().ToLower().Contains("void"))
			{
				value.Body.Instructions.Insert(value.Body.Instructions.Count() - 2, new Instruction(OpCodes.Ldc_I4_1));
				value.Body.Instructions.Insert(value.Body.Instructions.Count() - 2, new Instruction(OpCodes.Call, operand));
			}
			proxies[key] = value;
		}
		instruction.OpCode = OpCodes.Call;
		if (ctx.Method.DeclaringType.HasGenericParameters)
		{
			GenericVar[] array = new GenericVar[ctx.Method.DeclaringType.GenericParameters.Count];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = new GenericVar(j);
			}
			ModuleDef module = ctx.Module;
			UTF8String name = value.Name;
			MethodSig methodSig2 = value.MethodSig;
			ClassOrValueTypeSig genericType = (ClassOrValueTypeSig)ctx.Method.DeclaringType.ToTypeSig();
			TypeSig[] genArgs = array;
			instruction.Operand = new MemberRefUser(module, name, methodSig2, new GenericInstSig(genericType, genArgs).ToTypeDefOrRef());
		}
		else
		{
			instruction.Operand = value;
		}
		MethodDef methodDef = method.ResolveMethodDef();
		if (methodDef != null)
		{
			ctx.Context.Annotations.Set(methodDef, methodDef, methodDef);
		}
	}

	public override void Finalize(RPContext ctx)
	{
	}
}
