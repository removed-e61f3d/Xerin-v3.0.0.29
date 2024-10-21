using XVM.Runtime.Execution;

namespace XVM.Runtime.OpCodes;

internal class SxByte : IOpCode
{
	public byte Code => VMInstance.STATIC_Instance.Data.Constants.OP_SX_BYTE;

	public void Run(VMContext ctx, out ExecutionState state)
	{
		uint u = ctx.Registers[ctx.Data.Constants.REG_SP].U4;
		VMSlot value = ctx.Stack[u];
		if ((value.U1 & 0x80u) != 0)
		{
			value.U4 = value.U1 | 0xFFFFFF00u;
		}
		ctx.Stack[u] = value;
		state = ExecutionState.Next;
	}
}
