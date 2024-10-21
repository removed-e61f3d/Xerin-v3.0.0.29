using System;
using System.Collections.Generic;
using System.Linq;
using Confuser.DynCipher;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Generator;
using XCore.Protections;
using XCore.Utils;

namespace Confuser.Protections.ReferenceProxy;

public class ReferenceProxyPhase : Protection
{
	private class RPStore
	{
		private class MethodSigComparer : IEqualityComparer<MethodSig>
		{
			public bool Equals(MethodSig x, MethodSig y)
			{
				return default(SigComparer).Equals(x, y);
			}

			public int GetHashCode(MethodSig obj)
			{
				return default(SigComparer).GetHashCode(obj);
			}
		}

		public readonly Dictionary<MethodSig, TypeDef> delegates = new Dictionary<MethodSig, TypeDef>(new MethodSigComparer());

		public ExpressionEncoding expression;

		public MildMode mild;

		public NormalEncoding normal;

		public RandomGenerator random;
	}

	public override string name => "Mild Reference Proxy";

	public override int number => 5;

	private static RPContext ParseParameters(MethodDef method, XContext context, RPStore store)
	{
		RPContext rPContext = new RPContext
		{
			Mode = (Mode)0,
			Encoding = (EncodingType)0,
			InternalAlso = false,
			TypeErasure = false,
			Depth = 1,
			Module = method.Module,
			Method = method,
			Body = method.Body,
			BranchTargets = new HashSet<Instruction>(from target in method.Body.Instructions.Select((Instruction instr) => instr.Operand as Instruction).Concat(method.Body.Instructions.Where((Instruction instr) => instr.Operand is Instruction[]).SelectMany((Instruction instr) => (Instruction[])instr.Operand))
				where target != null
				select target),
			Random = store.random,
			Context = context,
			DynCipher = context.Registry.GetService<IDynCipherService>(),
			Delegates = store.delegates
		};
		if (rPContext.Mode != 0)
		{
			throw new Exception();
		}
		rPContext.ModeHandler = store.mild ?? (store.mild = new MildMode());
		switch (rPContext.Encoding)
		{
		default:
			throw new Exception();
		case EncodingType.Expression:
			rPContext.EncodingHandler = store.expression ?? (store.expression = new ExpressionEncoding());
			break;
		case (EncodingType)0:
			rPContext.EncodingHandler = store.normal ?? (store.normal = new NormalEncoding());
			break;
		}
		return rPContext;
	}

	private static RPContext ParseParameters(ModuleDef module, XContext context, RPStore store)
	{
		return new RPContext
		{
			Depth = 1,
			InitCount = 16,
			Random = store.random,
			Module = module,
			Context = context,
			DynCipher = context.Registry.GetService<IDynCipherService>(),
			Delegates = store.delegates
		};
	}

	public override void Execute(XContext context)
	{
		RandomGenerator random = new RandomGenerator(GGeneration.GenerateGuidStartingWithLetter(), GGeneration.GenerateGuidStartingWithLetter());
		RPStore rPStore = new RPStore
		{
			random = random
		};
		TypeDef[] array = context.Module.Types.ToArray();
		foreach (TypeDef typeDef in array)
		{
			MethodDef[] array2 = typeDef.Methods.ToArray();
			foreach (MethodDef methodDef in array2)
			{
				if (methodDef.HasBody && methodDef.Body.Instructions.Count > 0)
				{
					ProcessMethod(ParseParameters(methodDef, context, rPStore));
				}
			}
		}
		RPContext ctx = ParseParameters(context.Module, context, rPStore);
		rPStore.mild?.Finalize(ctx);
	}

	private void ProcessMethod(RPContext ctx)
	{
		if (ctx.Method == null)
		{
			return;
		}
		for (int i = 0; i < ctx.Body.Instructions.Count; i++)
		{
			Instruction instruction = ctx.Body.Instructions[i];
			if (instruction.OpCode.Code != Code.Call && instruction.OpCode.Code != Code.Callvirt && instruction.OpCode.Code != Code.Newobj)
			{
				continue;
			}
			IMethod method = (IMethod)instruction.Operand;
			MethodDef methodDef = method.ResolveMethodDef();
			if (methodDef == null)
			{
				break;
			}
			if ((instruction.OpCode.Code == Code.Newobj || !(method.Name == ".ctor")) && (!(method is MethodDef) || ctx.InternalAlso) && !(method is MethodSpec) && !(method.DeclaringType is TypeSpec) && (method.MethodSig.ParamsAfterSentinel == null || method.MethodSig.ParamsAfterSentinel.Count <= 0))
			{
				TypeDef typeDef = method.DeclaringType.ResolveTypeDefThrow();
				if (!typeDef.IsDelegate() && (!typeDef.IsValueType || !method.MethodSig.HasThis) && (i - 1 < 0 || ctx.Body.Instructions[i - 1].OpCode.OpCodeType != OpCodeType.Prefix))
				{
					ctx.ModeHandler.ProcessCall(ctx, i);
				}
			}
		}
	}
}
