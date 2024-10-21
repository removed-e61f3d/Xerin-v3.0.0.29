using System.Collections.Generic;
using System.Text;

namespace Confuser.DynCipher.AST;

public class StatementBlock : Statement
{
	public IList<Statement> Statements { get; private set; }

	public StatementBlock()
	{
		Statements = new List<Statement>();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("{");
		foreach (Statement statement in Statements)
		{
			stringBuilder.AppendLine(statement.ToString());
		}
		stringBuilder.AppendLine("}");
		return stringBuilder.ToString();
	}
}
