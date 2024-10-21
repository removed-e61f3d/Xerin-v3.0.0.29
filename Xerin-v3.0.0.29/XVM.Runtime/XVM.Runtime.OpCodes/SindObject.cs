using System;
using XVM.Runtime.Execution;

namespace XVM.Runtime.OpCodes;

internal class SindObject : IOpCode
{
	public byte Code => VMInstance.STATIC_Instance.Data.Constants.OP_SIND_OBJECT;

	public void Run(VMContext ctx, out ExecutionState state)
	{
		uint u = ctx.Registers[ctx.Data.Constants.REG_SP].U4;
		VMSlot vMSlot = ctx.Stack[u--];
		VMSlot slot = ctx.Stack[u--];
		ctx.Stack.SetTopPosition(u);
		ctx.Registers[ctx.Data.Constants.REG_SP].U4 = u;
		if (vMSlot.O is IReference)
		{
			((IReference)vMSlot.O).SetValue(ctx, slot, PointerType.OBJECT);
			state = ExecutionState.Next;
			return;
		}
		throw new ExecutionEngineException();
	}
}
