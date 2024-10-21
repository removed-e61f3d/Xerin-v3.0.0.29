#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.Helpers.System;

namespace KoiVM.Core.Services;

internal class MethodTrace
{
	private static readonly Dictionary<MethodDef, MethodTrace> cache = new Dictionary<MethodDef, MethodTrace>();

	private readonly MethodDef method;

	private Dictionary<int, List<Instruction>> fromInstrs;

	private Dictionary<uint, int> offset2index;

	public MethodDef Method => method;

	public Instruction[] Instructions { get; private set; }

	public Func<uint, int> OffsetToIndexMap => (uint offset) => offset2index[offset];

	public int[] BeforeStackDepths { get; private set; }

	public int[] AfterStackDepths { get; private set; }

	internal MethodTrace(MethodDef method)
	{
		this.method = method;
	}

	public static MethodTrace Trace(MethodDef method)
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		return cache.GetValueOrDefaultLazy(method, (MethodDef m) => cache[m] = new MethodTrace(m)).Trace();
	}

	public bool IsBranchTarget(int instrIndex)
	{
		return fromInstrs.ContainsKey(instrIndex);
	}

	internal MethodTrace Trace()
	{
		CilBody body = method.Body;
		method.Body.UpdateInstructionOffsets();
		Instructions = method.Body.Instructions.ToArray();
		offset2index = new Dictionary<uint, int>();
		int[] array = new int[body.Instructions.Count];
		int[] array2 = new int[body.Instructions.Count];
		fromInstrs = new Dictionary<int, List<Instruction>>();
		IList<Instruction> instructions = body.Instructions;
		for (int i = 0; i < instructions.Count; i++)
		{
			offset2index.Add(instructions[i].Offset, i);
			array[i] = int.MinValue;
		}
		foreach (ExceptionHandler exceptionHandler in body.ExceptionHandlers)
		{
			array[offset2index[exceptionHandler.TryStart.Offset]] = 0;
			array[offset2index[exceptionHandler.HandlerStart.Offset]] = ((exceptionHandler.HandlerType != ExceptionHandlerType.Finally) ? 1 : 0);
			if (exceptionHandler.FilterStart != null)
			{
				array[offset2index[exceptionHandler.FilterStart.Offset]] = 1;
			}
		}
		int stack = 0;
		for (int j = 0; j < instructions.Count; j++)
		{
			Instruction instruction = instructions[j];
			if (array[j] != int.MinValue)
			{
				stack = array[j];
			}
			array[j] = stack;
			instruction.UpdateStack(ref stack);
			array2[j] = stack;
			switch (instruction.OpCode.FlowControl)
			{
			case FlowControl.Branch:
			{
				int num3 = offset2index[((Instruction)instruction.Operand).Offset];
				if (array[num3] == int.MinValue)
				{
					array[num3] = stack;
				}
				fromInstrs.AddListEntry(offset2index[((Instruction)instruction.Operand).Offset], instruction);
				stack = 0;
				break;
			}
			case FlowControl.Call:
				if (instruction.OpCode.Code == Code.Jmp)
				{
					stack = 0;
				}
				break;
			case FlowControl.Cond_Branch:
				if (instruction.OpCode.Code == Code.Switch)
				{
					Instruction[] array3 = (Instruction[])instruction.Operand;
					foreach (Instruction instruction2 in array3)
					{
						int num = offset2index[instruction2.Offset];
						if (array[num] == int.MinValue)
						{
							array[num] = stack;
						}
						fromInstrs.AddListEntry(offset2index[instruction2.Offset], instruction);
					}
				}
				else
				{
					int num2 = offset2index[((Instruction)instruction.Operand).Offset];
					if (array[num2] == int.MinValue)
					{
						array[num2] = stack;
					}
					fromInstrs.AddListEntry(offset2index[((Instruction)instruction.Operand).Offset], instruction);
				}
				break;
			default:
				throw new UnreachableException();
			case FlowControl.Break:
			case FlowControl.Meta:
			case FlowControl.Next:
			case FlowControl.Return:
			case FlowControl.Throw:
				break;
			}
		}
		int[] array4 = array;
		foreach (int num4 in array4)
		{
			if (num4 == int.MinValue)
			{
				throw new InvalidMethodException("Bad method body.");
			}
		}
		int[] array5 = array2;
		foreach (int num5 in array5)
		{
			if (num5 == int.MinValue)
			{
				throw new InvalidMethodException("Bad method body.");
			}
		}
		BeforeStackDepths = array;
		AfterStackDepths = array2;
		return this;
	}

	public int[] TraceArguments(Instruction instr)
	{
		if (instr.OpCode.Code != Code.Call && instr.OpCode.Code != Code.Callvirt && instr.OpCode.Code != Code.Newobj)
		{
			throw new ArgumentException("Invalid call instruction.", "instr");
		}
		instr.CalculateStackUsage(out var pushes, out var pops);
		if (pops == 0)
		{
			return new int[0];
		}
		int num = offset2index[instr.Offset];
		int num2 = pops;
		int num3 = BeforeStackDepths[num] - num2;
		int num4 = -1;
		HashSet<uint> hashSet = new HashSet<uint>();
		Queue<int> queue = new Queue<int>();
		queue.Enqueue(offset2index[instr.Offset] - 1);
		while (queue.Count > 0)
		{
			int num5 = queue.Dequeue();
			while (num5 >= 0 && BeforeStackDepths[num5] != num3)
			{
				if (fromInstrs.ContainsKey(num5))
				{
					foreach (Instruction item2 in fromInstrs[num5])
					{
						if (!hashSet.Contains(item2.Offset))
						{
							hashSet.Add(item2.Offset);
							queue.Enqueue(offset2index[item2.Offset]);
						}
					}
				}
				num5--;
			}
			if (num5 < 0)
			{
				return null;
			}
			if (num4 == -1)
			{
				num4 = num5;
			}
			else if (num4 != num5)
			{
				return null;
			}
		}
		hashSet.Clear();
		Queue<KoiVM.Core.Helpers.System.Tuple<int, Stack<int>>> queue2 = new Queue<KoiVM.Core.Helpers.System.Tuple<int, Stack<int>>>();
		queue2.Clear();
		queue2.Enqueue(KoiVM.Core.Helpers.System.Tuple.Create(num4, new Stack<int>()));
		int[] array = null;
		while (queue2.Count > 0)
		{
			KoiVM.Core.Helpers.System.Tuple<int, Stack<int>> tuple = queue2.Dequeue();
			int num6 = tuple.Item1;
			Stack<int> item = tuple.Item2;
			while (num6 != num && num6 < method.Body.Instructions.Count)
			{
				Instruction instruction = Instructions[num6];
				instruction.CalculateStackUsage(out pushes, out pops);
				int num7 = pops - pushes;
				if (num7 < 0)
				{
					Debug.Assert(num7 == -1);
					item.Push(num6);
				}
				else
				{
					if (item.Count < num7)
					{
						return null;
					}
					for (int i = 0; i < num7; i++)
					{
						item.Pop();
					}
				}
				object operand = instruction.Operand;
				if (instruction.Operand is Instruction)
				{
					int num8 = offset2index[((Instruction)instruction.Operand).Offset];
					if (instruction.OpCode.FlowControl == FlowControl.Branch)
					{
						num6 = num8;
						continue;
					}
					queue2.Enqueue(KoiVM.Core.Helpers.System.Tuple.Create(num8, new Stack<int>(item)));
					num6++;
				}
				else if (instruction.Operand is Instruction[])
				{
					Instruction[] array2 = (Instruction[])instruction.Operand;
					foreach (Instruction instruction2 in array2)
					{
						queue2.Enqueue(KoiVM.Core.Helpers.System.Tuple.Create(offset2index[instruction2.Offset], new Stack<int>(item)));
					}
					num6++;
				}
				else
				{
					num6++;
				}
			}
			if (item.Count != num2)
			{
				return null;
			}
			if (array != null && !item.SequenceEqual(array))
			{
				return null;
			}
			array = item.ToArray();
		}
		if (array == null)
		{
			return array;
		}
		Array.Reverse(array);
		return array;
	}
}
