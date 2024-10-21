using System;
using KoiVM.Core.AST.IL;

namespace KoiVM.Core.RT;

public class JumpTableChunk : IChunk
{
	internal VMRuntime runtime;

	public ILJumpTable Table { get; private set; }

	public uint Offset { get; private set; }

	uint IChunk.Length => (uint)(Table.Targets.Length * 4 + 2);

	public JumpTableChunk(ILJumpTable table)
	{
		Table = table;
		if (table.Targets.Length > 65535)
		{
			throw new NotSupportedException("Jump table too large.");
		}
	}

	void IChunk.OnOffsetComputed(uint offset)
	{
		Offset = offset + 2;
	}

	byte[] IChunk.GetData()
	{
		byte[] array = new byte[Table.Targets.Length * 4 + 2];
		ushort num = (ushort)Table.Targets.Length;
		int num2 = 0;
		array[num2++] = (byte)Table.Targets.Length;
		array[num2++] = (byte)(Table.Targets.Length >> 8);
		uint offset = Table.RelativeBase.Offset;
		offset += runtime.Serializer.ComputeLength(Table.RelativeBase);
		for (int i = 0; i < Table.Targets.Length; i++)
		{
			uint offset2 = ((ILBlock)Table.Targets[i]).Content[0].Offset;
			offset2 -= offset;
			array[num2++] = (byte)offset2;
			array[num2++] = (byte)(offset2 >> 8);
			array[num2++] = (byte)(offset2 >> 16);
			array[num2++] = (byte)(offset2 >> 24);
		}
		return array;
	}
}
