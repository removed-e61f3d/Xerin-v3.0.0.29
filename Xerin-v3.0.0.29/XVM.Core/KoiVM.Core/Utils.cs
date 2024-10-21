using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VM;

namespace KoiVM.Core;

public static class Utils
{
	public static ModuleWriterOptions ExecuteModuleWriterOptions;

	public static void AddListEntry<TKey, TValue>(this IDictionary<TKey, List<TValue>> self, TKey key, TValue value)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (!self.TryGetValue(key, out var value2))
		{
			List<TValue> list2 = (self[key] = new List<TValue>());
			value2 = list2;
		}
		value2.Add(value);
	}

	public static StrongNameKey LoadSNKey(string path, string pass)
	{
		if (path == null)
		{
			return null;
		}
		try
		{
			if (pass != null)
			{
				X509Certificate2 x509Certificate = new X509Certificate2();
				x509Certificate.Import(path, pass, X509KeyStorageFlags.Exportable);
				if (!(x509Certificate.PrivateKey is RSACryptoServiceProvider rSACryptoServiceProvider))
				{
					throw new ArgumentException("RSA key does not present in the certificate.", "path");
				}
				return new StrongNameKey(rSACryptoServiceProvider.ExportCspBlob(includePrivateParameters: true));
			}
			return new StrongNameKey(path);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static TValue GetValueOrDefaultLazy<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> defValueFactory)
	{
		if (dictionary.TryGetValue(key, out var value))
		{
			return value;
		}
		return defValueFactory(key);
	}

	public static void Increment<T>(this Dictionary<T, int> self, T key)
	{
		if (!self.TryGetValue(key, out var value))
		{
			value = 0;
		}
		value = (self[key] = value + 1);
	}

	public static void Replace<T>(this List<T> list, int index, IEnumerable<T> newItems)
	{
		list.RemoveAt(index);
		list.InsertRange(index, newItems);
	}

	public static void Replace(this List<IRInstruction> list, int index, IEnumerable<IRInstruction> newItems)
	{
		IRInstruction iRInstruction = list[index];
		list.RemoveAt(index);
		foreach (IRInstruction newItem in newItems)
		{
			newItem.ILAST = iRInstruction.ILAST;
		}
		list.InsertRange(index, newItems);
	}

	public static bool IsGPR(this VMRegisters reg)
	{
		if (reg >= VMRegisters.R0 && reg <= VMRegisters.R7)
		{
			return true;
		}
		return false;
	}

	public static uint GetCompressedUIntLength(uint value)
	{
		uint num = 0u;
		do
		{
			value >>= 7;
			num++;
		}
		while (value != 0);
		return num;
	}

	public static void WriteCompressedUInt(this BinaryWriter writer, uint value)
	{
		do
		{
			byte b = (byte)(value & 0x7Fu);
			value >>= 7;
			if (value != 0)
			{
				b = (byte)(b | 0x80u);
			}
			writer.Write(b);
		}
		while (value != 0);
	}

	public static TypeSig ResolveType(this GenericArguments genericArgs, TypeSig typeSig)
	{
		switch (typeSig.ElementType)
		{
		case ElementType.Ptr:
			return new PtrSig(genericArgs.ResolveType(typeSig.Next));
		case ElementType.ByRef:
			return new ByRefSig(genericArgs.ResolveType(typeSig.Next));
		case ElementType.SZArray:
			return new SZArraySig(genericArgs.ResolveType(typeSig.Next));
		case ElementType.Array:
		{
			ArraySig arraySig = (ArraySig)typeSig;
			return new ArraySig(genericArgs.ResolveType(typeSig.Next), arraySig.Rank, arraySig.Sizes, arraySig.LowerBounds);
		}
		case ElementType.Pinned:
			return new PinnedSig(genericArgs.ResolveType(typeSig.Next));
		case ElementType.Var:
		case ElementType.MVar:
			return genericArgs.Resolve(typeSig);
		case ElementType.GenericInst:
		{
			GenericInstSig genericInstSig = (GenericInstSig)typeSig;
			List<TypeSig> list = new List<TypeSig>();
			foreach (TypeSig genericArgument in genericInstSig.GenericArguments)
			{
				list.Add(genericArgs.ResolveType(genericArgument));
			}
			return new GenericInstSig(genericInstSig.GenericType, list);
		}
		case ElementType.CModReqd:
			return new CModReqdSig(((CModReqdSig)typeSig).Modifier, genericArgs.ResolveType(typeSig.Next));
		case ElementType.CModOpt:
			return new CModOptSig(((CModOptSig)typeSig).Modifier, genericArgs.ResolveType(typeSig.Next));
		case ElementType.ValueArray:
			return new ValueArraySig(genericArgs.ResolveType(typeSig.Next), ((ValueArraySig)typeSig).Size);
		case ElementType.Module:
			return new ModuleSig(((ModuleSig)typeSig).Index, genericArgs.ResolveType(typeSig.Next));
		default:
			if (typeSig.IsTypeDefOrRef)
			{
				TypeDefOrRefSig typeDefOrRefSig = (TypeDefOrRefSig)typeSig;
				if (typeDefOrRefSig.TypeDefOrRef is TypeSpec)
				{
					throw new NotSupportedException();
				}
			}
			return typeSig;
		}
	}
}
