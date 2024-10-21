using Confuser.DynCipher.AST;
using Confuser.DynCipher.Generation;
using XCore.Generator;

namespace Confuser.DynCipher.Elements;

internal class AddKey : CryptoElement
{
	public int Index { get; private set; }

	public AddKey(int index)
		: base(0)
	{
		Index = index;
	}

	public override void Initialize(RandomGenerator random)
	{
	}

	private void EmitCore(CipherGenContext context)
	{
		Expression dataExpression = context.GetDataExpression(Index);
		context.Emit(new AssignmentStatement
		{
			Value = (dataExpression ^ context.GetKeyExpression(Index)),
			Target = dataExpression
		});
	}

	public override void Emit(CipherGenContext context)
	{
		EmitCore(context);
	}

	public override void EmitInverse(CipherGenContext context)
	{
		EmitCore(context);
	}
}
