using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace XProtections.Mutation;

internal static class Helper
{
	public static void AddRange<T>(this IList<T> list, IList<T> values)
	{
		for (int i = 0; i < values.Count; i++)
		{
			list.Add(values[i]);
		}
	}

	public static void ReplaceWithInstructionList(this Instruction instruction, IList<Instruction> instructions, MethodDef method)
	{
		if (method.HasBody)
		{
			instruction.OpCode = instructions[0].OpCode;
			instruction.Operand = instructions[0].Operand;
			int num = method.Body.Instructions.IndexOf(instruction) + 1;
			for (int i = 1; i < instructions.Count; i++)
			{
				method.Body.Instructions.Insert(num++, instructions[i]);
			}
		}
	}
}
