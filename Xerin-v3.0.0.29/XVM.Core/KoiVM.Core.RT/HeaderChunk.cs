#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.MD;
using KoiVM.Core.AST.IL;
using KoiVM.Core.VM;

namespace KoiVM.Core.RT;

internal class HeaderChunk : IChunk
{
	private byte[] data;

	public uint Length { get; set; }

	public HeaderChunk(VMRuntime rt)
	{
		Length = ComputeLength(rt);
	}

	public void OnOffsetComputed(uint offset)
	{
	}

	public byte[] GetData()
	{
		return data;
	}

	private uint GetCodedLen(MDToken token)
	{
		switch (token.Table)
		{
		case Table.TypeRef:
		case Table.TypeDef:
		case Table.Field:
		case Table.Method:
		case Table.MemberRef:
		case Table.TypeSpec:
		case Table.MethodSpec:
			return Utils.GetCompressedUIntLength(token.Rid << 3);
		default:
			throw new NotSupportedException();
		}
	}

	private uint GetCodedToken(MDToken token)
	{
		return token.Table switch
		{
			Table.TypeDef => (token.Rid << 3) | 1u, 
			Table.TypeRef => (token.Rid << 3) | 2u, 
			Table.TypeSpec => (token.Rid << 3) | 3u, 
			Table.MemberRef => (token.Rid << 3) | 4u, 
			Table.Method => (token.Rid << 3) | 5u, 
			Table.Field => (token.Rid << 3) | 6u, 
			Table.MethodSpec => (token.Rid << 3) | 7u, 
			_ => throw new NotSupportedException(), 
		};
	}

	private uint ComputeLength(VMRuntime rt)
	{
		uint num = (uint)rt.Descriptor.Data.constantsMap.Length;
		foreach (KeyValuePair<string, uint> item in rt.Descriptor.Data.strMap)
		{
			num += Utils.GetCompressedUIntLength(item.Value);
			num += Utils.GetCompressedUIntLength((uint)item.Key.Length);
			num += (uint)(item.Key.Length * 2);
		}
		foreach (KeyValuePair<IMemberRef, uint> item2 in rt.Descriptor.Data.refMap)
		{
			num += Utils.GetCompressedUIntLength(item2.Value) + GetCodedLen(item2.Key.MDToken);
		}
		foreach (FuncSigDesc sig in rt.Descriptor.Data.sigs)
		{
			num += Utils.GetCompressedUIntLength(sig.Id);
			num += 4;
			if (sig.Method != null)
			{
				num += 4;
			}
			uint value = (uint)sig.FuncSig.ParamSigs.Length;
			num += 1 + Utils.GetCompressedUIntLength(value);
			ITypeDefOrRef[] paramSigs = sig.FuncSig.ParamSigs;
			foreach (ITypeDefOrRef typeDefOrRef in paramSigs)
			{
				num += GetCodedLen(typeDefOrRef.MDToken);
			}
			num += GetCodedLen(sig.FuncSig.RetType.MDToken);
		}
		return num;
	}

	internal void WriteData(VMRuntime rt)
	{
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		byte[] constantsMap = rt.Descriptor.Data.constantsMap;
		foreach (byte value in constantsMap)
		{
			binaryWriter.Write(value);
		}
		foreach (KeyValuePair<string, uint> item2 in rt.Descriptor.Data.strMap)
		{
			binaryWriter.WriteCompressedUInt(item2.Value);
			binaryWriter.WriteCompressedUInt((uint)item2.Key.Length);
			string key = item2.Key;
			foreach (char value2 in key)
			{
				binaryWriter.Write((ushort)value2);
			}
		}
		foreach (KeyValuePair<IMemberRef, uint> item3 in rt.Descriptor.Data.refMap)
		{
			binaryWriter.WriteCompressedUInt(item3.Value);
			binaryWriter.WriteCompressedUInt(GetCodedToken(item3.Key.MDToken));
		}
		foreach (FuncSigDesc sig in rt.Descriptor.Data.sigs)
		{
			binaryWriter.WriteCompressedUInt(sig.Id);
			if (sig.Method != null)
			{
				ILBlock item = rt.MethodMap[sig.Method].Item2;
				uint offset = item.Content[0].Offset;
				Debug.Assert(offset != 0);
				binaryWriter.Write(offset);
				uint num = rt.Descriptor.RandomGenerator.NextUInt32();
				num = (num << 8) | rt.Descriptor.Data.LookupInfo(sig.Method).EntryKey;
				binaryWriter.Write(num);
			}
			else
			{
				binaryWriter.Write(0u);
			}
			binaryWriter.Write(sig.FuncSig.Flags);
			binaryWriter.WriteCompressedUInt((uint)sig.FuncSig.ParamSigs.Length);
			ITypeDefOrRef[] paramSigs = sig.FuncSig.ParamSigs;
			foreach (ITypeDefOrRef typeDefOrRef in paramSigs)
			{
				binaryWriter.WriteCompressedUInt(GetCodedToken(typeDefOrRef.MDToken));
			}
			binaryWriter.WriteCompressedUInt(GetCodedToken(sig.FuncSig.RetType.MDToken));
		}
		data = memoryStream.ToArray();
		Debug.Assert(data.Length == Length);
	}
}
