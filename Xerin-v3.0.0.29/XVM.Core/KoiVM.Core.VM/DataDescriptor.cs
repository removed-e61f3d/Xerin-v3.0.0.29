using System;
using System.Collections.Generic;
using dnlib.DotNet;
using KoiVM.Core.Services;

namespace KoiVM.Core.VM;

public class DataDescriptor
{
	private readonly Dictionary<MethodDef, uint> exportMap = new Dictionary<MethodDef, uint>();

	private readonly Dictionary<MethodDef, VMMethodInfo> methodInfos = new Dictionary<MethodDef, VMMethodInfo>();

	private uint nextRefId;

	private uint nextStrId;

	private readonly RandomGenerator randomgen;

	internal byte[] constantsMap = new byte[0];

	internal Dictionary<IMemberRef, uint> refMap = new Dictionary<IMemberRef, uint>();

	internal List<FuncSigDesc> sigs = new List<FuncSigDesc>();

	internal Dictionary<string, uint> strMap = new Dictionary<string, uint>(StringComparer.Ordinal);

	internal DataDescriptor(RandomGenerator randomGenerator)
	{
		strMap[""] = 1u;
		nextStrId = 2u;
		nextRefId = 1u;
		randomgen = randomGenerator;
	}

	public uint GetId(IMemberRef memberRef)
	{
		if (!refMap.TryGetValue(memberRef, out var value))
		{
			value = (refMap[memberRef] = nextRefId++);
		}
		return value;
	}

	public void ReplaceReference(IMemberRef old, IMemberRef @new)
	{
		if (refMap.TryGetValue(old, out var value))
		{
			refMap.Remove(old);
			refMap[@new] = value;
		}
	}

	public uint GetId(string str)
	{
		if (!strMap.TryGetValue(str, out var value))
		{
			value = (strMap[str] = nextStrId++);
		}
		return value;
	}

	public uint GetExportId(MethodDef method)
	{
		if (!exportMap.TryGetValue(method, out var value))
		{
			byte[] array = Guid.NewGuid().ToByteArray();
			array[0] = (byte)(method.MDToken.ToInt32() >> 24);
			array[1] = (byte)(method.MDToken.ToInt32() >> 16);
			array[2] = (byte)(method.MDToken.ToInt32() >> 8);
			array[3] = (byte)method.MDToken.ToInt32();
			byte[] array2 = new Guid(array).ToByteArray();
			uint num = (uint)(array2[0] | (array2[1] << 8) | (array2[2] << 16) | (array2[3] << 24));
			value = (exportMap[method] = num);
			sigs.Add(new FuncSigDesc(num, method));
		}
		return value;
	}

	public VMMethodInfo LookupInfo(MethodDef method)
	{
		if (!methodInfos.TryGetValue(method, out var value))
		{
			byte b = randomgen.NextByte();
			value = new VMMethodInfo
			{
				EntryKey = b,
				ExitKey = (byte)(b >> 8)
			};
			methodInfos[method] = value;
		}
		return value;
	}

	public void SetInfo(MethodDef method, VMMethodInfo info)
	{
		methodInfos[method] = info;
	}
}
