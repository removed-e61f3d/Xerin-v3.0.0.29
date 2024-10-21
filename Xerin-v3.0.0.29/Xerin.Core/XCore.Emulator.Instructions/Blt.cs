using dnlib.DotNet.Emit;

namespace XCore.Emulator.Instructions;

internal class Blt : EmuInstruction
{
	internal override OpCode OpCode => OpCodes.Blt;

	internal override void Emulate(EmuContext context, Instruction instr)
	{
		int num = (int)context.Stack.Pop();
		int num2 = (int)context.Stack.Pop();
		if (num2 < num)
		{
			context.InstructionPointer = context.Instructions.IndexOf((Instruction)instr.Operand);
		}
	}
}
