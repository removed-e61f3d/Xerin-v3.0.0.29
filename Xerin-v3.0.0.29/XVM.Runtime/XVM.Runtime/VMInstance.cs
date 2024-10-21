#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using XVM.Runtime.Data;
using XVM.Runtime.Execution;

namespace XVM.Runtime;

internal class VMInstance
{
	[ThreadStatic]
	private static Dictionary<Module, VMInstance> instances;

	private static readonly object initLock = new object();

	private static Dictionary<Module, int> initialized = new Dictionary<Module, int>();

	public static VMInstance STATIC_Instance { get; private set; }

	public VMData Data { get; private set; }

	public VMInstance(VMData data)
	{
		Data = data;
	}

	public static VMInstance Instance(Module module)
	{
		if (instances == null)
		{
			instances = new Dictionary<Module, VMInstance>();
		}
		if (!instances.TryGetValue(module, out var value))
		{
			value = new VMInstance(VMData.Instance(module));
			instances[module] = value;
			lock (initLock)
			{
				if (!initialized.ContainsKey(module))
				{
					initialized.Add(module, initialized.Count);
				}
			}
		}
		STATIC_Instance = value;
		return value;
	}

	public unsafe object Run(uint id, object[] arguments)
	{
		VMExportInfo vMExportInfo = Data.LookupExport(id);
		return Run((ulong)vMExportInfo.CodeAddress, vMExportInfo.EntryKey, vMExportInfo.Signature, arguments);
	}

	private unsafe object Run(ulong codeAddr, uint key, VMFuncSig sig, object[] arguments)
	{
		Stack<VMContext> stack = new Stack<VMContext>();
		VMContext vMContext = new VMContext(this);
		if (arguments == null)
		{
			arguments = new object[0];
		}
		if (vMContext != null)
		{
			stack.Push(vMContext);
		}
		try
		{
			Debug.Assert(sig.ParamTypes.Length == arguments.Length);
			vMContext.Stack.SetTopPosition((uint)(arguments.Length + 1));
			for (uint num = 0u; num < arguments.Length; num++)
			{
				Type type = sig.ParamTypes[num];
				if (type.IsByRef)
				{
					vMContext.Stack[num + 1] = new VMSlot
					{
						O = arguments[num]
					};
				}
				else if (type.IsPointer)
				{
					vMContext.Stack[num + 1] = new VMSlot
					{
						U8 = (ulong)Pointer.Unbox(arguments[num])
					};
				}
				else
				{
					vMContext.Stack[num + 1] = VMSlot.FromObject(arguments[num], sig.ParamTypes[num]);
				}
			}
			vMContext.Stack[(uint)(arguments.Length + 1)] = new VMSlot
			{
				U8 = 1uL
			};
			vMContext.Registers[vMContext.Data.Constants.REG_K1] = new VMSlot
			{
				U4 = key
			};
			vMContext.Registers[vMContext.Data.Constants.REG_BP] = new VMSlot
			{
				U4 = 0u
			};
			vMContext.Registers[vMContext.Data.Constants.REG_SP] = new VMSlot
			{
				U4 = (uint)(arguments.Length + 1)
			};
			vMContext.Registers[vMContext.Data.Constants.REG_IP] = new VMSlot
			{
				U8 = codeAddr
			};
			VMDispatcher.Run(vMContext);
			Debug.Assert(vMContext.EHStack.Count == 0);
			object result = null;
			if (sig.RetType != typeof(void))
			{
				VMSlot vMSlot = vMContext.Registers[vMContext.Data.Constants.REG_R0];
				result = ((Type.GetTypeCode(sig.RetType) != TypeCode.String || vMSlot.O != null) ? vMSlot.ToObject(sig.RetType) : Data.LookupString(vMSlot.U4));
			}
			return result;
		}
		finally
		{
			vMContext.Stack.FreeAllLocalloc();
			if (stack.Count > 0)
			{
				vMContext = stack.Pop();
			}
		}
	}
}
