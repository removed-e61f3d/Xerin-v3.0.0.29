using System;

namespace Confuser.DynCipher.AST;

public class UnaryOpExpression : Expression
{
	public Expression Value { get; set; }

	public UnaryOps Operation { get; set; }

	public override string ToString()
	{
		return Operation switch
		{
			UnaryOps.Negate => "-", 
			(UnaryOps)0 => "~", 
			_ => throw new Exception(), 
		} + Value;
	}
}
