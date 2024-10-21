namespace Confuser.DynCipher.AST;

public abstract class Expression
{
	public object Tag { get; set; }

	public abstract override string ToString();

	public static BinOpExpression operator +(Expression a, Expression b)
	{
		return new BinOpExpression
		{
			Left = a,
			Right = b,
			Operation = (BinOps)0
		};
	}

	public static BinOpExpression operator -(Expression a, Expression b)
	{
		return new BinOpExpression
		{
			Left = a,
			Right = b,
			Operation = BinOps.Sub
		};
	}

	public static BinOpExpression operator *(Expression a, Expression b)
	{
		return new BinOpExpression
		{
			Left = a,
			Right = b,
			Operation = BinOps.Mul
		};
	}

	public static BinOpExpression operator >>(Expression a, int b)
	{
		return new BinOpExpression
		{
			Left = a,
			Right = (LiteralExpression)(uint)b,
			Operation = (BinOps)8
		};
	}

	public static BinOpExpression operator <<(Expression a, int b)
	{
		return new BinOpExpression
		{
			Left = a,
			Right = (LiteralExpression)(uint)b,
			Operation = BinOps.Lsh
		};
	}

	public static BinOpExpression operator |(Expression a, Expression b)
	{
		return new BinOpExpression
		{
			Left = a,
			Right = b,
			Operation = (BinOps)4
		};
	}

	public static BinOpExpression operator &(Expression a, Expression b)
	{
		return new BinOpExpression
		{
			Left = a,
			Right = b,
			Operation = BinOps.And
		};
	}

	public static BinOpExpression operator ^(Expression a, Expression b)
	{
		return new BinOpExpression
		{
			Left = a,
			Right = b,
			Operation = (BinOps)6
		};
	}

	public static UnaryOpExpression operator ~(Expression val)
	{
		return new UnaryOpExpression
		{
			Value = val,
			Operation = (UnaryOps)0
		};
	}

	public static UnaryOpExpression operator -(Expression val)
	{
		return new UnaryOpExpression
		{
			Value = val,
			Operation = UnaryOps.Negate
		};
	}
}
