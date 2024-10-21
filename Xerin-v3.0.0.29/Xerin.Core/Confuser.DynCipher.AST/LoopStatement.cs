using System.Text;

namespace Confuser.DynCipher.AST;

public class LoopStatement : StatementBlock
{
	public int Begin { get; set; }

	public int Limit { get; set; }

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("for (int i = {0}; i < {1}; i++)", Begin, Limit);
		stringBuilder.AppendLine();
		stringBuilder.Append(base.ToString());
		return stringBuilder.ToString();
	}
}
