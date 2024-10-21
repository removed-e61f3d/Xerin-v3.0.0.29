using System.Reflection;

namespace XVM.Runtime.Data;

internal struct VMExportInfo
{
	public readonly uint CodeOffset;

	public readonly uint EntryKey;

	public unsafe readonly byte* CodeAddress;

	public readonly VMFuncSig Signature;

	public unsafe VMExportInfo(ref byte* ptr, byte* data, Module module)
	{
		CodeOffset = *(uint*)ptr;
		ptr += 4;
		if (CodeOffset != 0)
		{
			EntryKey = *(uint*)ptr;
			ptr += 4;
		}
		else
		{
			EntryKey = 0u;
		}
		CodeAddress = data + CodeOffset;
		Signature = new VMFuncSig(ref ptr, module);
	}
}
