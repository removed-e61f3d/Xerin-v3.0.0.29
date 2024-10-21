using System;
using System.Collections.Generic;
using Confuser.DynCipher.AST;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Confuser.DynCipher.Generation;

public class CILCodeGen
{
	private readonly Dictionary<string, Local> localMap = new Dictionary<string, Local>();

	public MethodDef Method { get; private set; }

	public IList<Instruction> Instructions { get; private set; }

	public CILCodeGen(MethodDef method, IList<Instruction> instrs)
	{
		Method = method;
		Instructions = instrs;
	}

	protected void Emit(Instruction instr)
	{
		Instructions.Add(instr);
	}

	protected virtual Local Var(Variable var)
	{
		if (!localMap.TryGetValue(var.Name, out var value))
		{
			value = new Local(Method.Module.CorLibTypes.UInt32);
			value.Name = var.Name;
			localMap[var.Name] = value;
		}
		return value;
	}

	protected virtual void LoadVar(Variable var)
	{
		Emit(Instruction.Create(OpCodes.Ldloc, Var(var)));
	}

	protected virtual void StoreVar(Variable var)
	{
		Emit(Instruction.Create(OpCodes.Stloc, Var(var)));
	}

	public void Commit(CilBody body)
	{
		foreach (Local value in localMap.Values)
		{
			body.InitLocals = true;
			body.Variables.Add(value);
		}
	}

	public void GenerateCIL(Expression expression)
	{
		EmitLoad(expression);
	}

	public void GenerateCIL(Statement statement)
	{
		EmitStatement(statement);
	}

	private void EmitLoad(Expression exp)
	{
		if (exp is ArrayIndexExpression)
		{
			ArrayIndexExpression arrayIndexExpression = (ArrayIndexExpression)exp;
			EmitLoad(arrayIndexExpression.Array);
			Emit(Instruction.CreateLdcI4(arrayIndexExpression.Index));
			Emit(Instruction.Create(OpCodes.Ldelem_U4));
		}
		else if (exp is BinOpExpression)
		{
			BinOpExpression binOpExpression = (BinOpExpression)exp;
			EmitLoad(binOpExpression.Left);
			EmitLoad(binOpExpression.Right);
			Emit(Instruction.Create(binOpExpression.Operation switch
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
			}));
		}
		else if (exp is UnaryOpExpression)
		{
			UnaryOpExpression unaryOpExpression = (UnaryOpExpression)exp;
			EmitLoad(unaryOpExpression.Value);
			Emit(Instruction.Create(unaryOpExpression.Operation switch
			{
				UnaryOps.Negate => OpCodes.Neg, 
				(UnaryOps)0 => OpCodes.Not, 
				_ => throw new NotSupportedException(), 
			}));
		}
		else if (exp is LiteralExpression)
		{
			LiteralExpression literalExpression = (LiteralExpression)exp;
			Emit(Instruction.CreateLdcI4((int)literalExpression.Value));
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
			Emit(Instruction.CreateLdcI4(arrayIndexExpression.Index));
			EmitLoad(value);
			Emit(Instruction.Create(OpCodes.Stelem_I4));
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
			Local local = new Local(Method.Module.CorLibTypes.Int32);
			Method.Body.Variables.Add(local);
			Instruction instruction = OpCodes.Ldloc.ToInstruction(local);
			Instruction instruction2 = OpCodes.Nop.ToInstruction();
			Instruction instruction3 = OpCodes.Nop.ToInstruction();
			Emit(OpCodes.Ldc_I4_0.ToInstruction());
			Emit(OpCodes.Stloc.ToInstruction(local));
			Emit(instruction);
			Emit(Instruction.CreateLdcI4(loopStatement.Limit));
			Emit(OpCodes.Blt.ToInstruction(instruction3));
			Emit(OpCodes.Br.ToInstruction(instruction2));
			Emit(instruction3);
			foreach (Statement statement2 in loopStatement.Statements)
			{
				EmitStatement(statement2);
			}
			Emit(OpCodes.Ldc_I4_1.ToInstruction());
			Emit(OpCodes.Ldloc.ToInstruction(local));
			Emit(OpCodes.Add.ToInstruction());
			Emit(OpCodes.Stloc.ToInstruction(local));
			Emit(OpCodes.Br.ToInstruction(instruction));
			Emit(instruction2);
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
