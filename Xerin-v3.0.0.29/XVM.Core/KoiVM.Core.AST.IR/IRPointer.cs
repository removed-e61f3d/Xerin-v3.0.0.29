namespace KoiVM.Core.AST.IR;

public class IRPointer : IIROperand
{
	public IRRegister Register { get; set; }

	public int Offset { get; set; }

	public IRVariable SourceVariable { get; set; }

	public ASTType Type { get; set; }

	public override string ToString()
	{
		string arg = Type.ToString();
		string arg2 = "";
		if (Offset > 0)
		{
			arg2 = $" + {Offset:x}h";
		}
		else if (Offset < 0)
		{
			arg2 = $" - {-Offset:x}h";
		}
		return $"{arg}:[{Register}{arg2}]";
	}
}
