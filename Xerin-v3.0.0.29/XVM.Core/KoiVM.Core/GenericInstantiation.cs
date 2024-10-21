using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace KoiVM.Core;

public class GenericInstantiation
{
	private readonly Dictionary<MethodSpec, MethodDef> instantiations = new Dictionary<MethodSpec, MethodDef>(MethodEqualityComparer.CompareDeclaringTypes);

	public event Func<MethodSpec, bool> ShouldInstantiate;

	public void EnsureInstantiation(MethodDef method, Action<MethodSpec, MethodDef> onInstantiated)
	{
		foreach (Instruction instruction in method.Body.Instructions)
		{
			if (!(instruction.Operand is MethodSpec))
			{
				continue;
			}
			MethodSpec methodSpec = (MethodSpec)instruction.Operand;
			if (this.ShouldInstantiate == null || this.ShouldInstantiate(methodSpec))
			{
				if (!Instantiate(methodSpec, out var def))
				{
					onInstantiated(methodSpec, def);
				}
				instruction.Operand = def;
			}
		}
	}

	public bool Instantiate(MethodSpec methodSpec, out MethodDef def)
	{
		if (instantiations.TryGetValue(methodSpec, out def))
		{
			return true;
		}
		GenericArguments genericArguments = new GenericArguments();
		genericArguments.PushMethodArgs(methodSpec.GenericInstMethodSig.GenericArguments);
		MethodDef methodDef = methodSpec.Method.ResolveMethodDefThrow();
		MethodSig methodSig = ResolveMethod(methodDef.MethodSig, genericArguments);
		methodSig.Generic = false;
		methodSig.GenParamCount = 0u;
		string text = methodDef.Name;
		foreach (TypeSig genericArgument in methodSpec.GenericInstMethodSig.GenericArguments)
		{
			text = text + ";" + genericArgument.TypeName;
		}
		def = new MethodDefUser(text, methodSig, methodDef.ImplAttributes, methodDef.Attributes);
		TypeSig typeSig = (methodDef.HasThis ? methodDef.Parameters[0].Type : null);
		def.DeclaringType2 = methodDef.DeclaringType2;
		if (typeSig != null)
		{
			def.Parameters[0].Type = typeSig;
		}
		foreach (DeclSecurity declSecurity in methodDef.DeclSecurities)
		{
			def.DeclSecurities.Add(declSecurity);
		}
		def.ImplMap = methodDef.ImplMap;
		foreach (MethodOverride @override in methodDef.Overrides)
		{
			def.Overrides.Add(@override);
		}
		def.Body = new CilBody();
		def.Body.InitLocals = methodDef.Body.InitLocals;
		def.Body.MaxStack = methodDef.Body.MaxStack;
		foreach (Local variable in methodDef.Body.Variables)
		{
			Local local = new Local(variable.Type);
			def.Body.Variables.Add(local);
		}
		Dictionary<Instruction, Instruction> dictionary = new Dictionary<Instruction, Instruction>();
		foreach (Instruction instruction2 in methodDef.Body.Instructions)
		{
			Instruction instruction = new Instruction(instruction2.OpCode, ResolveOperand(instruction2.Operand, genericArguments));
			def.Body.Instructions.Add(instruction);
			dictionary[instruction2] = instruction;
		}
		foreach (Instruction instruction3 in def.Body.Instructions)
		{
			if (instruction3.Operand is Instruction)
			{
				instruction3.Operand = dictionary[(Instruction)instruction3.Operand];
			}
			else if (instruction3.Operand is Instruction[])
			{
				Instruction[] array = (Instruction[])((Instruction[])instruction3.Operand).Clone();
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = dictionary[array[i]];
				}
				instruction3.Operand = array;
			}
		}
		def.Body.UpdateInstructionOffsets();
		foreach (ExceptionHandler exceptionHandler2 in methodDef.Body.ExceptionHandlers)
		{
			ExceptionHandler exceptionHandler = new ExceptionHandler(exceptionHandler2.HandlerType);
			exceptionHandler.TryStart = dictionary[exceptionHandler2.TryStart];
			exceptionHandler.HandlerStart = dictionary[exceptionHandler2.HandlerStart];
			if (exceptionHandler2.TryEnd != null)
			{
				exceptionHandler.TryEnd = dictionary[exceptionHandler2.TryEnd];
			}
			if (exceptionHandler2.HandlerEnd != null)
			{
				exceptionHandler.HandlerEnd = dictionary[exceptionHandler2.HandlerEnd];
			}
			if (exceptionHandler2.CatchType != null)
			{
				exceptionHandler.CatchType = genericArguments.Resolve(exceptionHandler.CatchType.ToTypeSig()).ToTypeDefOrRef();
			}
			else if (exceptionHandler2.FilterStart != null)
			{
				exceptionHandler.FilterStart = dictionary[exceptionHandler2.FilterStart];
			}
			def.Body.ExceptionHandlers.Add(exceptionHandler);
		}
		instantiations[methodSpec] = def;
		return false;
	}

	private FieldSig ResolveField(FieldSig sig, GenericArguments genericArgs)
	{
		FieldSig fieldSig = sig.Clone();
		fieldSig.Type = genericArgs.ResolveType(fieldSig.Type);
		return fieldSig;
	}

	private GenericInstMethodSig ResolveInst(GenericInstMethodSig sig, GenericArguments genericArgs)
	{
		GenericInstMethodSig genericInstMethodSig = sig.Clone();
		for (int i = 0; i < genericInstMethodSig.GenericArguments.Count; i++)
		{
			genericInstMethodSig.GenericArguments[i] = genericArgs.ResolveType(genericInstMethodSig.GenericArguments[i]);
		}
		return genericInstMethodSig;
	}

	private MethodSig ResolveMethod(MethodSig sig, GenericArguments genericArgs)
	{
		MethodSig methodSig = sig.Clone();
		for (int i = 0; i < methodSig.Params.Count; i++)
		{
			methodSig.Params[i] = genericArgs.ResolveType(methodSig.Params[i]);
		}
		if (methodSig.ParamsAfterSentinel != null)
		{
			for (int j = 0; j < methodSig.ParamsAfterSentinel.Count; j++)
			{
				methodSig.ParamsAfterSentinel[j] = genericArgs.ResolveType(methodSig.ParamsAfterSentinel[j]);
			}
		}
		methodSig.RetType = genericArgs.ResolveType(methodSig.RetType);
		return methodSig;
	}

	private object ResolveOperand(object operand, GenericArguments genericArgs)
	{
		if (operand is MemberRef)
		{
			MemberRef memberRef = (MemberRef)operand;
			if (memberRef.IsFieldRef)
			{
				FieldSig sig = ResolveField(memberRef.FieldSig, genericArgs);
				memberRef = new MemberRefUser(memberRef.Module, memberRef.Name, sig, memberRef.Class);
			}
			else
			{
				MethodSig sig2 = ResolveMethod(memberRef.MethodSig, genericArgs);
				memberRef = new MemberRefUser(memberRef.Module, memberRef.Name, sig2, memberRef.Class);
			}
			return memberRef;
		}
		if (operand is TypeSpec)
		{
			TypeSig typeSig = ((TypeSpec)operand).TypeSig;
			return genericArgs.ResolveType(typeSig).ToTypeDefOrRef();
		}
		if (operand is MethodSpec)
		{
			MethodSpec methodSpec = (MethodSpec)operand;
			return new MethodSpecUser(methodSpec.Method, ResolveInst(methodSpec.GenericInstMethodSig, genericArgs));
		}
		return operand;
	}
}
