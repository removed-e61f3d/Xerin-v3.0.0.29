using XVM.Runtime.Execution;

namespace XVM.Runtime.OpCodes;

internal class Swt : IOpCode
{
	public byte Code => VMInstance.STATIC_Instance.Data.Constants.OP_SWT;

	public unsafe void Run(VMContext ctx, out ExecutionState state)
	{
		uint u = ctx.Registers[ctx.Data.Constants.REG_SP].U4;
		VMSlot vMSlot = ctx.Stack[u];
		VMSlot vMSlot2 = ctx.Stack[u - 1];
		u -= 2;
		ctx.Stack.SetTopPosition(u);
		ctx.Registers[ctx.Data.Constants.REG_SP].U4 = u;
		uint u2 = vMSlot2.U4;
		ushort num = *(ushort*)(vMSlot.U8 - 2);
		if (u2 < num)
		{
			ctx.Registers[ctx.Data.Constants.REG_IP].U8 += (ulong)(*(int*)((nint)vMSlot.U8 + (nint)((long)u2 * 4L)));
		}
		state = ExecutionState.Next;
	}
}
