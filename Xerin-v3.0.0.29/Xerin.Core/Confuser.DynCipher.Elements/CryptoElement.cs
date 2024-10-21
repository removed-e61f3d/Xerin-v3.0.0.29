using Confuser.DynCipher.Generation;
using XCore.Generator;

namespace Confuser.DynCipher.Elements;

internal abstract class CryptoElement
{
	public int DataCount { get; private set; }

	public int[] DataIndexes { get; private set; }

	public CryptoElement(int count)
	{
		DataCount = count;
		DataIndexes = new int[count];
	}

	public abstract void Initialize(RandomGenerator random);

	public abstract void Emit(CipherGenContext context);

	public abstract void EmitInverse(CipherGenContext context);
}
