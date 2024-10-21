using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Generator;
using XCore.Injection;
using XCore.Utils;

namespace XProtections;

public static class stillWorkingOn2
{
	private static newInjector inj;

	private static MethodDef Call;

	private static string xor(Tuple<string, int> values)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int item = values.Item2;
		string item2 = values.Item1;
		foreach (char c in item2)
		{
			stringBuilder.Append((char)(c ^ item));
		}
		return stringBuilder.ToString();
	}

	private static void Inject(ModuleDefMD module)
	{
		inj = new newInjector(module, typeof(sUtils));
		Call = inj.FindMember("XorCipher") as MethodDef;
		inj.injectMethod("", Utils.MethodsRenamig(), module, Call);
	}

	public static void EncodeFor(XContext context, MethodDef[] methods)
	{
		Inject(context.Module);
		RandomGenerator randomGenerator = new RandomGenerator(default(Guid).ToString(), default(Guid).ToString());
		foreach (TypeDef item in from x in context.Module.GetTypes()
			where x.HasMethods && x.Name != "Costura"
			select x)
		{
			foreach (MethodDef item2 in item.Methods.Where((MethodDef x) => x.HasBody))
			{
				foreach (MethodDef methodDef in methods)
				{
					if (item2 != methodDef)
					{
						continue;
					}
					item2.Body.SimplifyMacros(item2.Parameters);
					item2.Body.SimplifyBranches();
					IList<Instruction> instructions = item2.Body.Instructions;
					for (int j = 0; j < instructions.Count; j++)
					{
						if (item2.Body.Instructions[j].OpCode == OpCodes.Ldstr)
						{
							int num = item2.Name.Length + randomGenerator.NextInt32();
							string operand = xor(new Tuple<string, int>(instructions[j].Operand.ToString(), num));
							item2.Body.Instructions[j].OpCode = OpCodes.Ldstr;
							item2.Body.Instructions[j].Operand = operand;
							item2.Body.Instructions.Insert(j + 1, new Instruction(OpCodes.Ldc_I4, num));
							item2.Body.Instructions.Insert(j + 2, new Instruction(OpCodes.Ldc_I4, 1));
							item2.Body.Instructions.Insert(j + 3, new Instruction(OpCodes.Mul));
							item2.Body.Instructions.Insert(j + 4, new Instruction(OpCodes.Call, Call));
							j += 4;
						}
					}
					item2.Body.OptimizeMacros();
				}
			}
		}
		inj.Rename();
	}
}
