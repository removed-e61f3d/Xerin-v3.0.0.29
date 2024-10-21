using System.Collections.Generic;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.RegAlloc;

public class BlockLiveness
{
	public HashSet<IRVariable> InLive { get; }

	public HashSet<IRVariable> OutLive { get; }

	private BlockLiveness(HashSet<IRVariable> inLive, HashSet<IRVariable> outLive)
	{
		InLive = inLive;
		OutLive = outLive;
	}

	internal static BlockLiveness Empty()
	{
		return new BlockLiveness(new HashSet<IRVariable>(), new HashSet<IRVariable>());
	}

	internal BlockLiveness Clone()
	{
		return new BlockLiveness(new HashSet<IRVariable>(InLive), new HashSet<IRVariable>(OutLive));
	}
}
