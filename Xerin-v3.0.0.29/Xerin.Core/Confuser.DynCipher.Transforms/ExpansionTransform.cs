using System.Linq;
using Confuser.DynCipher.AST;

namespace Confuser.DynCipher.Transforms;

internal class ExpansionTransform
{
	private static bool ProcessStatement(Statement st, StatementBlock block)
	{
		if (st is AssignmentStatement)
		{
			AssignmentStatement assignmentStatement = (AssignmentStatement)st;
			if (assignmentStatement.Value is BinOpExpression)
			{
				BinOpExpression binOpExpression = (BinOpExpression)assignmentStatement.Value;
				if ((binOpExpression.Left is BinOpExpression || binOpExpression.Right is BinOpExpression) && binOpExpression.Left != assignmentStatement.Target)
				{
					block.Statements.Add(new AssignmentStatement
					{
						Target = assignmentStatement.Target,
						Value = binOpExpression.Left
					});
					block.Statements.Add(new AssignmentStatement
					{
						Target = assignmentStatement.Target,
						Value = new BinOpExpression
						{
							Left = assignmentStatement.Target,
							Operation = binOpExpression.Operation,
							Right = binOpExpression.Right
						}
					});
					return true;
				}
			}
		}
		block.Statements.Add(st);
		return false;
	}

	public static void Run(StatementBlock block)
	{
		bool flag;
		do
		{
			flag = false;
			Statement[] array = block.Statements.ToArray();
			block.Statements.Clear();
			Statement[] array2 = array;
			foreach (Statement st in array2)
			{
				flag |= ProcessStatement(st, block);
			}
		}
		while (flag);
	}
}
