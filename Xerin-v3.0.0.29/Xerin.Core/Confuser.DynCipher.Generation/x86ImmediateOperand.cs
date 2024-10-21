namespace Confuser.DynCipher.Generation;

public class x86ImmediateOperand : Ix86Operand
{
	public int Immediate { get; set; }

	public x86ImmediateOperand(int imm)
	{
		Immediate = imm;
	}

	public override string ToString()
	{
		return Immediate.ToString("X") + "h";
	}
}
