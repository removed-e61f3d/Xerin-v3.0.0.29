using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Generator;

namespace XCore.Utils;

public static class Utils
{
	public static Random rnd = new Random();

	public static int Complexity = 100;

	public static string b_d(this string base64EncodedData)
	{
		byte[] bytes = Convert.FromBase64String(base64EncodedData);
		return Encoding.UTF8.GetString(bytes);
	}

	public static string e_e(this string plainText)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		return Convert.ToBase64String(bytes);
	}

	public static bool isAssemblyDotNet(string assemblyPath)
	{
		try
		{
			AssemblyName.GetAssemblyName(assemblyPath);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static byte[] RandomByteArr(int size)
	{
		byte[] array = new byte[size];
		rnd.NextBytes(array);
		return array;
	}

	public static bool OVMAnalyzer(MethodDef method)
	{
		if (!method.HasBody)
		{
			return false;
		}
		if (method.HasGenericParameters)
		{
			return false;
		}
		if (method.IsPinvokeImpl)
		{
			return false;
		}
		if (method.IsUnmanagedExport)
		{
			return false;
		}
		return true;
	}

	public static Code GetCode(bool supported = false)
	{
		Code[] array = new Code[5]
		{
			Code.Add,
			Code.And,
			Code.Xor,
			Code.Sub,
			Code.Or
		};
		if (supported)
		{
			array = new Code[3]
			{
				Code.Add,
				Code.Sub,
				Code.Xor
			};
		}
		return array[rnd.Next(0, array.Length)];
	}

	public static FieldDefUser CreateField(FieldSig sig)
	{
		return new FieldDefUser(MethodsRenamig(), sig, dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
	}

	public static MethodDefUser CreateMethod(ModuleDef mod)
	{
		MethodDefUser methodDefUser = new MethodDefUser(MethodsRenamig(), MethodSig.CreateStatic(mod.CorLibTypes.Void), dnlib.DotNet.MethodImplAttributes.IL, dnlib.DotNet.MethodAttributes.Public | dnlib.DotNet.MethodAttributes.Static)
		{
			Body = new CilBody()
		};
		methodDefUser.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
		mod.GlobalType.Methods.Add(methodDefUser);
		return methodDefUser;
	}

	public static MethodDefUser CreateMethod(ModuleDef mod, int num, string content)
	{
		MethodDefUser methodDefUser = null;
		for (int i = 0; i < num; i++)
		{
			methodDefUser = new MethodDefUser(MethodsRenamig(), MethodSig.CreateStatic(mod.CorLibTypes.Void), dnlib.DotNet.MethodImplAttributes.IL, dnlib.DotNet.MethodAttributes.Public | dnlib.DotNet.MethodAttributes.Static | dnlib.DotNet.MethodAttributes.HideBySig)
			{
				Body = new CilBody()
			};
			methodDefUser.Body.Instructions.Add(OpCodes.Ldstr.ToInstruction(content));
			methodDefUser.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
			mod.GlobalType.Methods.Add(methodDefUser);
		}
		return methodDefUser;
	}

	public static int GetBasicRandomInt32()
	{
		RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		byte[] array = new byte[4];
		rNGCryptoServiceProvider.GetBytes(array);
		int num = BitConverter.ToInt32(array, 0);
		if (num < 0)
		{
			num *= -1;
		}
		return num;
	}

	public static int GetRandomInt32(int min, int max)
	{
		return GetRandomInt32() % (max - min + 1) + min;
	}

	public static int GetRandomInt32()
	{
		List<int[]> list = new List<int[]>();
		for (int i = 0; i < Complexity; i++)
		{
			int[] array = new int[Complexity];
			for (int j = 0; j < Complexity; j++)
			{
				array[j] = GetBasicRandomInt32();
			}
			list.Add(array);
		}
		return list[GetBasicRandomInt32() % Complexity][GetBasicRandomInt32() % Complexity];
	}

	public static int RandomTinyInt32()
	{
		return rnd.Next(2, 25);
	}

	public static int RandomSmallInt32()
	{
		return rnd.Next(15, 40);
	}

	public static int RandomInt32()
	{
		return rnd.Next(100, 300);
	}

	public static int RandomInt322()
	{
		return rnd.Next(10000, 100000);
	}

	public static int RandomBigInt32()
	{
		return rnd.Next();
	}

	public static bool RandomBoolean()
	{
		return Convert.ToBoolean(rnd.Next(0, 2));
	}

	public static void MethodRenamig(MethodDef m)
	{
		m.Name = GGeneration.GenerateGuidStartingWithLetter();
	}

	public static void MethodRenamig(MethodDef[] methods)
	{
		foreach (MethodDef methodDef in methods)
		{
			methodDef.Name = GGeneration.GenerateGuidStartingWithLetter();
		}
	}

	public static void MethodsRenamig(IDnlibDef mem)
	{
		mem.Name = GGeneration.GenerateGuidStartingWithLetter();
	}

	public static string MethodsRenamig()
	{
		return GGeneration.GenerateGuidStartingWithLetter();
	}

	public static string RandomString(int length)
	{
		return new string((from s in Enumerable.Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", length)
			select s[rnd.Next(s.Length)]).ToArray());
	}
}
