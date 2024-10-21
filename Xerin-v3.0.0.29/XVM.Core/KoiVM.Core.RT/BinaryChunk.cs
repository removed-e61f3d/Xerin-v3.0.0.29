using System;

namespace KoiVM.Core.RT;

public class BinaryChunk : IChunk
{
	public EventHandler<OffsetComputeEventArgs> OffsetComputed;

	public byte[] Data { get; private set; }

	public uint Offset { get; private set; }

	uint IChunk.Length => (uint)Data.Length;

	public BinaryChunk(byte[] data)
	{
		Data = data;
	}

	void IChunk.OnOffsetComputed(uint offset)
	{
		if (OffsetComputed != null)
		{
			OffsetComputed(this, new OffsetComputeEventArgs(offset));
		}
		Offset = offset;
	}

	byte[] IChunk.GetData()
	{
		return Data;
	}
}
