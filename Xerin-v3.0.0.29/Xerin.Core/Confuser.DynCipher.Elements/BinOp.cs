using Confuser.DynCipher.AST;
using Confuser.DynCipher.Generation;
using XCore.Generator;

namespace Confuser.DynCipher.Elements;

internal class BinOp : CryptoElement
{
	public CryptoBinOps Operation { get; private set; }

	public BinOp()
		: base(2)
	{
	}

	public override void Initialize(RandomGenerator random)
	{
		Operation = (CryptoBinOps)random.NextInt32(3);
	}

	public override void Emit(CipherGenContext context)
	{
		Expression dataExpression = context.GetDataExpression(base.DataIndexes[0]);
		Expression dataExpression2 = context.GetDataExpression(base.DataIndexes[1]);
		switch (Operation)
		{
		case (CryptoBinOps)0:
			context.Emit(new AssignmentStatement
			{
				Value = dataExpression + dataExpression2,
				Target = dataExpression
			});
			break;
		case CryptoBinOps.Xor:
			context.Emit(new AssignmentStatement
			{
				Value = (dataExpression ^ dataExpression2),
				Target = dataExpression
			});
			break;
		case (CryptoBinOps)2:
			context.Emit(new AssignmentStatement
			{
				Value = ~(dataExpression ^ dataExpression2),
				Target = dataExpression
			});
			break;
		}
	}

	public override void EmitInverse(CipherGenContext context)
	{
		Expression dataExpression = context.GetDataExpression(base.DataIndexes[0]);
		Expression dataExpression2 = context.GetDataExpression(base.DataIndexes[1]);
		switch (Operation)
		{
		case (CryptoBinOps)0:
			context.Emit(new AssignmentStatement
			{
				Value = dataExpression - dataExpression2,
				Target = dataExpression
			});
			break;
		case CryptoBinOps.Xor:
			context.Emit(new AssignmentStatement
			{
				Value = (dataExpression ^ dataExpression2),
				Target = dataExpression
			});
			break;
		case (CryptoBinOps)2:
			context.Emit(new AssignmentStatement
			{
				Value = (dataExpression ^ ~dataExpression2),
				Target = dataExpression
			});
			break;
		}
	}
}
