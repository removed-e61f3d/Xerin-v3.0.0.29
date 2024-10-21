using System.Collections.Generic;
using System.Linq;
using Confuser.DynCipher.AST;
using XCore.Generator;

namespace Confuser.DynCipher.Generation;

public class StatementGenerator
{
	private static LoopStatement GenerateInverse(LoopStatement encodeLoop, Expression var, Dictionary<AssignmentStatement, (Expression encode, Expression inverse)> assignments)
	{
		LoopStatement loopStatement = new LoopStatement
		{
			Begin = encodeLoop.Begin,
			Limit = encodeLoop.Limit
		};
		foreach (KeyValuePair<AssignmentStatement, (Expression, Expression)> item in assignments.Reverse())
		{
			loopStatement.Statements.Add(new AssignmentStatement
			{
				Target = var,
				Value = item.Value.Item2
			});
		}
		return loopStatement;
	}

	public static void GeneratePair(RandomGenerator random, Expression var, Expression result, int depth, out LoopStatement statement, out LoopStatement inverse)
	{
		statement = new LoopStatement
		{
			Begin = 1,
			Limit = depth
		};
		Dictionary<AssignmentStatement, (Expression, Expression)> dictionary = new Dictionary<AssignmentStatement, (Expression, Expression)>();
		for (int i = 0; i < depth; i++)
		{
			ExpressionGenerator.GeneratePair(random, var, result, depth, out var expression, out var inverse2);
			AssignmentStatement assignmentStatement = new AssignmentStatement
			{
				Target = var,
				Value = expression
			};
			dictionary.Add(assignmentStatement, (expression, inverse2));
			statement.Statements.Add(assignmentStatement);
		}
		inverse = GenerateInverse(statement, result, dictionary);
	}
}
