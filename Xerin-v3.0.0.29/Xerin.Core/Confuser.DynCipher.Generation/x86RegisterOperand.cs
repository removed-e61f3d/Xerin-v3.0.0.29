namespace Confuser.DynCipher.Generation;

public class x86RegisterOperand : Ix86Operand
{
	public x86Register Register { get; set; }

	public x86RegisterOperand(x86Register reg)
	{
		Register = reg;
	}

	public override string ToString()
	{
		return Register.ToString();
	}
}
