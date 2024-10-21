using System;
using System.Collections.Generic;
using Confuser.DynCipher.AST;
using Confuser.DynCipher.Generation;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Confuser.Protections.ReferenceProxy;

internal class ExpressionEncoding : IRPEncoding
{
	private class CodeGen : CILCodeGen
	{
		private readonly Instruction[] arg;

		public CodeGen(Instruction[] arg, MethodDef method, IList<Instruction> instrs)
			: base(method, instrs)
		{
			this.arg = arg;
		}

		protected override void LoadVar(Variable var)
		{
			if (var.Name == "{RESULT}")
			{
				Instruction[] array = arg;
				foreach (Instruction instr in array)
				{
					Emit(instr);
				}
			}
			else
			{
				base.LoadVar(var);
			}
		}
	}

	private readonly Dictionary<MethodDef, Tuple<Expression, Func<int, int>>> keys = new Dictionary<MethodDef, Tuple<Expression, Func<int, int>>>();

	public Instruction[] EmitDecode(MethodDef init, RPContext ctx, Instruction[] arg)
	{
		Tuple<Expression, Func<int, int>> key = GetKey(ctx, init);
		List<Instruction> list = new List<Instruction>();
		new CodeGen(arg, ctx.Method, list).GenerateCIL(key.Item1);
		init.Body.MaxStack += (ushort)ctx.Depth;
		return list.ToArray();
	}

	public int Encode(MethodDef init, RPContext ctx, int value)
	{
		Tuple<Expression, Func<int, int>> key = GetKey(ctx, init);
		return key.Item2(value);
	}

	private void Compile(RPContext ctx, CilBody body, out Func<int, int> expCompiled, out Expression inverse)
	{
		Variable variable = new Variable("{VAR}");
		Variable variable2 = new Variable("{RESULT}");
		ctx.DynCipher.GenerateExpressionPair(ctx.Random, new VariableExpression
		{
			Variable = variable
		}, new VariableExpression
		{
			Variable = variable2
		}, ctx.Depth, out var expression, out inverse);
		expCompiled = new DMCodeGen(typeof(int), new Tuple<string, Type>[1] { Tuple.Create("{VAR}", typeof(int)) }).GenerateCIL(expression).Compile<Func<int, int>>();
	}

	private Tuple<Expression, Func<int, int>> GetKey(RPContext ctx, MethodDef init)
	{
		if (!keys.TryGetValue(init, out var value))
		{
			Compile(ctx, init.Body, out var expCompiled, out var inverse);
			value = (keys[init] = Tuple.Create(inverse, expCompiled));
		}
		return value;
	}
}
