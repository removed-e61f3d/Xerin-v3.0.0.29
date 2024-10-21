using KoiVM.Core.CFG;
using KoiVM.Core.RT;

namespace KoiVM.Core.AST.IL;

public class ILJumpTable : IILOperand, IHasOffset
{
	public JumpTableChunk Chunk { get; private set; }

	public ILInstruction RelativeBase { get; set; }

	public IBasicBlock[] Targets { get; set; }

	public uint Offset => Chunk.Offset;

	public ILJumpTable(IBasicBlock[] targets)
	{
		Targets = targets;
		Chunk = new JumpTableChunk(this);
	}

	public override string ToString()
	{
		return $"[..{Targets.Length}..]";
	}
}
