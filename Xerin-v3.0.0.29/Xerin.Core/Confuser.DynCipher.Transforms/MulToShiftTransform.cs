using System.Collections.Generic;
using System.Linq;
using Confuser.DynCipher.AST;

namespace Confuser.DynCipher.Transforms;

internal class MulToShiftTransform
{
	private static uint NumberOfSetBits(uint i)
	{
		i -= (i >> 1) & 0x55555555;
		i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
		return ((i + (i >> 4)) & 0xF0F0F0F) * 16843009 >> 24;
	}

	private static Expression ProcessExpression(Expression exp)
	{
		if (exp is BinOpExpression)
		{
			BinOpExpression binOpExpression = (BinOpExpression)exp;
			if (binOpExpression.Operation == BinOps.Mul && binOpExpression.Right is LiteralExpression)
			{
				uint num = ((LiteralExpression)binOpExpression.Right).Value;
				switch (num)
				{
				case 0u:
					return (LiteralExpression)0u;
				case 1u:
					return binOpExpression.Left;
				}
				uint num2 = NumberOfSetBits(num);
				if (num2 <= 2)
				{
					List<Expression> list = new List<Expression>();
					int num3 = 0;
					while (num != 0)
					{
						if ((num & (true ? 1u : 0u)) != 0)
						{
							if (num3 == 0)
							{
								list.Add(binOpExpression.Left);
							}
							else
							{
								list.Add(binOpExpression.Left << num3);
							}
						}
						num >>= 1;
						num3++;
					}
					BinOpExpression binOpExpression2 = list.OfType<BinOpExpression>().First();
					foreach (Expression item in list.Except(new BinOpExpression[1] { binOpExpression2 }))
					{
						binOpExpression2 += item;
					}
					return binOpExpression2;
				}
			}
			else
			{
				binOpExpression.Left = ProcessExpression(binOpExpression.Left);
				binOpExpression.Right = ProcessExpression(binOpExpression.Right);
			}
		}
		else if (exp is ArrayIndexExpression)
		{
			((ArrayIndexExpression)exp).Array = ProcessExpression(((ArrayIndexExpression)exp).Array);
		}
		else if (exp is UnaryOpExpression)
		{
			((UnaryOpExpression)exp).Value = ProcessExpression(((UnaryOpExpression)exp).Value);
		}
		return exp;
	}

	private static void ProcessStatement(Statement st)
	{
		if (st is AssignmentStatement)
		{
			AssignmentStatement assignmentStatement = (AssignmentStatement)st;
			assignmentStatement.Target = ProcessExpression(assignmentStatement.Target);
			assignmentStatement.Value = ProcessExpression(assignmentStatement.Value);
		}
	}

	public static void Run(StatementBlock block)
	{
		foreach (Statement statement in block.Statements)
		{
			ProcessStatement(statement);
		}
	}
}
