using System;
using System.Collections.Generic;
using dnlib.DotNet.Emit;

namespace XCore.Emulator;

public class EmuContext
{
	internal Stack<object> Stack;

	internal List<Instruction> Instructions;

	internal int InstructionPointer = 0;

	public Dictionary<Local, object> _locals;

	public EmuContext(List<Instruction> instructions, List<Local> locals)
	{
		Stack = new Stack<object>();
		Instructions = instructions;
		_locals = new Dictionary<Local, object>();
		foreach (Local local in locals)
		{
			_locals.Add(local, null);
		}
	}

	internal object GetLocalValue(Local local)
	{
		Type type = Type.GetType(local.Type.AssemblyQualifiedName);
		return Convert.ChangeType(_locals[local], type);
	}

	internal void SetLocalValue(Local local, object val)
	{
		_locals[local] = val;
	}
}
