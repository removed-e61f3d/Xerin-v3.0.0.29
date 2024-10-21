using dnlib.DotNet.Emit;

namespace XCore.Emulator.Instructions;

internal class Stloc : EmuInstruction
{
	internal override OpCode OpCode => OpCodes.Stloc;

	internal override void Emulate(EmuContext context, Instruction instr)
	{
		object val = context.Stack.Pop();
		context.SetLocalValue((Local)instr.Operand, val);
	}
}
