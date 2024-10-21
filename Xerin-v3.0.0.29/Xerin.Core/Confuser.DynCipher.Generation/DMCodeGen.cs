using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Confuser.DynCipher.AST;

namespace Confuser.DynCipher.Generation;

public class DMCodeGen
{
	private readonly DynamicMethod dm;

	private readonly ILGenerator ilGen;

	private readonly Dictionary<string, LocalBuilder> localMap = new Dictionary<string, LocalBuilder>();

	private readonly Dictionary<string, int> paramMap;

	public DMCodeGen(Type returnType, Tuple<string, Type>[] parameters)
	{
		dm = new DynamicMethod("", returnType, parameters.Select((Tuple<string, Type> param) => param.Item2).ToArray(), restrictedSkipVisibility: true);
		paramMap = new Dictionary<string, int>();
		for (int i = 0; i < parameters.Length; i++)
		{
			paramMap.Add(parameters[i].Item1, i);
		}
		ilGen = dm.GetILGenerator();
	}

	protected virtual LocalBuilder Var(Variable var)
	{
		if (!localMap.TryGetValue(var.Name, out var value))
		{
			value = ilGen.DeclareLocal(typeof(int));
			localMap[var.Name] = value;
		}
		return value;
	}

	protected virtual void LoadVar(Variable var)
	{
		if (paramMap.ContainsKey(var.Name))
		{
			ilGen.Emit(OpCodes.Ldarg, paramMap[var.Name]);
		}
		else
		{
			ilGen.Emit(OpCodes.Ldloc, Var(var));
		}
	}

	protected virtual void StoreVar(Variable var)
	{
		if (paramMap.ContainsKey(var.Name))
		{
			ilGen.Emit(OpCodes.Starg, paramMap[var.Name]);
		}
		else
		{
			ilGen.Emit(OpCodes.Stloc, Var(var));
		}
	}

	public T Compile<T>()
	{
		ilGen.Emit(OpCodes.Ret);
		return (T)(object)dm.CreateDelegate(typeof(T));
	}

	public DMCodeGen GenerateCIL(Expression expression)
	{
		EmitLoad(expression);
		return this;
	}

	public DMCodeGen GenerateCIL(Statement statement)
	{
		EmitStatement(statement);
		return this;
	}

	private void EmitLoad(Expression exp)
	{
		if (exp is ArrayIndexExpression)
		{
			ArrayIndexExpression arrayIndexExpression = (ArrayIndexExpression)exp;
			EmitLoad(arrayIndexExpression.Array);
			ilGen.Emit(OpCodes.Ldc_I4, arrayIndexExpression.Index);
			ilGen.Emit(OpCodes.Ldelem_U4);
		}
		else if (exp is BinOpExpression)
		{
			BinOpExpression binOpExpression = (BinOpExpression)exp;
			EmitLoad(binOpExpression.Left);
			EmitLoad(binOpExpression.Right);
			OpCode opcode = binOpExpression.Operation switch
			{
				(BinOps)0 => OpCodes.Add, 
				BinOps.Sub => OpCodes.Sub, 
				(BinOps)2 => OpCodes.Div, 
				BinOps.Mul => OpCodes.Mul, 
				(BinOps)4 => OpCodes.Or, 
				BinOps.And => OpCodes.And, 
				(BinOps)6 => OpCodes.Xor, 
				BinOps.Lsh => OpCodes.Shl, 
				(BinOps)8 => OpCodes.Shr_Un, 
				_ => throw new NotSupportedException(), 
			};
			ilGen.Emit(opcode);
		}
		else if (exp is UnaryOpExpression)
		{
			UnaryOpExpression unaryOpExpression = (UnaryOpExpression)exp;
			EmitLoad(unaryOpExpression.Value);
			OpCode opcode2 = unaryOpExpression.Operation switch
			{
				UnaryOps.Negate => OpCodes.Neg, 
				(UnaryOps)0 => OpCodes.Not, 
				_ => throw new NotSupportedException(), 
			};
			ilGen.Emit(opcode2);
		}
		else if (exp is LiteralExpression)
		{
			LiteralExpression literalExpression = (LiteralExpression)exp;
			ilGen.Emit(OpCodes.Ldc_I4, (int)literalExpression.Value);
		}
		else
		{
			if (!(exp is VariableExpression))
			{
				throw new NotSupportedException();
			}
			VariableExpression variableExpression = (VariableExpression)exp;
			LoadVar(variableExpression.Variable);
		}
	}

	private void EmitStore(Expression exp, Expression value)
	{
		if (exp is ArrayIndexExpression)
		{
			ArrayIndexExpression arrayIndexExpression = (ArrayIndexExpression)exp;
			EmitLoad(arrayIndexExpression.Array);
			ilGen.Emit(OpCodes.Ldc_I4, arrayIndexExpression.Index);
			EmitLoad(value);
			ilGen.Emit(OpCodes.Stelem_I4);
			return;
		}
		if (exp is VariableExpression)
		{
			VariableExpression variableExpression = (VariableExpression)exp;
			EmitLoad(value);
			StoreVar(variableExpression.Variable);
			return;
		}
		throw new NotSupportedException();
	}

	private void EmitStatement(Statement statement)
	{
		if (statement is AssignmentStatement)
		{
			AssignmentStatement assignmentStatement = (AssignmentStatement)statement;
			EmitStore(assignmentStatement.Target, assignmentStatement.Value);
			return;
		}
		if (statement is LoopStatement)
		{
			LoopStatement loopStatement = (LoopStatement)statement;
			LocalBuilder local = ilGen.DeclareLocal(typeof(int));
			Label label = ilGen.DefineLabel();
			Label label2 = ilGen.DefineLabel();
			Label label3 = ilGen.DefineLabel();
			ilGen.Emit(OpCodes.Ldc_I4_0);
			ilGen.Emit(OpCodes.Stloc, local);
			ilGen.MarkLabel(label);
			ilGen.Emit(OpCodes.Ldloc, local);
			ilGen.Emit(OpCodes.Ldc_I4, loopStatement.Limit);
			ilGen.Emit(OpCodes.Blt, label2);
			ilGen.Emit(OpCodes.Br, label3);
			ilGen.MarkLabel(label2);
			foreach (Statement statement2 in loopStatement.Statements)
			{
				EmitStatement(statement2);
			}
			ilGen.Emit(OpCodes.Ldc_I4_1);
			ilGen.Emit(OpCodes.Ldloc, local);
			ilGen.Emit(OpCodes.Add);
			ilGen.Emit(OpCodes.Stloc, local);
			ilGen.Emit(OpCodes.Br, label);
			ilGen.MarkLabel(label3);
			ilGen.Emit(OpCodes.Nop);
			return;
		}
		if (statement is StatementBlock)
		{
			foreach (Statement statement3 in ((StatementBlock)statement).Statements)
			{
				EmitStatement(statement3);
			}
			return;
		}
		throw new NotSupportedException();
	}
}
