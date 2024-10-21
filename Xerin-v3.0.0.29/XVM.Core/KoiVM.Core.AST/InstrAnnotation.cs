namespace KoiVM.Core.AST;

public class InstrAnnotation
{
	public static readonly InstrAnnotation JUMP = new InstrAnnotation("JUMP");

	public string Name { get; private set; }

	public InstrAnnotation(string name)
	{
		Name = name;
	}

	public override string ToString()
	{
		return Name;
	}
}
