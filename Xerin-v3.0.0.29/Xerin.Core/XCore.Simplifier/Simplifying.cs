using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace XCore.Simplifier;

public static class Simplifying
{
	private static T ReadList<T>(IList<T> list, int index)
	{
		if (list == null)
		{
			return default(T);
		}
		if ((uint)index < (uint)list.Count)
		{
			return list[index];
		}
		return default(T);
	}

	private static void SimplifyMacros(this IList<Instruction> instructions, IList<Local> locals, IList<Parameter> parameters)
	{
		int count = instructions.Count;
		for (int i = 0; i < count; i++)
		{
			Instruction instruction = instructions[i];
			switch (instruction.OpCode.Code)
			{
			case Code.Leave_S:
				instruction.OpCode = OpCodes.Leave;
				break;
			case Code.Ldarg_0:
				instruction.OpCode = OpCodes.Ldarg;
				instruction.Operand = ReadList(parameters, 0);
				break;
			case Code.Ldarg_1:
				instruction.OpCode = OpCodes.Ldarg;
				instruction.Operand = ReadList(parameters, 1);
				break;
			case Code.Ldarg_2:
				instruction.OpCode = OpCodes.Ldarg;
				instruction.Operand = ReadList(parameters, 2);
				break;
			case Code.Ldarg_3:
				instruction.OpCode = OpCodes.Ldarg;
				instruction.Operand = ReadList(parameters, 3);
				break;
			case Code.Ldloc_0:
				instruction.OpCode = OpCodes.Ldloc;
				instruction.Operand = ReadList(locals, 0);
				break;
			case Code.Ldloc_1:
				instruction.OpCode = OpCodes.Ldloc;
				instruction.Operand = ReadList(locals, 1);
				break;
			case Code.Ldloc_2:
				instruction.OpCode = OpCodes.Ldloc;
				instruction.Operand = ReadList(locals, 2);
				break;
			case Code.Ldloc_3:
				instruction.OpCode = OpCodes.Ldloc;
				instruction.Operand = ReadList(locals, 3);
				break;
			case Code.Stloc_0:
				instruction.OpCode = OpCodes.Stloc;
				instruction.Operand = ReadList(locals, 0);
				break;
			case Code.Stloc_1:
				instruction.OpCode = OpCodes.Stloc;
				instruction.Operand = ReadList(locals, 1);
				break;
			case Code.Stloc_2:
				instruction.OpCode = OpCodes.Stloc;
				instruction.Operand = ReadList(locals, 2);
				break;
			case Code.Stloc_3:
				instruction.OpCode = OpCodes.Stloc;
				instruction.Operand = ReadList(locals, 3);
				break;
			case Code.Ldarg_S:
				instruction.OpCode = OpCodes.Ldarg;
				break;
			case Code.Ldarga_S:
				instruction.OpCode = OpCodes.Ldarga;
				break;
			case Code.Starg_S:
				instruction.OpCode = OpCodes.Starg;
				break;
			case Code.Ldloc_S:
				instruction.OpCode = OpCodes.Ldloc;
				break;
			case Code.Ldloca_S:
				instruction.OpCode = OpCodes.Ldloca;
				break;
			case Code.Stloc_S:
				instruction.OpCode = OpCodes.Stloc;
				break;
			case Code.Ldc_I4_M1:
				instruction.OpCode = OpCodes.Ldc_I4;
				instruction.Operand = -1;
				break;
			case Code.Ldc_I4_0:
				instruction.OpCode = OpCodes.Ldc_I4;
				instruction.Operand = 0;
				break;
			case Code.Ldc_I4_1:
				instruction.OpCode = OpCodes.Ldc_I4;
				instruction.Operand = 1;
				break;
			case Code.Ldc_I4_2:
				instruction.OpCode = OpCodes.Ldc_I4;
				instruction.Operand = 2;
				break;
			case Code.Ldc_I4_3:
				instruction.OpCode = OpCodes.Ldc_I4;
				instruction.Operand = 3;
				break;
			case Code.Ldc_I4_4:
				instruction.OpCode = OpCodes.Ldc_I4;
				instruction.Operand = 4;
				break;
			case Code.Ldc_I4_5:
				instruction.OpCode = OpCodes.Ldc_I4;
				instruction.Operand = 5;
				break;
			case Code.Ldc_I4_6:
				instruction.OpCode = OpCodes.Ldc_I4;
				instruction.Operand = 6;
				break;
			case Code.Ldc_I4_7:
				instruction.OpCode = OpCodes.Ldc_I4;
				instruction.Operand = 7;
				break;
			case Code.Ldc_I4_8:
				instruction.OpCode = OpCodes.Ldc_I4;
				instruction.Operand = 8;
				break;
			case Code.Ldc_I4_S:
				instruction.OpCode = OpCodes.Ldc_I4;
				instruction.Operand = (int)(sbyte)instruction.Operand;
				break;
			case Code.Br_S:
				instruction.OpCode = OpCodes.Br;
				break;
			case Code.Brfalse_S:
				instruction.OpCode = OpCodes.Brfalse;
				break;
			case Code.Brtrue_S:
				instruction.OpCode = OpCodes.Brtrue;
				break;
			case Code.Beq_S:
				instruction.OpCode = OpCodes.Beq;
				break;
			case Code.Bge_S:
				instruction.OpCode = OpCodes.Bge;
				break;
			case Code.Bgt_S:
				instruction.OpCode = OpCodes.Bgt;
				break;
			case Code.Ble_S:
				instruction.OpCode = OpCodes.Ble;
				break;
			case Code.Blt_S:
				instruction.OpCode = OpCodes.Blt;
				break;
			case Code.Bne_Un_S:
				instruction.OpCode = OpCodes.Bne_Un;
				break;
			case Code.Bge_Un_S:
				instruction.OpCode = OpCodes.Bge_Un;
				break;
			case Code.Bgt_Un_S:
				instruction.OpCode = OpCodes.Bgt_Un;
				break;
			case Code.Ble_Un_S:
				instruction.OpCode = OpCodes.Ble_Un;
				break;
			case Code.Blt_Un_S:
				instruction.OpCode = OpCodes.Blt_Un;
				break;
			}
		}
	}

	public static void Simplefy(ModuleDefMD Module)
	{
		uint num = 1758301857u;
		num = 377208505u;
		num = 902238194u;
		num = 4046008613u;
		num = 1758301857u;
		num = 2420016798u;
		num = 461628071u;
		IEnumerator<TypeDef> enumerator = Module.GetTypes().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				TypeDef current = enumerator.Current;
				num = 1259723677u;
				num = 3561648864u;
				num = 1621957013u;
				num = 3169621932u;
				num = 359659171u;
				num = 647071759u;
				num = 1712998104u;
				IEnumerator<MethodDef> enumerator2 = current.Methods.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						while (true)
						{
							IL_0182:
							MethodDef current2 = enumerator2.Current;
							while (true)
							{
								IL_016f:
								num = 1709781232u;
								num = 1548432940u;
								while (true)
								{
									CilBody body = current2.Body;
									if (body != null)
									{
										body.Instructions.SimplifyMacros(current2.Body.Variables, current2.Parameters);
										int num2 = (((int)(num + 1956669763 - 1956669763) + -1891630081) ^ -1601616572) + -1891630081 - -1891630081 >> 0 >> 0;
										num = 770395553u;
										num = 2403337215u;
										int num3 = num2 >> 0;
										while (true)
										{
											num = 1665746981u;
											num = 2261856985u;
											switch ((num = (uint)(num3 << 0)) % 10)
											{
											case 3u:
												break;
											case 4u:
											{
												int num4 = ((((int)(num + 1473386669 - 1473386669) + -434831334) ^ 0x4CCC1C8D) + -434831334 - -434831334) ^ 0 ^ 0;
												num = 497161444u;
												num = 3860135962u;
												num3 = num4 << 0;
												continue;
											}
											default:
												goto end_IL_008d;
											case 2u:
												goto IL_016f;
											case 0u:
											case 1u:
												goto IL_0182;
											case 5u:
												goto IL_018c;
											case 6u:
												goto IL_01a2;
											case 7u:
											case 8u:
												goto IL_01b0;
											case 9u:
												goto end_IL_008d;
											}
											break;
										}
										continue;
									}
									goto IL_018c;
									IL_018c:
									CilBody body2 = current2.Body;
									if (body2 == null)
									{
										goto IL_01b0;
									}
									body2.Instructions.SimplifyBranches();
									goto IL_01a2;
									IL_01a2:
									num = 1275685976u;
									num = 4112414650u;
									goto IL_01b0;
									continue;
									end_IL_008d:
									break;
								}
								break;
							}
							break;
						}
						break;
						IL_01b0:;
					}
				}
				finally
				{
					if (enumerator2 != null)
					{
						enumerator2.Dispose();
						num = 1918030397u;
						num = 1661375474u;
						num = 1603745310u;
						num = 1704553265u;
						num = 1918030397u;
						num = 4254873454u;
						num = 386283187u;
					}
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				enumerator.Dispose();
				num = 242805645u;
				num = 2538019396u;
				num = 2046497146u;
				num = 2797815664u;
				num = 242805645u;
				num = 583561026u;
				num = 1427638075u;
			}
		}
	}
}
