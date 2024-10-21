using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet.Emit;

namespace XCore.Emulator;

public class XEmulator
{
	public EmuContext _context;

	private Dictionary<OpCode, EmuInstruction> _emuInstructions;

	public XEmulator(List<Instruction> instructions, List<Local> locals)
	{
		_context = new EmuContext(instructions, locals);
		_emuInstructions = new Dictionary<OpCode, EmuInstruction>();
		List<EmuInstruction> list = (from t in typeof(EmuInstruction).Assembly.GetTypes()
			where t.IsSubclassOf(typeof(EmuInstruction)) && !t.IsAbstract
			select (EmuInstruction)Activator.CreateInstance(t)).ToList();
		foreach (EmuInstruction item in list)
		{
			_emuInstructions.Add(item.OpCode, item);
		}
	}

	internal int Emulate()
	{
		for (int i = _context.InstructionPointer; i < _context.Instructions.Count; i++)
		{
			Instruction instruction = _context.Instructions[i];
			if (instruction.OpCode == OpCodes.Stloc)
			{
				break;
			}
			if (instruction.OpCode != OpCodes.Nop)
			{
				_emuInstructions[instruction.OpCode].Emulate(_context, instruction);
			}
		}
		return (int)_context.Stack.Pop();
	}
}
