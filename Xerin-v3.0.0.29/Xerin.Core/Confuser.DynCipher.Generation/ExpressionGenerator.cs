#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Confuser.DynCipher.AST;
using XCore.Generator;

namespace Confuser.DynCipher.Generation;

public class ExpressionGenerator
{
	private enum ExpressionOps
	{
		Add,
		Sub,
		Mul,
		Xor,
		Not,
		Neg
	}

	private static Expression GenerateExpression(RandomGenerator random, Expression current, int currentDepth, int targetDepth)
	{
		if (currentDepth == targetDepth || (currentDepth > targetDepth / 3 && random.NextInt32(100) > 85))
		{
			return current;
		}
		return (ExpressionOps)random.NextInt32(1) switch
		{
			ExpressionOps.Add => GenerateExpression(random, current, currentDepth + 1, targetDepth) + GenerateExpression(random, (LiteralExpression)random.NextUInt32(), currentDepth + 1, targetDepth), 
			ExpressionOps.Sub => GenerateExpression(random, current, currentDepth + 1, targetDepth) - GenerateExpression(random, (LiteralExpression)random.NextUInt32(), currentDepth + 1, targetDepth), 
			ExpressionOps.Mul => GenerateExpression(random, current, currentDepth + 1, targetDepth) * (LiteralExpression)(random.NextUInt32() | 1u), 
			ExpressionOps.Xor => GenerateExpression(random, current, currentDepth + 1, targetDepth) ^ GenerateExpression(random, (LiteralExpression)random.NextUInt32(), currentDepth + 1, targetDepth), 
			ExpressionOps.Not => ~GenerateExpression(random, current, currentDepth + 1, targetDepth), 
			ExpressionOps.Neg => -GenerateExpression(random, current, currentDepth + 1, targetDepth), 
			_ => throw new Exception(), 
		};
	}

	private static void SwapOperands(RandomGenerator random, Expression exp)
	{
		if (exp is BinOpExpression)
		{
			BinOpExpression binOpExpression = (BinOpExpression)exp;
			if (random.NextBoolean())
			{
				Expression left = binOpExpression.Left;
				binOpExpression.Left = binOpExpression.Right;
				binOpExpression.Right = left;
			}
			SwapOperands(random, binOpExpression.Left);
			SwapOperands(random, binOpExpression.Right);
		}
		else if (exp is UnaryOpExpression)
		{
			SwapOperands(random, ((UnaryOpExpression)exp).Value);
		}
		else if (!(exp is LiteralExpression) && !(exp is VariableExpression))
		{
			throw new Exception();
		}
	}

	private static bool HasVariable(Expression exp, Dictionary<Expression, bool> hasVar)
	{
		if (!hasVar.TryGetValue(exp, out var value))
		{
			if (exp is VariableExpression)
			{
				value = true;
			}
			else if (exp is LiteralExpression)
			{
				value = false;
			}
			else if (exp is BinOpExpression)
			{
				BinOpExpression binOpExpression = (BinOpExpression)exp;
				value = HasVariable(binOpExpression.Left, hasVar) || HasVariable(binOpExpression.Right, hasVar);
			}
			else
			{
				if (!(exp is UnaryOpExpression))
				{
					throw new Exception();
				}
				value = HasVariable(((UnaryOpExpression)exp).Value, hasVar);
			}
			hasVar[exp] = value;
		}
		return value;
	}

	private static Expression GenerateInverse(Expression exp, Expression var, Dictionary<Expression, bool> hasVar)
	{
		Expression expression = var;
		while (!(exp is VariableExpression))
		{
			Debug.Assert(hasVar[exp]);
			if (exp is UnaryOpExpression)
			{
				UnaryOpExpression unaryOpExpression = (UnaryOpExpression)exp;
				expression = new UnaryOpExpression
				{
					Operation = unaryOpExpression.Operation,
					Value = expression
				};
				exp = unaryOpExpression.Value;
			}
			else if (exp is BinOpExpression)
			{
				BinOpExpression binOpExpression = (BinOpExpression)exp;
				bool flag;
				Expression expression2 = ((flag = hasVar[binOpExpression.Left]) ? binOpExpression.Left : binOpExpression.Right);
				Expression expression3 = (flag ? binOpExpression.Right : binOpExpression.Left);
				if (binOpExpression.Operation == (BinOps)0)
				{
					expression = new BinOpExpression
					{
						Operation = BinOps.Sub,
						Left = expression,
						Right = expression3
					};
				}
				else if (binOpExpression.Operation == BinOps.Sub)
				{
					expression = ((!flag) ? new BinOpExpression
					{
						Operation = BinOps.Sub,
						Left = expression3,
						Right = expression
					} : new BinOpExpression
					{
						Operation = (BinOps)0,
						Left = expression,
						Right = expression3
					});
				}
				else if (binOpExpression.Operation == BinOps.Mul)
				{
					Debug.Assert(expression3 is LiteralExpression);
					uint value = ((LiteralExpression)expression3).Value;
					value = MathsUtils.modInv(value);
					expression = new BinOpExpression
					{
						Operation = BinOps.Mul,
						Left = expression,
						Right = (LiteralExpression)value
					};
				}
				else if (binOpExpression.Operation == (BinOps)6)
				{
					expression = new BinOpExpression
					{
						Operation = (BinOps)6,
						Left = expression,
						Right = expression3
					};
				}
				exp = expression2;
			}
		}
		return expression;
	}

	public static void GeneratePair(RandomGenerator random, Expression var, Expression result, int depth, out Expression expression, out Expression inverse)
	{
		expression = GenerateExpression(random, var, 0, depth);
		SwapOperands(random, expression);
		Dictionary<Expression, bool> hasVar = new Dictionary<Expression, bool>();
		HasVariable(expression, hasVar);
		inverse = GenerateInverse(expression, result, hasVar);
	}
}
