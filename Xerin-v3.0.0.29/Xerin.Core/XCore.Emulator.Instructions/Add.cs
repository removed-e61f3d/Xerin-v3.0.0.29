using dnlib.DotNet.Emit;

namespace XCore.Emulator.Instructions;

internal class Add : EmuInstruction
{
	internal override OpCode OpCode => OpCodes.Add;

	internal override void Emulate(EmuContext context, Instruction instr)
	{
		int num = (int)context.Stack.Pop();
		int num2 = (int)context.Stack.Pop();
		context.Stack.Push(num2 + num);
	}
}
