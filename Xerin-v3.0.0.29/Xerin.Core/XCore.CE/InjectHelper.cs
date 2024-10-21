using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace XCore.CE;

public static class InjectHelper
{
	private class InjectContext : ImportMapper
	{
		public readonly Dictionary<IMemberRef, IMemberRef> DefMap = new Dictionary<IMemberRef, IMemberRef>();

		public readonly ModuleDef OriginModule;

		public readonly ModuleDef TargetModule;

		public Importer Importer { get; }

		public InjectContext(ModuleDef module, ModuleDef target)
		{
			OriginModule = module;
			TargetModule = target;
			Importer = new Importer(target, ImporterOptions.TryToUseTypeDefs, default(GenericParamContext), this);
		}

		public override ITypeDefOrRef Map(ITypeDefOrRef source)
		{
			if (DefMap.TryGetValue(source, out var value))
			{
				return value as ITypeDefOrRef;
			}
			if (source is TypeRef typeRef)
			{
				AssemblyRef assemblyRef = TargetModule.GetAssemblyRef(typeRef.DefinitionAssembly.Name);
				if (assemblyRef != null && !string.Equals(assemblyRef.FullName, source.DefinitionAssembly.FullName, StringComparison.Ordinal))
				{
					TypeRefUser type = new TypeRefUser(typeRef.Module, typeRef.Namespace, typeRef.Name, assemblyRef);
					return Importer.Import(type);
				}
			}
			return null;
		}

		public override IMethod Map(MethodDef source)
		{
			if (DefMap.TryGetValue(source, out var value))
			{
				return value as IMethod;
			}
			return null;
		}

		public override IField Map(FieldDef source)
		{
			if (DefMap.TryGetValue(source, out var value))
			{
				return value as IField;
			}
			return null;
		}

		public override MemberRef Map(MemberRef source)
		{
			if (DefMap.TryGetValue(source, out var value))
			{
				return value as MemberRef;
			}
			return null;
		}
	}

	private static TypeDefUser Clone(TypeDef origin)
	{
		TypeDefUser typeDefUser = new TypeDefUser(origin.Namespace, origin.Name);
		typeDefUser.Attributes = origin.Attributes;
		if (origin.ClassLayout != null)
		{
			typeDefUser.ClassLayout = new ClassLayoutUser(origin.ClassLayout.PackingSize, origin.ClassSize);
		}
		foreach (GenericParam genericParameter in origin.GenericParameters)
		{
			typeDefUser.GenericParameters.Add(new GenericParamUser(genericParameter.Number, genericParameter.Flags, "-"));
		}
		return typeDefUser;
	}

	private static MethodDefUser Clone(MethodDef origin)
	{
		MethodDefUser methodDefUser = new MethodDefUser(origin.Name, null, origin.ImplAttributes, origin.Attributes);
		foreach (GenericParam genericParameter in origin.GenericParameters)
		{
			methodDefUser.GenericParameters.Add(new GenericParamUser(genericParameter.Number, genericParameter.Flags, "-"));
		}
		return methodDefUser;
	}

	private static FieldDefUser Clone(FieldDef origin)
	{
		return new FieldDefUser(origin.Name, null, origin.Attributes);
	}

	private static TypeDef PopulateContext(TypeDef typeDef, InjectContext ctx)
	{
		TypeDef typeDef2 = ctx.Map(typeDef)?.ResolveTypeDef();
		if (typeDef2 == null)
		{
			typeDef2 = Clone(typeDef);
			ctx.DefMap[typeDef] = typeDef2;
		}
		foreach (TypeDef nestedType in typeDef.NestedTypes)
		{
			typeDef2.NestedTypes.Add(PopulateContext(nestedType, ctx));
		}
		foreach (MethodDef method in typeDef.Methods)
		{
			IList<MethodDef> methods = typeDef2.Methods;
			IMemberRef memberRef2 = (ctx.DefMap[method] = Clone(method));
			methods.Add((MethodDef)memberRef2);
		}
		foreach (FieldDef field in typeDef.Fields)
		{
			IList<FieldDef> fields = typeDef2.Fields;
			IMemberRef memberRef2 = (ctx.DefMap[field] = Clone(field));
			fields.Add((FieldDef)memberRef2);
		}
		return typeDef2;
	}

	private static void CopyTypeDef(TypeDef typeDef, InjectContext ctx)
	{
		TypeDef typeDef2 = ctx.Map(typeDef)?.ResolveTypeDefThrow();
		typeDef2.BaseType = ctx.Importer.Import(typeDef.BaseType);
		foreach (InterfaceImpl @interface in typeDef.Interfaces)
		{
			typeDef2.Interfaces.Add(new InterfaceImplUser(ctx.Importer.Import(@interface.Interface)));
		}
	}

	private static void CopyMethodDef(MethodDef methodDef, InjectContext ctx)
	{
		MethodDef methodDef2 = ctx.Map(methodDef)?.ResolveMethodDefThrow();
		methodDef2.Signature = ctx.Importer.Import(methodDef.Signature);
		methodDef2.Parameters.UpdateParameterTypes();
		foreach (ParamDef paramDef in methodDef.ParamDefs)
		{
			methodDef2.ParamDefs.Add(new ParamDefUser(paramDef.Name, paramDef.Sequence, paramDef.Attributes));
		}
		if (methodDef.ImplMap != null)
		{
			methodDef2.ImplMap = new ImplMapUser(new ModuleRefUser(ctx.TargetModule, methodDef.ImplMap.Module.Name), methodDef.ImplMap.Name, methodDef.ImplMap.Attributes);
		}
		foreach (CustomAttribute customAttribute in methodDef.CustomAttributes)
		{
			methodDef2.CustomAttributes.Add(new CustomAttribute((ICustomAttributeType)ctx.Importer.Import(customAttribute.Constructor)));
		}
		if (methodDef.HasBody)
		{
			CopyMethodBody(methodDef, ctx, methodDef2);
		}
	}

	private static void CopyMethodBody(MethodDef methodDef, InjectContext ctx, MethodDef newMethodDef)
	{
		newMethodDef.Body = new CilBody(methodDef.Body.InitLocals, new List<Instruction>(), new List<ExceptionHandler>(), new List<Local>())
		{
			MaxStack = methodDef.Body.MaxStack
		};
		Dictionary<object, object> bodyMap = new Dictionary<object, object>();
		foreach (Local variable in methodDef.Body.Variables)
		{
			Local local = new Local(ctx.Importer.Import(variable.Type));
			newMethodDef.Body.Variables.Add(local);
			local.Name = variable.Name;
			bodyMap[variable] = local;
		}
		foreach (Instruction instruction2 in methodDef.Body.Instructions)
		{
			Instruction instruction = new Instruction(instruction2.OpCode, instruction2.Operand)
			{
				SequencePoint = instruction2.SequencePoint
			};
			object operand = instruction.Operand;
			object obj = operand;
			if (!(obj is IType type))
			{
				if (!(obj is IMethod method))
				{
					if (obj is IField field)
					{
						instruction.Operand = ctx.Importer.Import(field);
					}
				}
				else
				{
					instruction.Operand = ctx.Importer.Import(method);
				}
			}
			else
			{
				instruction.Operand = ctx.Importer.Import(type);
			}
			newMethodDef.Body.Instructions.Add(instruction);
			bodyMap[instruction2] = instruction;
		}
		foreach (Instruction instruction3 in newMethodDef.Body.Instructions)
		{
			if (instruction3.Operand != null && bodyMap.ContainsKey(instruction3.Operand))
			{
				instruction3.Operand = bodyMap[instruction3.Operand];
			}
			else if (instruction3.Operand is Instruction[] source)
			{
				instruction3.Operand = source.Select((Instruction target) => (Instruction)bodyMap[target]).ToArray();
			}
		}
		foreach (ExceptionHandler exceptionHandler in methodDef.Body.ExceptionHandlers)
		{
			newMethodDef.Body.ExceptionHandlers.Add(new ExceptionHandler(exceptionHandler.HandlerType)
			{
				CatchType = ((exceptionHandler.CatchType == null) ? null : ctx.Importer.Import(exceptionHandler.CatchType)),
				TryStart = (Instruction)bodyMap[exceptionHandler.TryStart],
				TryEnd = (Instruction)bodyMap[exceptionHandler.TryEnd],
				HandlerStart = (Instruction)bodyMap[exceptionHandler.HandlerStart],
				HandlerEnd = (Instruction)bodyMap[exceptionHandler.HandlerEnd],
				FilterStart = ((exceptionHandler.FilterStart == null) ? null : ((Instruction)bodyMap[exceptionHandler.FilterStart]))
			});
		}
		newMethodDef.Body.SimplifyMacros(newMethodDef.Parameters);
	}

	private static void CopyFieldDef(FieldDef fieldDef, InjectContext ctx)
	{
		FieldDef fieldDef2 = ctx.Map(fieldDef).ResolveFieldDefThrow();
		fieldDef2.Signature = ctx.Importer.Import(fieldDef.Signature);
	}

	private static void Copy(TypeDef typeDef, InjectContext ctx, bool copySelf)
	{
		if (copySelf)
		{
			CopyTypeDef(typeDef, ctx);
		}
		foreach (TypeDef nestedType in typeDef.NestedTypes)
		{
			Copy(nestedType, ctx, copySelf: true);
		}
		foreach (MethodDef method in typeDef.Methods)
		{
			CopyMethodDef(method, ctx);
		}
		foreach (FieldDef field in typeDef.Fields)
		{
			CopyFieldDef(field, ctx);
		}
	}

	public static TypeDef Inject(TypeDef typeDef, ModuleDef target)
	{
		InjectContext ctx = new InjectContext(typeDef.Module, target);
		TypeDef result = PopulateContext(typeDef, ctx);
		Copy(typeDef, ctx, copySelf: true);
		return result;
	}

	public static MethodDef Inject(MethodDef methodDef, ModuleDef target)
	{
		InjectContext injectContext = new InjectContext(methodDef.Module, target);
		MethodDef result = (MethodDef)(injectContext.DefMap[methodDef] = Clone(methodDef));
		CopyMethodDef(methodDef, injectContext);
		return result;
	}

	public static IEnumerable<IDnlibDef> Inject(TypeDef typeDef, TypeDef newType, ModuleDef target)
	{
		InjectContext injectContext = new InjectContext(typeDef.Module, target);
		injectContext.DefMap[typeDef] = newType;
		PopulateContext(typeDef, injectContext);
		Copy(typeDef, injectContext, copySelf: false);
		return injectContext.DefMap.Values.Except(new TypeDef[1] { newType }).OfType<IDnlibDef>();
	}
}
