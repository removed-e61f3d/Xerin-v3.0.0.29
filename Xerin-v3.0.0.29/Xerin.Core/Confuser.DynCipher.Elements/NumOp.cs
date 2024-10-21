using Confuser.DynCipher.AST;
using Confuser.DynCipher.Generation;
using XCore.Generator;

namespace Confuser.DynCipher.Elements;

internal class NumOp : CryptoElement
{
	public uint Key { get; private set; }

	public uint InverseKey { get; private set; }

	public CryptoNumOps Operation { get; private set; }

	public NumOp()
		: base(1)
	{
	}

	public override void Initialize(RandomGenerator random)
	{
		Operation = (CryptoNumOps)random.NextInt32(4);
		switch (Operation)
		{
		case CryptoNumOps.Mul:
			Key = random.NextUInt32() | 1u;
			InverseKey = MathsUtils.modInv(Key);
			break;
		case (CryptoNumOps)0:
		case (CryptoNumOps)2:
		{
			uint key = (InverseKey = random.NextUInt32());
			Key = key;
			break;
		}
		case CryptoNumOps.Xnor:
			Key = random.NextUInt32();
			InverseKey = ~Key;
			break;
		}
	}

	public override void Emit(CipherGenContext context)
	{
		Expression dataExpression = context.GetDataExpression(base.DataIndexes[0]);
		switch (Operation)
		{
		case (CryptoNumOps)0:
			context.Emit(new AssignmentStatement
			{
				Value = dataExpression + (LiteralExpression)Key,
				Target = dataExpression
			});
			break;
		case CryptoNumOps.Mul:
			context.Emit(new AssignmentStatement
			{
				Value = dataExpression * (LiteralExpression)Key,
				Target = dataExpression
			});
			break;
		case (CryptoNumOps)2:
			context.Emit(new AssignmentStatement
			{
				Value = (dataExpression ^ (LiteralExpression)Key),
				Target = dataExpression
			});
			break;
		case CryptoNumOps.Xnor:
			context.Emit(new AssignmentStatement
			{
				Value = ~(dataExpression ^ (LiteralExpression)Key),
				Target = dataExpression
			});
			break;
		}
	}

	public override void EmitInverse(CipherGenContext context)
	{
		Expression dataExpression = context.GetDataExpression(base.DataIndexes[0]);
		switch (Operation)
		{
		case (CryptoNumOps)0:
			context.Emit(new AssignmentStatement
			{
				Value = dataExpression - (LiteralExpression)InverseKey,
				Target = dataExpression
			});
			break;
		case CryptoNumOps.Mul:
			context.Emit(new AssignmentStatement
			{
				Value = dataExpression * (LiteralExpression)InverseKey,
				Target = dataExpression
			});
			break;
		case (CryptoNumOps)2:
			context.Emit(new AssignmentStatement
			{
				Value = (dataExpression ^ (LiteralExpression)InverseKey),
				Target = dataExpression
			});
			break;
		case CryptoNumOps.Xnor:
			context.Emit(new AssignmentStatement
			{
				Value = (dataExpression ^ (LiteralExpression)InverseKey),
				Target = dataExpression
			});
			break;
		}
	}
}
