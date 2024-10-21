using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Injection;
using XCore.Utils;
using XRuntime;

namespace XProtections;

public class PreStrings
{
	private static newInjector inj;

	private static MethodDef preMeth;

	private static void Inject(ModuleDefMD Module)
	{
		inj = new newInjector(Module, typeof(preString));
		preMeth = inj.FindMember("subIt") as MethodDef;
		inj.injectMethod("", Utils.MethodsRenamig(), Module, preMeth);
	}

	public static void Execute(XContext context)
	{
		Inject(context.Module);
		foreach (TypeDef item in from x in context.Module.GetTypes()
			where x.HasMethods && !x.IsGlobalModuleType && x.Namespace != "Costura"
			select x)
		{
			foreach (MethodDef item2 in item.Methods.Where((MethodDef x) => x.HasBody && x.Body.HasInstructions))
			{
				item2.Body.SimplifyMacros(item2.Parameters);
				item2.Body.SimplifyBranches();
				IList<Instruction> instructions = item2.Body.Instructions;
				for (int i = 0; i < instructions.Count; i++)
				{
					if (instructions[i].OpCode == OpCodes.Ldstr)
					{
						item2.Body.Instructions[i].Operand = item2.Body.Instructions[i].Operand.ToString() + "ظ";
						item2.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Call, preMeth));
					}
				}
				item2.Body.OptimizeMacros();
			}
		}
		inj.Rename();
	}
}
