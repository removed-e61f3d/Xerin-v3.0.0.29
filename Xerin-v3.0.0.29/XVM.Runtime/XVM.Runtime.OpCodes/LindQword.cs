using XVM.Runtime.Execution;

namespace XVM.Runtime.OpCodes;

internal class LindQword : IOpCode
{
	public byte Code => VMInstance.STATIC_Instance.Data.Constants.OP_LIND_QWORD;

	public unsafe void Run(VMContext ctx, out ExecutionState state)
	{
		uint u = ctx.Registers[ctx.Data.Constants.REG_SP].U4;
		VMSlot vMSlot = ctx.Stack[u];
		VMSlot value;
		if (vMSlot.O is IReference)
		{
			value = ((IReference)vMSlot.O).GetValue(ctx, PointerType.QWORD);
		}
		else
		{
			ulong* ptr = (ulong*)vMSlot.U8;
			VMSlot vMSlot2 = default(VMSlot);
			vMSlot2.U8 = *ptr;
			value = vMSlot2;
		}
		ctx.Stack[u] = value;
		state = ExecutionState.Next;
	}
}
