using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Generator;
using XCore.Protections;
using XCore.Utils;

namespace XProtections;

public class IntEncoding : Protection
{
	private Random random = new Random();

	private readonly List<int> _alreadyGeneratedInts = new List<int>();

	public override int number => 1;

	public override string name => "Ints encoding";

	private static MethodDef CreateDecryptMethod(ModuleDefMD module)
	{
		return new MethodDefUser(GGeneration.GenerateGuidStartingWithLetter(), MethodSig.CreateStatic(module.CorLibTypes.Int32, module.CorLibTypes.Int32), MethodImplAttributes.IL, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig)
		{
			Body = new CilBody()
		};
	}

	internal int GenerateInt()
	{
		int num;
		do
		{
			num = random.Next();
		}
		while (_alreadyGeneratedInts.Contains(num));
		_alreadyGeneratedInts.Add(num);
		return num;
	}

	private void CreateIntMethodBody(Dictionary<int, int> ints, MethodDef method)
	{
		CilBody cilBody2 = (method.Body = method.Body ?? new CilBody());
		cilBody2.OptimizeMacros();
		foreach (KeyValuePair<int, int> @int in ints)
		{
			int key = @int.Key;
			int value = @int.Value;
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, value));
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ceq));
			Instruction instruction = Instruction.Create(OpCodes.Nop);
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction));
			int num = GenerateInt();
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, num));
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, key ^ num));
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Xor));
			cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ret));
			cilBody2.Instructions.Add(instruction);
		}
		cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4_0));
		cilBody2.Instructions.Add(Instruction.Create(OpCodes.Ret));
	}

	public override void Execute(XContext context)
	{
		foreach (TypeDef item in context.Module.Types.Where((TypeDef t) => t.IsClass))
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			MethodDef methodDef = CreateDecryptMethod(context.Module);
			methodDef.excludeMethod(context.Module);
			foreach (MethodDef item2 in item.Methods.Where((MethodDef m) => m.HasBody))
			{
				DnlibUtils.Simplify(item2);
			}
			foreach (MethodDef item3 in item.Methods.Where((MethodDef m) => m.HasBody))
			{
				int num = item3.Body.Instructions.Count;
				for (int num2 = num - 1; num2 >= 0; num2--)
				{
					if (DnlibUtils.CanObfuscate(item3.Body.Instructions, num2) && item3.Body.Instructions[num2].OpCode == OpCodes.Ldc_I4)
					{
						int key = (int)item3.Body.Instructions[num2].Operand;
						if (!dictionary.TryGetValue(key, out var value))
						{
							value = GenerateInt();
							dictionary.Add(key, value);
						}
						item3.Body.Instructions[num2].Operand = value;
						item3.Body.Instructions.Insert(num2 + 1, Instruction.Create(OpCodes.Call, methodDef));
						num++;
					}
				}
				DnlibUtils.Optimize(item3);
			}
			if (dictionary.Count != 0)
			{
				CreateIntMethodBody(dictionary, methodDef);
				MethodDef methodDef2 = context.Module.GlobalType.FindOrCreateStaticConstructor();
				methodDef2.DeclaringType.Methods.Add(methodDef);
			}
		}
	}
}
