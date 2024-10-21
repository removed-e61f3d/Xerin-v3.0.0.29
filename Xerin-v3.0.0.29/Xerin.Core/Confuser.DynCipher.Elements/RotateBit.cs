using Confuser.DynCipher.AST;
using Confuser.DynCipher.Generation;
using XCore.Generator;

namespace Confuser.DynCipher.Elements;

internal class RotateBit : CryptoElement
{
	public int Bits { get; private set; }

	public bool IsAlternate { get; private set; }

	public RotateBit()
		: base(1)
	{
	}

	public override void Initialize(RandomGenerator random)
	{
		Bits = random.NextInt32(1, 32);
		IsAlternate = random.NextInt32() % 2 == 0;
	}

	public override void Emit(CipherGenContext context)
	{
		Expression dataExpression = context.GetDataExpression(base.DataIndexes[0]);
		VariableExpression exp;
		using (context.AcquireTempVar(out exp))
		{
			if (IsAlternate)
			{
				context.Emit(new AssignmentStatement
				{
					Value = dataExpression >> 32 - Bits,
					Target = exp
				}).Emit(new AssignmentStatement
				{
					Value = ((dataExpression << Bits) | exp),
					Target = dataExpression
				});
			}
			else
			{
				context.Emit(new AssignmentStatement
				{
					Value = dataExpression << 32 - Bits,
					Target = exp
				}).Emit(new AssignmentStatement
				{
					Value = ((dataExpression >> Bits) | exp),
					Target = dataExpression
				});
			}
		}
	}

	public override void EmitInverse(CipherGenContext context)
	{
		Expression dataExpression = context.GetDataExpression(base.DataIndexes[0]);
		VariableExpression exp;
		using (context.AcquireTempVar(out exp))
		{
			if (IsAlternate)
			{
				context.Emit(new AssignmentStatement
				{
					Value = dataExpression << 32 - Bits,
					Target = exp
				}).Emit(new AssignmentStatement
				{
					Value = ((dataExpression >> Bits) | exp),
					Target = dataExpression
				});
			}
			else
			{
				context.Emit(new AssignmentStatement
				{
					Value = dataExpression >> 32 - Bits,
					Target = exp
				}).Emit(new AssignmentStatement
				{
					Value = ((dataExpression << Bits) | exp),
					Target = dataExpression
				});
			}
		}
	}
}
