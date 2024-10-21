using System;

namespace Confuser.DynCipher.AST;

public class BinOpExpression : Expression
{
	public Expression Left { get; set; }

	public Expression Right { get; set; }

	public BinOps Operation { get; set; }

	public override string ToString()
	{
		return string.Format(arg1: Operation switch
		{
			(BinOps)0 => "+", 
			BinOps.Sub => "-", 
			(BinOps)2 => "/", 
			BinOps.Mul => "*", 
			(BinOps)4 => "|", 
			BinOps.And => "&", 
			(BinOps)6 => "^", 
			BinOps.Lsh => "<<", 
			(BinOps)8 => ">>", 
			_ => throw new Exception(), 
		}, format: "({0} {1} {2})", arg0: Left, arg2: Right);
	}
}
