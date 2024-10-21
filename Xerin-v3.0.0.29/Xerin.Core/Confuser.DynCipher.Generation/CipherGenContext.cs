using System;
using System.Collections.Generic;
using Confuser.DynCipher.AST;
using XCore.Generator;

namespace Confuser.DynCipher.Generation;

internal class CipherGenContext
{
	private struct TempVarHolder : IDisposable
	{
		private readonly CipherGenContext parent;

		private readonly Variable tempVar;

		public TempVarHolder(CipherGenContext p, Variable v)
		{
			parent = p;
			tempVar = v;
		}

		public void Dispose()
		{
			parent.tempVars.Add(tempVar);
		}
	}

	private readonly Variable[] dataVars;

	private readonly Variable keyVar = new Variable("{KEY}");

	private readonly RandomGenerator random;

	private readonly List<Variable> tempVars = new List<Variable>();

	private int tempVarCounter;

	public StatementBlock Block { get; private set; }

	public CipherGenContext(RandomGenerator random, int dataVarCount)
	{
		this.random = random;
		Block = new StatementBlock();
		dataVars = new Variable[dataVarCount];
		for (int i = 0; i < dataVarCount; i++)
		{
			dataVars[i] = new Variable("v" + i)
			{
				Tag = i
			};
		}
	}

	public Expression GetDataExpression(int index)
	{
		return new VariableExpression
		{
			Variable = dataVars[index]
		};
	}

	public Expression GetKeyExpression(int index)
	{
		return new ArrayIndexExpression
		{
			Array = new VariableExpression
			{
				Variable = keyVar
			},
			Index = index
		};
	}

	public CipherGenContext Emit(Statement statement)
	{
		Block.Statements.Add(statement);
		return this;
	}

	public IDisposable AcquireTempVar(out VariableExpression exp)
	{
		Variable variable;
		if (tempVars.Count == 0)
		{
			variable = new Variable("t" + tempVarCounter++);
		}
		else
		{
			variable = tempVars[random.NextInt32(tempVars.Count)];
			tempVars.Remove(variable);
		}
		exp = new VariableExpression
		{
			Variable = variable
		};
		return new TempVarHolder(this, variable);
	}
}
