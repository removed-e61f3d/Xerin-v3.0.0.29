using System.Text;

namespace KoiVM.Core.AST.ILAST;

public class ILASTPhi : ASTNode, IILASTStatement
{
	public ILASTVariable Variable { get; set; }

	public ILASTVariable[] SourceVariables { get; set; }

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("{0} = [", Variable);
		for (int i = 0; i < SourceVariables.Length; i++)
		{
			if (i != 0)
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(SourceVariables[i]);
		}
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}
}
