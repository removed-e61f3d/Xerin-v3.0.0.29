using System;

namespace KoiVM.Core.RT;

public class OffsetComputeEventArgs : EventArgs
{
	public uint Offset { get; private set; }

	internal OffsetComputeEventArgs(uint offset)
	{
		Offset = offset;
	}
}
