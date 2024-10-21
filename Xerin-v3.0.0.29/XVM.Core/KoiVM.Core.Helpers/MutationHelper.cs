using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.RT;
using KoiVM.Core.Services;

namespace KoiVM.Core.Helpers;

internal static class MutationHelper
{
	internal static Dictionary<string, int> Field2IntIndex = new Dictionary<string, int>
	{
		{ "IntKey0", 0 },
		{ "IntKey1", 1 },
		{ "IntKey2", 2 },
		{ "IntKey3", 3 },
		{ "IntKey4", 4 },
		{ "IntKey5", 5 },
		{ "IntKey6", 6 },
		{ "IntKey7", 7 },
		{ "IntKey8", 8 },
		{ "IntKey9", 9 },
		{ "IntKey10", 10 },
		{ "IntKey11", 11 },
		{ "IntKey12", 12 },
		{ "IntKey13", 13 },
		{ "IntKey14", 14 },
		{ "IntKey15", 15 },
		{ "IntKey16", 16 },
		{ "IntKey17", 17 },
		{ "IntKey18", 18 },
		{ "IntKey19", 19 },
		{ "IntKey20", 20 }
	};

	internal static Dictionary<string, int> Field2LongIndex = new Dictionary<string, int>
	{
		{ "LongKey0", 0 },
		{ "LongKey1", 1 },
		{ "LongKey2", 2 },
		{ "LongKey3", 3 },
		{ "LongKey4", 4 },
		{ "LongKey5", 5 },
		{ "LongKey6", 6 },
		{ "LongKey7", 7 },
		{ "LongKey8", 8 },
		{ "LongKey9", 9 },
		{ "LongKey10", 10 },
		{ "LongKey11", 11 },
		{ "LongKey12", 12 },
		{ "LongKey13", 13 },
		{ "LongKey14", 14 },
		{ "LongKey15", 15 },
		{ "LongKey16", 16 },
		{ "LongKey17", 17 },
		{ "LongKey18", 18 },
		{ "LongKey19", 19 },
		{ "LongKey20", 20 }
	};

	internal static Dictionary<string, string> Field2LdstrIndex = new Dictionary<string, string>
	{
		{
			"LdstrKey0",
			Convert.ToString(0)
		},
		{
			"LdstrKey1",
			Convert.ToString(1)
		},
		{
			"LdstrKey2",
			Convert.ToString(2)
		},
		{
			"LdstrKey3",
			Convert.ToString(3)
		},
		{
			"LdstrKey4",
			Convert.ToString(4)
		},
		{
			"LdstrKey5",
			Convert.ToString(5)
		},
		{
			"LdstrKey6",
			Convert.ToString(6)
		},
		{
			"LdstrKey7",
			Convert.ToString(7)
		},
		{
			"LdstrKey8",
			Convert.ToString(8)
		},
		{
			"LdstrKey9",
			Convert.ToString(9)
		},
		{
			"LdstrKey10",
			Convert.ToString(10)
		},
		{
			"LdstrKey11",
			Convert.ToString(11)
		},
		{
			"LdstrKey12",
			Convert.ToString(12)
		},
		{
			"LdstrKey13",
			Convert.ToString(13)
		},
		{
			"LdstrKey14",
			Convert.ToString(14)
		},
		{
			"LdstrKey15",
			Convert.ToString(15)
		},
		{
			"LdstrKey16",
			Convert.ToString(16)
		},
		{
			"LdstrKey17",
			Convert.ToString(17)
		},
		{
			"LdstrKey18",
			Convert.ToString(18)
		},
		{
			"LdstrKey19",
			Convert.ToString(19)
		},
		{
			"LdstrKey20",
			Convert.ToString(20)
		}
	};

	internal static readonly Dictionary<string, int> Original_Field2IntIndex = new Dictionary<string, int>
	{
		{ "IntKey0", 0 },
		{ "IntKey1", 1 },
		{ "IntKey2", 2 },
		{ "IntKey3", 3 },
		{ "IntKey4", 4 },
		{ "IntKey5", 5 },
		{ "IntKey6", 6 },
		{ "IntKey7", 7 },
		{ "IntKey8", 8 },
		{ "IntKey9", 9 },
		{ "IntKey10", 10 },
		{ "IntKey11", 11 },
		{ "IntKey12", 12 },
		{ "IntKey13", 13 },
		{ "IntKey14", 14 },
		{ "IntKey15", 15 },
		{ "IntKey16", 16 },
		{ "IntKey17", 17 },
		{ "IntKey18", 18 },
		{ "IntKey19", 19 },
		{ "IntKey20", 20 }
	};

	internal static readonly Dictionary<string, int> Original_Field2LongIndex = new Dictionary<string, int>
	{
		{ "LongKey0", 0 },
		{ "LongKey1", 1 },
		{ "LongKey2", 2 },
		{ "LongKey3", 3 },
		{ "LongKey4", 4 },
		{ "LongKey5", 5 },
		{ "LongKey6", 6 },
		{ "LongKey7", 7 },
		{ "LongKey8", 8 },
		{ "LongKey9", 9 },
		{ "LongKey10", 10 },
		{ "LongKey11", 11 },
		{ "LongKey12", 12 },
		{ "LongKey13", 13 },
		{ "LongKey14", 14 },
		{ "LongKey15", 15 },
		{ "LongKey16", 16 },
		{ "LongKey17", 17 },
		{ "LongKey18", 18 },
		{ "LongKey19", 19 },
		{ "LongKey20", 20 }
	};

	internal static readonly Dictionary<string, string> Original_Field2LdstrIndex = new Dictionary<string, string>
	{
		{
			"LdstrKey0",
			Convert.ToString(0)
		},
		{
			"LdstrKey1",
			Convert.ToString(1)
		},
		{
			"LdstrKey2",
			Convert.ToString(2)
		},
		{
			"LdstrKey3",
			Convert.ToString(3)
		},
		{
			"LdstrKey4",
			Convert.ToString(4)
		},
		{
			"LdstrKey5",
			Convert.ToString(5)
		},
		{
			"LdstrKey6",
			Convert.ToString(6)
		},
		{
			"LdstrKey7",
			Convert.ToString(7)
		},
		{
			"LdstrKey8",
			Convert.ToString(8)
		},
		{
			"LdstrKey9",
			Convert.ToString(9)
		},
		{
			"LdstrKey10",
			Convert.ToString(10)
		},
		{
			"LdstrKey11",
			Convert.ToString(11)
		},
		{
			"LdstrKey12",
			Convert.ToString(12)
		},
		{
			"LdstrKey13",
			Convert.ToString(13)
		},
		{
			"LdstrKey14",
			Convert.ToString(14)
		},
		{
			"LdstrKey15",
			Convert.ToString(15)
		},
		{
			"LdstrKey16",
			Convert.ToString(16)
		},
		{
			"LdstrKey17",
			Convert.ToString(17)
		},
		{
			"LdstrKey18",
			Convert.ToString(18)
		},
		{
			"LdstrKey19",
			Convert.ToString(19)
		},
		{
			"LdstrKey20",
			Convert.ToString(20)
		}
	};

	internal static void InjectKey_Int(MethodDef method, int keyId, int key)
	{
		foreach (Instruction instruction in method.Body.Instructions)
		{
			if (instruction.OpCode == OpCodes.Ldsfld)
			{
				IField field = instruction.Operand as IField;
				if (field.DeclaringType.FullName == RTMap.Mutation && Field2IntIndex.TryGetValue(field.Name, out var value) && value == keyId)
				{
					instruction.OpCode = OpCodes.Ldc_I4;
					instruction.Operand = key;
				}
			}
		}
	}

	internal static void InjectKey_Long(MethodDef method, int keyId, long key)
	{
		foreach (Instruction instruction in method.Body.Instructions)
		{
			if (instruction.OpCode == OpCodes.Ldsfld)
			{
				IField field = instruction.Operand as IField;
				if (field.DeclaringType.FullName == RTMap.Mutation && Field2LongIndex.TryGetValue(field.Name, out var value) && value == keyId)
				{
					instruction.OpCode = OpCodes.Ldc_I8;
					instruction.Operand = key;
				}
			}
		}
	}

	internal static void InjectKey_String(MethodDef method, int keyId, string key)
	{
		foreach (Instruction instruction in method.Body.Instructions)
		{
			if (instruction.OpCode == OpCodes.Ldsfld)
			{
				IField field = instruction.Operand as IField;
				if (field.DeclaringType.FullName == RTMap.Mutation && Field2LdstrIndex.TryGetValue(field.Name, out var value) && value == keyId.ToString())
				{
					instruction.OpCode = OpCodes.Ldstr;
					instruction.Operand = key;
				}
			}
		}
	}

	internal static void InjectKeys_Int(MethodDef method, int[] keyIds, int[] keys)
	{
		foreach (Instruction instruction in method.Body.Instructions)
		{
			if (instruction.OpCode == OpCodes.Ldsfld)
			{
				IField field = instruction.Operand as IField;
				if (field.DeclaringType.FullName == RTMap.Mutation && Field2IntIndex.TryGetValue(field.Name, out var value) && (value = Array.IndexOf(keyIds, value)) != -1)
				{
					instruction.OpCode = OpCodes.Ldc_I4;
					instruction.Operand = keys[value];
				}
			}
		}
	}

	internal static void InjectKeys_Long(MethodDef method, int[] keyIds, long[] keys)
	{
		foreach (Instruction instruction in method.Body.Instructions)
		{
			if (instruction.OpCode == OpCodes.Ldsfld)
			{
				IField field = instruction.Operand as IField;
				if (field.DeclaringType.FullName == RTMap.Mutation && Field2LongIndex.TryGetValue(field.Name, out var value) && (value = Array.IndexOf(keyIds, value)) != -1)
				{
					instruction.OpCode = OpCodes.Ldc_I8;
					instruction.Operand = keys[value];
				}
			}
		}
	}

	internal static void InjectKeys_String(MethodDef method, int[] keyIds, string[] keys)
	{
		foreach (Instruction instruction in method.Body.Instructions)
		{
			if (instruction.OpCode == OpCodes.Ldsfld)
			{
				IField field = instruction.Operand as IField;
				if (field.DeclaringType.FullName == RTMap.Mutation && Field2LdstrIndex.TryGetValue(field.Name, out var value) && Convert.ToInt32(value = Array.IndexOf(keyIds, int.Parse(value)).ToString()) != -1)
				{
					instruction.OpCode = OpCodes.Ldstr;
					instruction.Operand = keys[int.Parse(value)];
				}
			}
		}
	}

	internal static void ReplaceValue_T(MethodDef method, Instruction ret_inst)
	{
		for (int i = 0; i < method.Body.Instructions.Count; i++)
		{
			Instruction instruction = method.Body.Instructions[i];
			IMethod method2 = instruction.Operand as IMethod;
			if (instruction.OpCode == OpCodes.Call && method2.DeclaringType.Name == RTMap.Mutation && method2.Name == RTMap.Mutation_Value_T)
			{
				method.Body.Instructions[i] = ret_inst;
			}
		}
	}

	internal static void ReplacePlaceholder_Inject_ByteArray(MethodDef method, byte[] data)
	{
		ReplacePlaceholder(method, delegate(Instruction[] arg)
		{
			List<Instruction> list = new List<Instruction>();
			list.AddRange(arg);
			for (int i = 0; i < data.Length; i++)
			{
				list.Add(Instruction.Create(OpCodes.Dup));
				list.Add(Instruction.Create(OpCodes.Ldc_I4, i));
				list.Add(Instruction.Create(OpCodes.Ldc_I4, (int)data[i]));
				list.Add(Instruction.Create(OpCodes.Stelem_Ref));
			}
			return list.ToArray();
		});
	}

	internal static void ReplacePlaceholder(MethodDef method, Func<Instruction[], Instruction[]> repl)
	{
		MethodTrace methodTrace = new MethodTrace(method).Trace();
		for (int i = 0; i < method.Body.Instructions.Count; i++)
		{
			Instruction instruction = method.Body.Instructions[i];
			if (instruction.OpCode != OpCodes.Call)
			{
				continue;
			}
			IMethod method2 = (IMethod)instruction.Operand;
			if (!(method2.DeclaringType.FullName == RTMap.Mutation) || !(method2.Name == RTMap.Mutation_Placeholder))
			{
				continue;
			}
			int[] array = methodTrace.TraceArguments(instruction);
			if (array == null)
			{
				throw new ArgumentException("Failed to trace placeholder argument.");
			}
			int num = array[0];
			Instruction[] array2 = method.Body.Instructions.Skip(num).Take(i - num).ToArray();
			for (int j = 0; j < array2.Length; j++)
			{
				method.Body.Instructions.RemoveAt(num);
			}
			method.Body.Instructions.RemoveAt(num);
			array2 = repl(array2);
			for (int num2 = array2.Length - 1; num2 >= 0; num2--)
			{
				method.Body.Instructions.Insert(num, array2[num2]);
			}
			break;
		}
	}
}
