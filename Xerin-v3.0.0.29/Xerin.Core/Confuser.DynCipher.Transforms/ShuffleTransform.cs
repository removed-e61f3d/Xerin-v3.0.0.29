using System.Collections.Generic;
using System.Linq;
using Confuser.DynCipher.AST;
using XCore.Generator;

namespace Confuser.DynCipher.Transforms;

internal class ShuffleTransform
{
	private class TransformContext
	{
		public Dictionary<Statement, Variable[]> Definitions;

		public Statement[] Statements;

		public Dictionary<Statement, Variable[]> Usages;
	}

	private static IEnumerable<Variable> GetVariableUsage(Expression exp)
	{
		if (exp is VariableExpression)
		{
			yield return ((VariableExpression)exp).Variable;
		}
		else if (exp is ArrayIndexExpression)
		{
			foreach (Variable item in GetVariableUsage(((ArrayIndexExpression)exp).Array))
			{
				yield return item;
			}
		}
		else if (exp is BinOpExpression)
		{
			foreach (Variable item2 in GetVariableUsage(((BinOpExpression)exp).Left).Concat(GetVariableUsage(((BinOpExpression)exp).Right)))
			{
				yield return item2;
			}
		}
		else
		{
			if (!(exp is UnaryOpExpression))
			{
				yield break;
			}
			foreach (Variable item3 in GetVariableUsage(((UnaryOpExpression)exp).Value))
			{
				yield return item3;
			}
		}
	}

	private static IEnumerable<Variable> GetVariableUsage(Statement st)
	{
		if (!(st is AssignmentStatement))
		{
			yield break;
		}
		foreach (Variable item in GetVariableUsage(((AssignmentStatement)st).Value))
		{
			yield return item;
		}
	}

	private static IEnumerable<Variable> GetVariableDefinition(Expression exp)
	{
		if (exp is VariableExpression)
		{
			yield return ((VariableExpression)exp).Variable;
		}
	}

	private static IEnumerable<Variable> GetVariableDefinition(Statement st)
	{
		if (!(st is AssignmentStatement))
		{
			yield break;
		}
		foreach (Variable item in GetVariableDefinition(((AssignmentStatement)st).Target))
		{
			yield return item;
		}
	}

	private static int SearchUpwardKill(TransformContext context, Statement st, StatementBlock block, int startIndex)
	{
		Variable[] second = context.Usages[st];
		Variable[] second2 = context.Definitions[st];
		int num = startIndex - 1;
		while (true)
		{
			if (num >= 0)
			{
				if (context.Usages[block.Statements[num]].Intersect(second2).Count() > 0 || context.Definitions[block.Statements[num]].Intersect(second).Count() > 0)
				{
					break;
				}
				num--;
				continue;
			}
			return 0;
		}
		return num;
	}

	private static int SearchDownwardKill(TransformContext context, Statement st, StatementBlock block, int startIndex)
	{
		Variable[] second = context.Usages[st];
		Variable[] second2 = context.Definitions[st];
		int num = startIndex + 1;
		while (true)
		{
			if (num < block.Statements.Count)
			{
				if (context.Usages[block.Statements[num]].Intersect(second2).Count() > 0 || context.Definitions[block.Statements[num]].Intersect(second).Count() > 0)
				{
					break;
				}
				num++;
				continue;
			}
			return block.Statements.Count - 1;
		}
		return num;
	}

	public static void Run(StatementBlock block, RandomGenerator random)
	{
		TransformContext transformContext = new TransformContext
		{
			Statements = block.Statements.ToArray(),
			Usages = block.Statements.ToDictionary((Statement s) => s, (Statement s) => GetVariableUsage(s).ToArray()),
			Definitions = block.Statements.ToDictionary((Statement s) => s, (Statement s) => GetVariableDefinition(s).ToArray())
		};
		for (int i = 0; i < 20; i++)
		{
			Statement[] statements = transformContext.Statements;
			foreach (Statement statement in statements)
			{
				int num = block.Statements.IndexOf(statement);
				GetVariableUsage(statement).Concat(GetVariableDefinition(statement)).ToArray();
				int num2 = SearchUpwardKill(transformContext, statement, block, num);
				int num3 = SearchDownwardKill(transformContext, statement, block, num);
				int num4 = num2 + random.NextInt32(1, num3 - num2);
				if (num4 > num)
				{
					num4--;
				}
				block.Statements.RemoveAt(num);
				block.Statements.Insert(num4, statement);
			}
		}
	}
}
