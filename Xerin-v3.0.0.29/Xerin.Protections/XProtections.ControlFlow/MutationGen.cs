using System.Collections.Generic;
using Confuser.DynCipher.AST;
using Confuser.DynCipher.Generation;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace XProtections.ControlFlow;

internal class MutationGen : CILCodeGen
{
	private readonly Local state;

	public MutationGen(Local state, MethodDef method, IList<Instruction> instrs)
		: base(method, instrs)
	{
		this.state = state;
	}

	protected override Local Var(Variable var)
	{
		if (var.Name == "{RESULT}")
		{
			return state;
		}
		return base.Var(var);
	}
}
