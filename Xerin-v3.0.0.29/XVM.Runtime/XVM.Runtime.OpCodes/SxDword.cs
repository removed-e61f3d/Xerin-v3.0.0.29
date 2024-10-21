using XVM.Runtime.Execution;

namespace XVM.Runtime.OpCodes;

internal class SxDword : IOpCode
{
	public byte Code => VMInstance.STATIC_Instance.Data.Constants.OP_SX_DWORD;

	public void Run(VMContext ctx, out ExecutionState state)
	{
		uint u = ctx.Registers[ctx.Data.Constants.REG_SP].U4;
		VMSlot value = ctx.Stack[u];
		if ((value.U4 & 0x80000000u) != 0)
		{
			value.U8 = 0xFFFFFFFF00000000uL | value.U4;
		}
		ctx.Stack[u] = value;
		state = ExecutionState.Next;
	}
}
