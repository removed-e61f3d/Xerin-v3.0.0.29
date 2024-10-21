using System;
using Confuser.DynCipher.AST;
using Confuser.DynCipher.Generation;
using XCore.Generator;

namespace Confuser.DynCipher.Elements;

internal class Matrix : CryptoElement
{
	public uint[,] Key { get; private set; }

	public uint[,] InverseKey { get; private set; }

	public Matrix()
		: base(4)
	{
	}

	private static uint[,] GenerateUnimodularMatrix(RandomGenerator random)
	{
		Func<uint> func = () => (uint)random.NextInt32(4);
		uint[,] obj = new uint[4, 4]
		{
			{ 1u, 0u, 0u, 0u },
			{ 0u, 1u, 0u, 0u },
			{ 0u, 0u, 1u, 0u },
			{ 0u, 0u, 0u, 1u }
		};
		obj[1, 0] = func();
		obj[2, 0] = func();
		obj[2, 1] = func();
		obj[3, 0] = func();
		obj[3, 1] = func();
		obj[3, 2] = func();
		uint[,] a = obj;
		uint[,] obj2 = new uint[4, 4]
		{
			{ 1u, 0u, 0u, 0u },
			{ 0u, 1u, 0u, 0u },
			{ 0u, 0u, 1u, 0u },
			{ 0u, 0u, 0u, 1u }
		};
		obj2[0, 1] = func();
		obj2[0, 2] = func();
		obj2[0, 3] = func();
		obj2[1, 2] = func();
		obj2[1, 3] = func();
		obj2[2, 3] = func();
		uint[,] b = obj2;
		return mul(a, b);
	}

	private static uint[,] mul(uint[,] a, uint[,] b)
	{
		int length = a.GetLength(0);
		int length2 = b.GetLength(1);
		int length3 = a.GetLength(1);
		if (b.GetLength(0) != length3)
		{
			return null;
		}
		uint[,] array = new uint[length, length2];
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				array[i, j] = 0u;
				for (int k = 0; k < length3; k++)
				{
					array[i, j] += a[i, k] * b[k, j];
				}
			}
		}
		return array;
	}

	private static uint cofactor4(uint[,] mat, int i, int j)
	{
		uint[,] array = new uint[3, 3];
		int num = 0;
		int num2 = 0;
		while (num < 4)
		{
			if (num == i)
			{
				num2--;
			}
			else
			{
				int num3 = 0;
				int num4 = 0;
				while (num3 < 4)
				{
					if (num3 == j)
					{
						num4--;
					}
					else
					{
						array[num2, num4] = mat[num, num3];
					}
					num3++;
					num4++;
				}
			}
			num++;
			num2++;
		}
		uint num5 = det3(array);
		if ((i + j) % 2 == 0)
		{
			return num5;
		}
		return (uint)(0uL - (ulong)num5);
	}

	private static uint det3(uint[,] mat)
	{
		return mat[0, 0] * mat[1, 1] * mat[2, 2] + mat[0, 1] * mat[1, 2] * mat[2, 0] + mat[0, 2] * mat[1, 0] * mat[2, 1] - mat[0, 2] * mat[1, 1] * mat[2, 0] - mat[0, 1] * mat[1, 0] * mat[2, 2] - mat[0, 0] * mat[1, 2] * mat[2, 1];
	}

	private static uint[,] transpose4(uint[,] mat)
	{
		uint[,] array = new uint[4, 4];
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				array[j, i] = mat[i, j];
			}
		}
		return array;
	}

	public override void Initialize(RandomGenerator random)
	{
		InverseKey = mul(transpose4(GenerateUnimodularMatrix(random)), GenerateUnimodularMatrix(random));
		uint[,] array = new uint[4, 4];
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				array[i, j] = cofactor4(InverseKey, i, j);
			}
		}
		Key = transpose4(array);
	}

	private void EmitCore(CipherGenContext context, uint[,] k)
	{
		Expression dataExpression = context.GetDataExpression(base.DataIndexes[0]);
		Expression dataExpression2 = context.GetDataExpression(base.DataIndexes[1]);
		Expression dataExpression3 = context.GetDataExpression(base.DataIndexes[2]);
		Expression dataExpression4 = context.GetDataExpression(base.DataIndexes[3]);
		Func<uint, LiteralExpression> func = (uint v) => v;
		VariableExpression exp;
		using (context.AcquireTempVar(out exp))
		{
			VariableExpression exp2;
			using (context.AcquireTempVar(out exp2))
			{
				VariableExpression exp3;
				using (context.AcquireTempVar(out exp3))
				{
					VariableExpression exp4;
					using (context.AcquireTempVar(out exp4))
					{
						context.Emit(new AssignmentStatement
						{
							Value = dataExpression * func(k[0, 0]) + dataExpression2 * func(k[0, 1]) + dataExpression3 * func(k[0, 2]) + dataExpression4 * func(k[0, 3]),
							Target = exp
						}).Emit(new AssignmentStatement
						{
							Value = dataExpression * func(k[1, 0]) + dataExpression2 * func(k[1, 1]) + dataExpression3 * func(k[1, 2]) + dataExpression4 * func(k[1, 3]),
							Target = exp2
						}).Emit(new AssignmentStatement
						{
							Value = dataExpression * func(k[2, 0]) + dataExpression2 * func(k[2, 1]) + dataExpression3 * func(k[2, 2]) + dataExpression4 * func(k[2, 3]),
							Target = exp3
						})
							.Emit(new AssignmentStatement
							{
								Value = dataExpression * func(k[3, 0]) + dataExpression2 * func(k[3, 1]) + dataExpression3 * func(k[3, 2]) + dataExpression4 * func(k[3, 3]),
								Target = exp4
							})
							.Emit(new AssignmentStatement
							{
								Value = exp,
								Target = dataExpression
							})
							.Emit(new AssignmentStatement
							{
								Value = exp2,
								Target = dataExpression2
							})
							.Emit(new AssignmentStatement
							{
								Value = exp3,
								Target = dataExpression3
							})
							.Emit(new AssignmentStatement
							{
								Value = exp4,
								Target = dataExpression4
							});
					}
				}
			}
		}
	}

	public override void Emit(CipherGenContext context)
	{
		EmitCore(context, Key);
	}

	public override void EmitInverse(CipherGenContext context)
	{
		EmitCore(context, InverseKey);
	}
}
