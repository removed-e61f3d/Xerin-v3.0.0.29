using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Protections;
using XCore.Utils;

namespace XProtections;

public class L2F : Protection
{
	public override string name => "Local to field";

	public override int number => 3;

	public override void Execute(XContext context)
	{
		ModuleDefMD module = context.Module;
		MethodDef methodDef = module.GlobalType.FindOrCreateStaticConstructor();
		IList<Instruction> instructions = methodDef.Body.Instructions;
		Dictionary<int, FieldDef> dictionary = new Dictionary<int, FieldDef>();
		foreach (TypeDef type in module.GetTypes())
		{
			if (type.IsGlobalModuleType)
			{
				continue;
			}
			foreach (MethodDef method in type.Methods)
			{
				if (!method.HasBody || !method.Body.HasInstructions || method.MethodHasL2FAttribute())
				{
					continue;
				}
				IList<Instruction> instructions2 = method.Body.Instructions;
				List<Instruction> list = instructions2.Where((Instruction x) => x.IsLdcI4()).ToList();
				foreach (Instruction item in list)
				{
					int ldcI4Value = item.GetLdcI4Value();
					if (!dictionary.TryGetValue(ldcI4Value, out var value))
					{
						value = Utils.CreateField(new FieldSig(module.CorLibTypes.Int32));
						module.GlobalType.Fields.Add(value);
						dictionary[ldcI4Value] = value;
						if (instructions.Count == 0)
						{
							instructions.Insert(0, OpCodes.Stsfld.ToInstruction(value));
							instructions.Insert(0, OpCodes.Ldc_I4.ToInstruction(ldcI4Value));
						}
						else
						{
							Instruction instruction = instructions.FirstOrDefault((Instruction i) => i.OpCode != OpCodes.Nop);
							if (instruction != null)
							{
								int index = instructions.IndexOf(instruction);
								instructions.Insert(index, OpCodes.Stsfld.ToInstruction(value));
								instructions.Insert(index, OpCodes.Ldc_I4.ToInstruction(ldcI4Value));
							}
							else
							{
								instructions.Insert(0, OpCodes.Stsfld.ToInstruction(value));
								instructions.Insert(0, OpCodes.Ldc_I4.ToInstruction(ldcI4Value));
							}
						}
					}
					item.OpCode = OpCodes.Ldsfld;
					item.Operand = value;
				}
			}
		}
	}
}
