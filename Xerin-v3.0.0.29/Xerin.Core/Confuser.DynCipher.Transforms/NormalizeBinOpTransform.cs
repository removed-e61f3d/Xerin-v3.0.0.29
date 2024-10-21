using Confuser.DynCipher.AST;

namespace Confuser.DynCipher.Transforms;

internal class NormalizeBinOpTransform
{
	private static Expression ProcessExpression(Expression exp)
	{
		if (exp is BinOpExpression)
		{
			BinOpExpression binOpExpression = (BinOpExpression)exp;
			if (binOpExpression.Right is BinOpExpression binOpExpression2 && binOpExpression2.Operation == binOpExpression.Operation && (binOpExpression.Operation == (BinOps)0 || binOpExpression.Operation == BinOps.Mul || binOpExpression.Operation == (BinOps)4 || binOpExpression.Operation == BinOps.And || binOpExpression.Operation == (BinOps)6))
			{
				binOpExpression.Left = new BinOpExpression
				{
					Left = binOpExpression.Left,
					Operation = binOpExpression.Operation,
					Right = binOpExpression2.Left
				};
				binOpExpression.Right = binOpExpression2.Right;
			}
			binOpExpression.Left = ProcessExpression(binOpExpression.Left);
			binOpExpression.Right = ProcessExpression(binOpExpression.Right);
			if (binOpExpression.Right is LiteralExpression && ((LiteralExpression)binOpExpression.Right).Value == 0 && binOpExpression.Operation == (BinOps)0)
			{
				return binOpExpression.Left;
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
