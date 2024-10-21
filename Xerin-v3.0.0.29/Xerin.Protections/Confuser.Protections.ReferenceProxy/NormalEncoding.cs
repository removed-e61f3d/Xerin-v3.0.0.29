using System;
using System.Collections.Generic;
using Confuser.DynCipher;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Generator;

namespace Confuser.Protections.ReferenceProxy;

internal class NormalEncoding : IRPEncoding
{
	private readonly Dictionary<MethodDef, Tuple<int, int>> keys = new Dictionary<MethodDef, Tuple<int, int>>();

	public Instruction[] EmitDecode(MethodDef init, RPContext ctx, Instruction[] arg)
	{
		Tuple<int, int> key = GetKey(ctx.Random, init);
		List<Instruction> list = new List<Instruction>();
		if (ctx.Random.NextBoolean())
		{
			list.Add(Instruction.Create(OpCodes.Ldc_I4, key.Item1));
			list.AddRange(arg);
		}
		else
		{
			list.AddRange(arg);
			list.Add(Instruction.Create(OpCodes.Ldc_I4, key.Item1));
		}
		list.Add(Instruction.Create(OpCodes.Mul));
		return list.ToArray();
	}

	public int Encode(MethodDef init, RPContext ctx, int value)
	{
		Tuple<int, int> key = GetKey(ctx.Random, init);
		return value * key.Item2;
	}

	private Tuple<int, int> GetKey(RandomGenerator random, MethodDef init)
	{
		if (!keys.TryGetValue(init, out var value))
		{
			int num = random.NextInt32() | 1;
			value = (keys[init] = Tuple.Create(num, (int)MathsUtils.modInv((uint)num)));
		}
		return value;
	}
}
