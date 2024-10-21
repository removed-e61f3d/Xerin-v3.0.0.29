using System;
using System.IO;
using dnlib.DotNet.Pdb;
using KoiVM.Core.AST.IL;

namespace KoiVM.Core.RT;

internal class BasicBlockSerializer
{
	private VMRuntime rt;

	public BasicBlockSerializer(VMRuntime rt)
	{
		this.rt = rt;
	}

	public uint ComputeLength(ILBlock block)
	{
		uint num = 0u;
		foreach (ILInstruction item in block.Content)
		{
			num += ComputeLength(item);
		}
		return num;
	}

	public uint ComputeLength(ILInstruction instr)
	{
		uint num = 2u;
		if (instr.Operand != null)
		{
			if (instr.Operand is ILRegister)
			{
				num++;
			}
			else if (instr.Operand is ILImmediate)
			{
				object value = ((ILImmediate)instr.Operand).Value;
				if (value is uint || value is int || value is float)
				{
					num += 4;
				}
				else
				{
					if (!(value is ulong) && !(value is long) && !(value is double))
					{
						throw new NotSupportedException();
					}
					num += 8;
				}
			}
			else
			{
				if (!(instr.Operand is ILRelReference))
				{
					throw new NotSupportedException();
				}
				num += 4;
			}
		}
		return num;
	}

	public uint ComputeOffset(ILBlock block, uint offset)
	{
		foreach (ILInstruction item in block.Content)
		{
			item.Offset = offset;
			offset += 2;
			if (item.Operand == null)
			{
				continue;
			}
			if (item.Operand is ILRegister)
			{
				offset++;
			}
			else if (item.Operand is ILImmediate)
			{
				object value = ((ILImmediate)item.Operand).Value;
				if (value is uint || value is int || value is float)
				{
					offset += 4;
					continue;
				}
				if (!(value is ulong) && !(value is long) && !(value is double))
				{
					throw new NotSupportedException();
				}
				offset += 8;
			}
			else
			{
				if (!(item.Operand is ILRelReference))
				{
					throw new NotSupportedException();
				}
				offset += 4;
			}
		}
		return offset;
	}

	private static bool Equals(SequencePoint a, SequencePoint b)
	{
		return a.Document.Url == b.Document.Url && a.StartLine == b.StartLine;
	}

	public void WriteData(ILBlock block, BinaryWriter writer)
	{
		uint num = 0u;
		foreach (ILInstruction item in block.Content)
		{
			writer.Write(rt.Descriptor.Architecture.OpCodes[item.OpCode]);
			writer.Write(rt.Descriptor.RandomGenerator.NextByte());
			num += 2;
			if (item.Operand == null)
			{
				continue;
			}
			if (item.Operand is ILRegister)
			{
				writer.Write(rt.Descriptor.Architecture.Registers[((ILRegister)item.Operand).Register]);
				num++;
				continue;
			}
			if (item.Operand is ILImmediate)
			{
				object value = ((ILImmediate)item.Operand).Value;
				if (value is int)
				{
					writer.Write((int)value);
					num += 4;
				}
				else if (value is uint)
				{
					writer.Write((uint)value);
					num += 4;
				}
				else if (value is long)
				{
					writer.Write((long)value);
					num += 8;
				}
				else if (value is ulong)
				{
					writer.Write((ulong)value);
					num += 8;
				}
				else if (value is float)
				{
					writer.Write((float)value);
					num += 4;
				}
				else if (value is double)
				{
					writer.Write((double)value);
					num += 8;
				}
				continue;
			}
			throw new NotSupportedException();
		}
	}
}
