using System.Linq;
using KoiVM.Core.Services;

namespace KoiVM.Core.VM;

public class RTFlagDescriptor
{
	private readonly byte[] ehOrder = (from x in Enumerable.Range(0, 4)
		select (byte)x).ToArray();

	private readonly byte[] flagOrder = (from x in Enumerable.Range(1, 7)
		select (byte)x).ToArray();

	public byte INSTANCE => flagOrder[0];

	public byte EH_CATCH => ehOrder[0];

	public byte EH_FILTER => ehOrder[1];

	public byte EH_FAULT => ehOrder[2];

	public byte EH_FINALLY => ehOrder[3];

	internal RTFlagDescriptor(RandomGenerator randomGenerator)
	{
		randomGenerator.Shuffle(flagOrder);
		randomGenerator.Shuffle(ehOrder);
	}
}
