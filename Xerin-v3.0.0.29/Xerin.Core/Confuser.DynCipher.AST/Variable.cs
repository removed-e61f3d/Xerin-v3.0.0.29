namespace Confuser.DynCipher.AST;

public class Variable
{
	public string Name { get; set; }

	public object Tag { get; set; }

	public Variable(string name)
	{
		Name = name;
	}

	public override string ToString()
	{
		return Name;
	}
}
