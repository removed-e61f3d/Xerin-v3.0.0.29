using dnlib.DotNet.Writer;

namespace XCore.Utils;

internal class RawHeap : HeapBase
{
	private readonly byte[] content;

	public override string Name { get; }

	public RawHeap(string name, byte[] content)
	{
		Name = name;
		this.content = content;
	}

	public override uint GetRawLength()
	{
		return (uint)content.Length;
	}

	protected override void WriteToImpl(DataWriter writer)
	{
		writer.WriteBytes(content);
	}
}
