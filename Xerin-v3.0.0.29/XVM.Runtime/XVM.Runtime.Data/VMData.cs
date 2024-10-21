using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using XVM.Runtime.Dynamic;

namespace XVM.Runtime.Data;

internal class VMData
{
	private Dictionary<uint, RefInfo> References;

	private Dictionary<uint, string> Strings;

	private Dictionary<uint, VMExportInfo> Exports;

	private static Dictionary<Module, VMData> ModuleVMData = new Dictionary<Module, VMData>();

	public Module Module { get; private set; }

	public unsafe VMDATA_HEADER* DATA_HEADER { get; private set; }

	public Constants Constants { get; private set; }

	public unsafe byte* KoiSection { get; private set; }

	public unsafe VMData(Module module)
	{
		References = new Dictionary<uint, RefInfo>();
		Strings = new Dictionary<uint, string>();
		Exports = new Dictionary<uint, VMExportInfo>();
		byte[] array = new byte[Mutation.IntKey0];
		RuntimeHelpers.InitializeArray(array, typeof(VMData).GetField(Mutation.LdstrKey0, BindingFlags.Static | BindingFlags.NonPublic).FieldHandle);
		byte* ptr = (byte*)(void*)Marshal.AllocHGlobal(array.Length);
		Marshal.Copy(array, 0, (IntPtr)ptr, array.Length);
		DATA_HEADER = (VMDATA_HEADER*)ptr;
		byte* ptr2 = (byte*)(DATA_HEADER + 1);
		for (int i = 0; i < Mutation.IntKey1; i++)
		{
			uint key = Utils.ReadCompressedUInt(ref ptr2);
			uint num = Utils.ReadCompressedUInt(ref ptr2);
			Strings[key] = new string((char*)ptr2, 0, (int)num);
			ptr2 += num << 1;
		}
		for (int j = 0; j < Mutation.IntKey2; j++)
		{
			uint key2 = Utils.ReadCompressedUInt(ref ptr2);
			int token = (int)Utils.FromCodedToken(Utils.ReadCompressedUInt(ref ptr2));
			References[key2] = new RefInfo
			{
				Module = module,
				Token = token
			};
		}
		for (int k = 0; k < Mutation.IntKey3; k++)
		{
			Exports[Utils.ReadCompressedUInt(ref ptr2)] = new VMExportInfo(ref ptr2, ptr, module);
		}
		KoiSection = ptr;
		Constants = DATA_HEADER->ToConstants();
		Module = module;
		ModuleVMData[module] = this;
	}

	public static VMData Instance(Module module)
	{
		VMData value;
		lock (ModuleVMData)
		{
			if (!ModuleVMData.TryGetValue(module, out value))
			{
				VMData vMData2 = (ModuleVMData[module] = new VMData(module));
				value = vMData2;
				return value;
			}
		}
		return value;
	}

	public MemberInfo LookupReference(uint id)
	{
		return References[id].Member();
	}

	public string LookupString(uint id)
	{
		if (id == 0)
		{
			return null;
		}
		return Strings[id];
	}

	public VMExportInfo LookupExport(uint id)
	{
		return Exports[id];
	}
}
