using System;
using System.Reflection;

namespace XVM.Runtime;

public class VMEntry
{
	public object Run(object[] arguments, RuntimeTypeHandle type, bool x, uint id)
	{
		Module module = typeof(VMEntry).Module;
		Module module2 = Type.GetTypeFromHandle(type)?.Module;
		if (module2 != null)
		{
			return VMInstance.Instance(module2).Run(id, arguments);
		}
		return null;
	}
}
