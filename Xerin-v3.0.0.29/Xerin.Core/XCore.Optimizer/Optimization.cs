using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace XCore.Optimizer;

public static class Optimization
{
	private static void FixJumps(this IEnumerable<Instruction> instructions, Dictionary<Instruction, Instruction> oldToNewTargets)
	{
		if (oldToNewTargets.Count == 0)
		{
			return;
		}
		foreach (Instruction instruction in instructions)
		{
			if (instruction.Operand is Instruction)
			{
				Instruction key = (Instruction)instruction.Operand;
				if (oldToNewTargets.TryGetValue(key, out var value))
				{
					instruction.Operand = value;
				}
			}
			else
			{
				if (!(instruction.Operand is Instruction[]))
				{
					continue;
				}
				Instruction[] array = (Instruction[])instruction.Operand;
				Instruction[] array2 = new Instruction[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					if (oldToNewTargets.TryGetValue(array[i], out var value2))
					{
						array2[i] = value2;
					}
					else
					{
						array2[i] = array[i];
					}
				}
				instruction.Operand = array2;
			}
		}
	}

	private static void SetNewInstructions(this CilBody methodBody, List<Instruction> newInstructions, Dictionary<Instruction, Instruction> oldToNewInstructions)
	{
		List<ExceptionHandler> list = new List<ExceptionHandler>();
		foreach (ExceptionHandler exceptionHandler2 in methodBody.ExceptionHandlers)
		{
			ExceptionHandler exceptionHandler = new ExceptionHandler(exceptionHandler2.HandlerType);
			exceptionHandler.CatchType = exceptionHandler2.CatchType;
			if (exceptionHandler2.FilterStart != null)
			{
				exceptionHandler.FilterStart = oldToNewInstructions[exceptionHandler2.FilterStart];
			}
			if (exceptionHandler2.HandlerEnd != null)
			{
				exceptionHandler.HandlerEnd = oldToNewInstructions[exceptionHandler2.HandlerEnd];
			}
			if (exceptionHandler2.HandlerStart != null)
			{
				exceptionHandler.HandlerStart = oldToNewInstructions[exceptionHandler2.HandlerStart];
			}
			if (exceptionHandler2.TryEnd != null)
			{
				exceptionHandler.TryEnd = oldToNewInstructions[exceptionHandler2.TryEnd];
			}
			if (exceptionHandler2.TryStart != null)
			{
				exceptionHandler.TryStart = oldToNewInstructions[exceptionHandler2.TryStart];
			}
			list.Add(exceptionHandler);
		}
		methodBody.ExceptionHandlers.Clear();
		foreach (ExceptionHandler item in list)
		{
			methodBody.ExceptionHandlers.Add(item);
		}
		newInstructions.FixJumps(oldToNewInstructions);
		methodBody.Instructions.Clear();
		foreach (Instruction newInstruction in newInstructions)
		{
			methodBody.Instructions.Add(newInstruction);
		}
	}

	private static Instruction CreateLoadInstructionInsteadOfLoadAddress(this Instruction instruction, Instruction _ilProcessor)
	{
		Instruction result = null;
		if (instruction.OpCode == OpCodes.Ldloca)
		{
			result = new Instruction(OpCodes.Ldloc, (Local)instruction.Operand);
		}
		else if (instruction.OpCode == OpCodes.Ldarga)
		{
			result = new Instruction(OpCodes.Ldloc, (Local)instruction.Operand);
		}
		return result;
	}

	private static void Remove_const_Value(ModuleDef module)
	{
		try
		{
			foreach (TypeDef type in module.Types)
			{
				for (int i = 0; i < type.Fields.Count; i++)
				{
					FieldDef fieldDef = type.Fields[i];
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.Object))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.Array))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.String))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.Boolean))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.Char))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.Ptr))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.SZArray))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.Class))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.I))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.I1))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.I2))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.I4))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.I8))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.R))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.R4))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.R8))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.U))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.U1))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.U4))
					{
						type.Fields.RemoveAt(i);
					}
					if (fieldDef.HasConstant && fieldDef.ElementType.Equals(ElementType.U8))
					{
						type.Fields.RemoveAt(i);
					}
				}
			}
		}
		catch
		{
		}
	}

	private static void ArmDot_Optimize(ModuleDef module)
	{
		foreach (TypeDef type in module.Types)
		{
			foreach (MethodDef method in type.Methods)
			{
				if (!method.HasBody)
				{
					continue;
				}
				using (new AutoSimplify(method, optimizeOnDispose: true))
				{
					method.Body.MaxStack = ushort.MaxValue;
					IMethod mrefB = method.Module.Import(typeof(IntPtr).GetConstructor(new Type[1] { typeof(int) }));
					IMethod mrefB2 = method.Module.Import(typeof(IntPtr).GetConstructor(new Type[1] { typeof(long) }));
					IMethod mrefB3 = method.Module.Import(typeof(IntPtr).GetMethod("ToInt32", Type.EmptyTypes));
					IMethod mrefB4 = method.Module.Import(typeof(IntPtr).GetMethod("ToInt64", Type.EmptyTypes));
					IMethod mrefB5 = method.Module.Import(typeof(UIntPtr).GetConstructor(new Type[1] { typeof(uint) }));
					IMethod mrefB6 = method.Module.Import(typeof(UIntPtr).GetConstructor(new Type[1] { typeof(ulong) }));
					IMethod mrefB7 = method.Module.Import(typeof(UIntPtr).GetMethod("ToUInt32", Type.EmptyTypes));
					IMethod mrefB8 = method.Module.Import(typeof(UIntPtr).GetMethod("ToUInt64", Type.EmptyTypes));
					List<Instruction> list = new List<Instruction>();
					Dictionary<Instruction, Instruction> dictionary = new Dictionary<Instruction, Instruction>();
					Instruction instruction = null;
					foreach (Instruction instruction4 in method.Body.Instructions)
					{
						Instruction instruction2 = null;
						Instruction instruction3 = null;
						if (instruction4.OpCode == OpCodes.Newobj)
						{
							IMethod mrefA = (IMethod)instruction4.Operand;
							if (EqualityComparer.Singleton.Equals(mrefA, mrefB) || EqualityComparer.Singleton.Equals(mrefA, mrefB2))
							{
								instruction2 = new Instruction(OpCodes.Conv_I);
							}
							else if (EqualityComparer.Singleton.Equals(mrefA, mrefB5) || EqualityComparer.Singleton.Equals(mrefA, mrefB6))
							{
								instruction2 = new Instruction(OpCodes.Conv_U);
							}
						}
						else if (instruction4.OpCode == OpCodes.Call)
						{
							IMethod mrefA2 = (IMethod)instruction4.Operand;
							if (EqualityComparer.Singleton.Equals(mrefA2, mrefB4))
							{
								instruction3 = instruction.CreateLoadInstructionInsteadOfLoadAddress(instruction4);
								if (instruction3 != null)
								{
									instruction2 = new Instruction(OpCodes.Conv_I8);
								}
							}
							else if (EqualityComparer.Singleton.Equals(mrefA2, mrefB3))
							{
								instruction3 = instruction.CreateLoadInstructionInsteadOfLoadAddress(instruction4);
								if (instruction3 != null)
								{
									instruction2 = new Instruction(OpCodes.Conv_I4);
								}
							}
							else if (EqualityComparer.Singleton.Equals(mrefA2, mrefB8))
							{
								instruction3 = instruction.CreateLoadInstructionInsteadOfLoadAddress(instruction4);
								if (instruction3 != null)
								{
									instruction2 = new Instruction(OpCodes.Conv_U8);
								}
							}
							else if (EqualityComparer.Singleton.Equals(mrefA2, mrefB7))
							{
								instruction3 = instruction.CreateLoadInstructionInsteadOfLoadAddress(instruction4);
								if (instruction3 != null)
								{
									instruction2 = new Instruction(OpCodes.Conv_U4);
								}
							}
						}
						if (instruction2 == null)
						{
							instruction2 = instruction4;
						}
						list.Add(instruction2);
						dictionary.Add(instruction4, instruction2);
						if (instruction3 != null)
						{
							list.Insert(list.IndexOf(instruction), instruction3);
							list.Remove(instruction);
							dictionary.Remove(instruction);
							dictionary.Add(instruction, instruction3);
						}
						instruction = instruction4;
					}
					method.Body.SetNewInstructions(list, dictionary);
				}
			}
		}
	}

	private static void OptimizeMacros(ModuleDef module)
	{
		uint num = 1758301857u;
		num = 377208505u;
		num = 902238195u;
		num = 4046008613u;
		num = 1758301857u;
		num = 2420016798u;
		num = 461628068u;
		IEnumerator<TypeDef> enumerator = module.GetTypes().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				TypeDef current = enumerator.Current;
				num = 1259723678u;
				num = 3561648864u;
				num = 1621957014u;
				num = 3169621932u;
				num = 359659171u;
				num = 647071759u;
				num = 1712998100u;
				IEnumerator<MethodDef> enumerator2 = current.Methods.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						while (true)
						{
							IL_0168:
							MethodDef current2 = enumerator2.Current;
							while (true)
							{
								IL_0155:
								num = 1709781227u;
								num = 1107201870u;
								while (true)
								{
									CilBody body = current2.Body;
									if (body == null)
									{
										break;
									}
									body.Instructions.OptimizeMacros();
									int num2 = ((((int)(num + 1457370637 - 1457370637) + -509051253) ^ 0x1231D8CF) + -509051253 - -509051253 >> 0) - 0;
									num = 770395556u;
									num = 3785916043u;
									int num3 = num2 << 0;
									while (true)
									{
										num = 1665746989u;
										num = 2855154040u;
										switch ((num = (uint)(num3 + 0)) % 8)
										{
										case 4u:
											break;
										case 0u:
										{
											int num4 = (((int)(num + 1455221297 - 1455221297) + -2071082168) ^ -313202618) + -2071082168 - -2071082168 + 0 + 0;
											num = 497161440u;
											num = 2223885128u;
											num3 = num4 - 0;
											continue;
										}
										default:
											goto end_IL_0172;
										case 3u:
											goto IL_0155;
										case 2u:
										case 5u:
											goto IL_0168;
										case 6u:
										case 7u:
											goto end_IL_008d;
										case 1u:
											goto end_IL_0172;
										}
										break;
									}
									continue;
									end_IL_008d:
									break;
								}
								break;
							}
							break;
						}
						continue;
						end_IL_0172:
						break;
					}
				}
				finally
				{
					if (enumerator2 != null)
					{
						enumerator2.Dispose();
						num = 1502418685u;
						num = 1559511532u;
						num = 1964931003u;
						num = 2533766834u;
						num = 1502418685u;
						num = 2731431507u;
						num = 452787184u;
					}
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				enumerator.Dispose();
				num = 757804181u;
				num = 3898060341u;
				num = 1452403643u;
				num = 3575121723u;
				num = 757804181u;
				num = 483666874u;
				num = 403315588u;
			}
		}
	}

	public static void OptimizeAssembly(ModuleDefMD Module)
	{
		Remove_const_Value(Module);
		ArmDot_Optimize(Module);
		OptimizeMacros(Module);
	}
}
