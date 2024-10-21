using Confuser.DynCipher.AST;
using Confuser.DynCipher.Generation;
using XCore.Generator;

namespace Confuser.DynCipher.Elements;

internal class Swap : CryptoElement
{
	public uint Mask { get; private set; }

	public uint Key { get; private set; }

	public Swap()
		: base(2)
	{
	}

	public override void Initialize(RandomGenerator random)
	{
		if (random.NextInt32(3) == 0)
		{
			Mask = uint.MaxValue;
		}
		else
		{
			Mask = random.NextUInt32();
		}
		Key = random.NextUInt32() | 1u;
	}

	private void EmitCore(CipherGenContext context)
	{
		Expression dataExpression = context.GetDataExpression(base.DataIndexes[0]);
		Expression dataExpression2 = context.GetDataExpression(base.DataIndexes[1]);
		VariableExpression exp;
		if (Mask == uint.MaxValue)
		{
			using (context.AcquireTempVar(out exp))
			{
				context.Emit(new AssignmentStatement
				{
					Value = dataExpression * (LiteralExpression)Key,
					Target = exp
				}).Emit(new AssignmentStatement
				{
					Value = dataExpression2,
					Target = dataExpression
				}).Emit(new AssignmentStatement
				{
					Value = exp * (LiteralExpression)MathsUtils.modInv(Key),
					Target = dataExpression2
				});
				return;
			}
		}
		LiteralExpression literalExpression = Mask;
		LiteralExpression literalExpression2 = ~Mask;
		using (context.AcquireTempVar(out exp))
		{
			context.Emit(new AssignmentStatement
			{
				Value = (dataExpression & literalExpression) * (LiteralExpression)Key,
				Target = exp
			}).Emit(new AssignmentStatement
			{
				Value = ((dataExpression & literalExpression2) | (dataExpression2 & literalExpression)),
				Target = dataExpression
			}).Emit(new AssignmentStatement
			{
				Value = ((dataExpression2 & literalExpression2) | (exp * (LiteralExpression)MathsUtils.modInv(Key))),
				Target = dataExpression2
			});
		}
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
