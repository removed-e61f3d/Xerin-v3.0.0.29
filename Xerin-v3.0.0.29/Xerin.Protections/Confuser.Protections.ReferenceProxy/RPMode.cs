#define DEBUG
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using XCore.Generator;
using XCore.Utils;

namespace Confuser.Protections.ReferenceProxy;

internal abstract class RPMode
{
	public abstract void ProcessCall(RPContext ctx, int instrIndex);

	public abstract void Finalize(RPContext ctx);

	private static ITypeDefOrRef Import(RPContext ctx, TypeDef typeDef)
	{
		return new Importer(ctx.Module, ImporterOptions.TryToUseTypeDefs).Import(typeDef);
	}

	protected static MethodSig CreateProxySignature(RPContext ctx, IMethod method, bool newObj)
	{
		ModuleDef module = ctx.Module;
		if (newObj)
		{
			Debug.Assert(method.MethodSig.HasThis);
			Debug.Assert(method.Name == ".ctor");
			TypeSig[] argTypes = method.MethodSig.Params.Select((TypeSig type) => (ctx.TypeErasure && type.IsClassSig && method.MethodSig.HasThis) ? module.CorLibTypes.Object : type).ToArray();
			TypeSig retType;
			if (ctx.TypeErasure)
			{
				retType = module.CorLibTypes.Object;
			}
			else
			{
				TypeDef typeDef = method.DeclaringType.ResolveTypeDefThrow();
				retType = Import(ctx, typeDef).ToTypeSig();
			}
			return MethodSig.CreateStatic(retType, argTypes);
		}
		IEnumerable<TypeSig> enumerable = method.MethodSig.Params.Select((TypeSig type) => (ctx.TypeErasure && type.IsClassSig && method.MethodSig.HasThis) ? module.CorLibTypes.Object : type);
		if (method.MethodSig.HasThis && !method.MethodSig.ExplicitThis)
		{
			TypeDef typeDef2 = method.DeclaringType.ResolveTypeDefThrow();
			enumerable = ((!ctx.TypeErasure || typeDef2.IsValueType) ? new TypeSig[1] { Import(ctx, typeDef2).ToTypeSig() }.Concat(enumerable) : new CorLibTypeSig[1] { module.CorLibTypes.Object }.Concat(enumerable));
		}
		TypeSig typeSig = method.MethodSig.RetType;
		if (ctx.TypeErasure && typeSig.IsClassSig)
		{
			typeSig = module.CorLibTypes.Object;
		}
		return MethodSig.CreateStatic(typeSig, enumerable.ToArray());
	}

	protected static TypeDef GetDelegateType(RPContext ctx, MethodSig sig)
	{
		if (ctx.Delegates.TryGetValue(sig, out var value))
		{
			return value;
		}
		value = new TypeDefUser(GGeneration.GenerateGuidStartingWithLetter(), GGeneration.GenerateGuidStartingWithLetter(), ctx.Module.CorLibTypes.GetTypeRef("System", "MulticastDelegate"));
		value.Attributes = TypeAttributes.Sealed;
		MethodDefUser methodDefUser = new MethodDefUser(".ctor", MethodSig.CreateInstance(ctx.Module.CorLibTypes.Void, ctx.Module.CorLibTypes.Object, ctx.Module.CorLibTypes.IntPtr));
		methodDefUser.Attributes = MethodAttributes.Assembly | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
		methodDefUser.ImplAttributes = MethodImplAttributes.CodeTypeMask;
		value.Methods.Add(methodDefUser);
		MethodDefUser methodDefUser2 = new MethodDefUser("Invoke", sig.Clone());
		methodDefUser2.MethodSig.HasThis = true;
		methodDefUser2.Attributes = MethodAttributes.Assembly | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask;
		methodDefUser2.ImplAttributes = MethodImplAttributes.CodeTypeMask;
		value.Methods.Add(methodDefUser2);
		ctx.Module.Types.Add(value);
		foreach (IDnlibDef item in value.FindDefinitions())
		{
			_ = item;
		}
		ctx.Delegates[sig] = value;
		return value;
	}
}
