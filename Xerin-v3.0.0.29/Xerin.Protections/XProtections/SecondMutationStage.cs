using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Protections;
using XCore.Shuffler;
using XCore.Utils;

namespace XProtections;

public class SecondMutationStage : Protection
{
	public override string name => "Performance mutation stage";

	public override int number => 2;

	public static void executeFor(MethodDef method)
	{
		if (method.HasBody && method.Body.HasInstructions)
		{
			DnlibUtils.Simplify(method);
			IntsToMath maths = new IntsToMath(method);
			IList<Instruction> instructions = method.Body.Instructions;
			ProcessInstructions(method, instructions, maths);
			DnlibUtils.Optimize(method);
		}
	}

	private static void ProcessInstructions(MethodDef method, IList<Instruction> instructions, IntsToMath maths)
	{
		for (int i = 0; i < instructions.Count; i++)
		{
			if (instructions[i].IsLdcI4() && DnlibUtils.CanObfuscate(instructions, i) && instructions[i].GetLdcI4Value() < int.MaxValue)
			{
				maths.Execute(ref i);
			}
		}
	}

	public override void Execute(XContext context)
	{
		ModuleDefMD module = context.Module;
		string excludedNamespace = "Costura";
		foreach (TypeDef item in from t in module.GetTypes()
			where !t.IsGlobalModuleType && t.Namespace != excludedNamespace
			select t)
		{
			ProcessType(item);
		}
	}

	private void ProcessType(TypeDef type)
	{
		foreach (MethodDef item in type.Methods.Where((MethodDef m) => m.HasBody && m.Body.HasInstructions && !m.MethodHasL2FAttribute()))
		{
			ProcessMethod(item);
		}
	}

	private void ProcessMethod(MethodDef method)
	{
		DnlibUtils.Simplify(method);
		IntsToMath intsToMath = new IntsToMath(method);
		IList<Instruction> instructions = method.Body.Instructions;
		for (int i = 0; i < instructions.Count; i++)
		{
			if (instructions[i].IsLdcI4() && DnlibUtils.CanObfuscate(instructions, i) && instructions[i].GetLdcI4Value() < int.MaxValue)
			{
				intsToMath.Execute(ref i);
				Shuffler.confuse(method, ref i);
			}
		}
		DnlibUtils.Optimize(method);
	}
}
