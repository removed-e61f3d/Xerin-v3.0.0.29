using System;
using System.Reflection;

namespace XVM.Runtime.Data;

internal class VMFuncSig
{
	private Module module;

	private readonly int[] paramToks;

	private readonly int retTok;

	private Type[] paramTypes;

	private Type retType;

	public byte Flags;

	public Type[] ParamTypes
	{
		get
		{
			if (paramTypes != null)
			{
				return paramTypes;
			}
			Type[] array = new Type[paramToks.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = module.ResolveType(paramToks[i]);
			}
			paramTypes = array;
			return array;
		}
	}

	public Type RetType => retType ?? (retType = module.ResolveType(retTok));

	public unsafe VMFuncSig(ref byte* ptr, Module module)
	{
		this.module = module;
		Flags = *(ptr++);
		paramToks = new int[Utils.ReadCompressedUInt(ref ptr)];
		for (int i = 0; i < paramToks.Length; i++)
		{
			paramToks[i] = (int)Utils.FromCodedToken(Utils.ReadCompressedUInt(ref ptr));
		}
		retTok = (int)Utils.FromCodedToken(Utils.ReadCompressedUInt(ref ptr));
	}
}
