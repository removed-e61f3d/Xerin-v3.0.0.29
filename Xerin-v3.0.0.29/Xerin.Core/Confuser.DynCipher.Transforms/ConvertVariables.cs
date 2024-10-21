using Confuser.DynCipher.AST;

namespace Confuser.DynCipher.Transforms;

internal class ConvertVariables
{
	private static Expression ReplaceVar(Expression exp, Variable buff)
	{
		if (exp is VariableExpression)
		{
			if (((VariableExpression)exp).Variable.Name[0] != 'v')
			{
				return exp;
			}
			ArrayIndexExpression arrayIndexExpression = new ArrayIndexExpression();
			arrayIndexExpression.Array = new VariableExpression
			{
				Variable = buff
			};
			arrayIndexExpression.Index = (int)(exp as VariableExpression).Variable.Tag;
			return arrayIndexExpression;
		}
		if (exp is ArrayIndexExpression)
		{
			((ArrayIndexExpression)exp).Array = ReplaceVar(((ArrayIndexExpression)exp).Array, buff);
		}
		else if (exp is BinOpExpression)
		{
			((BinOpExpression)exp).Left = ReplaceVar(((BinOpExpression)exp).Left, buff);
			((BinOpExpression)exp).Right = ReplaceVar(((BinOpExpression)exp).Right, buff);
		}
		else if (exp is UnaryOpExpression)
		{
			((UnaryOpExpression)exp).Value = ReplaceVar(((UnaryOpExpression)exp).Value, buff);
		}
		return exp;
	}

	private static Statement ReplaceVar(Statement st, Variable buff)
	{
		if (st is AssignmentStatement)
		{
			((AssignmentStatement)st).Value = ReplaceVar(((AssignmentStatement)st).Value, buff);
			((AssignmentStatement)st).Target = ReplaceVar(((AssignmentStatement)st).Target, buff);
		}
		return st;
	}

	public static void Run(StatementBlock block)
	{
		Variable buff = new Variable("{BUFFER}");
		for (int i = 0; i < block.Statements.Count; i++)
		{
			block.Statements[i] = ReplaceVar(block.Statements[i], buff);
		}
	}
}
