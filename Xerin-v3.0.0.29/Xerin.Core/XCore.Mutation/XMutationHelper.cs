using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace XCore.Mutation;

public class XMutationHelper : IDisposable
{
	private string m_mtFullName;

	public string MutationType
	{
		get
		{
			return m_mtFullName;
		}
		set
		{
			m_mtFullName = value;
		}
	}

	public XMutationHelper(string mtFullName)
	{
		m_mtFullName = mtFullName;
	}

	private static void SetInstrForInjectKey(Instruction instr, Type type, object value)
	{
		instr.OpCode = GetOpCode(type);
		instr.Operand = GetOperand(type, value);
	}

	private static OpCode GetOpCode(Type type)
	{
		return Type.GetTypeCode(type) switch
		{
			TypeCode.Boolean => OpCodes.Ldc_I4, 
			TypeCode.SByte => OpCodes.Ldc_I4_S, 
			TypeCode.Byte => OpCodes.Ldc_I4, 
			TypeCode.Int32 => OpCodes.Ldc_I4, 
			TypeCode.UInt32 => OpCodes.Ldc_I4, 
			TypeCode.Int64 => OpCodes.Ldc_I8, 
			TypeCode.UInt64 => OpCodes.Ldc_I8, 
			TypeCode.Single => OpCodes.Ldc_R4, 
			TypeCode.Double => OpCodes.Ldc_R8, 
			TypeCode.String => OpCodes.Ldstr, 
			_ => throw new SystemException("Unreachable code reached."), 
		};
	}

	private static object GetOperand(Type type, object value)
	{
		if (type == typeof(bool))
		{
			return ((bool)value) ? 1 : 0;
		}
		return value;
	}

	public void InjectKey<T>(MethodDef method, int keyId, T key)
	{
		if (string.IsNullOrWhiteSpace(m_mtFullName))
		{
			throw new ArgumentException();
		}
		IList<Instruction> instructions = method.Body.Instructions;
		int num = 0;
		while (true)
		{
			if (num >= instructions.Count)
			{
				return;
			}
			if (instructions[num].OpCode == OpCodes.Call && instructions[num].Operand is IMethod method2 && method2.DeclaringType.FullName == m_mtFullName && method2.Name == "Key")
			{
				int ldcI4Value = method.Body.Instructions[num - 1].GetLdcI4Value();
				if (ldcI4Value == keyId)
				{
					if (!typeof(T).IsAssignableFrom(Type.GetType(method2.FullName.Split(' ')[0])))
					{
						break;
					}
					method.Body.Instructions.RemoveAt(num);
					SetInstrForInjectKey(instructions[num - 1], typeof(T), key);
				}
			}
			num++;
		}
		throw new ArgumentException("The specified type does not match the type to be injected.");
	}

	public void InjectKeys<T>(MethodDef method, int[] keyIds, T[] keys)
	{
		if (string.IsNullOrWhiteSpace(m_mtFullName))
		{
			throw new ArgumentException();
		}
		IList<Instruction> instructions = method.Body.Instructions;
		int num = 0;
		while (true)
		{
			if (num >= instructions.Count)
			{
				return;
			}
			if (instructions[num].OpCode == OpCodes.Call && instructions[num].Operand is IMethod method2 && method2.DeclaringType.FullName == m_mtFullName && method2.Name == "Key")
			{
				int ldcI4Value = method.Body.Instructions[num - 1].GetLdcI4Value();
				if (ldcI4Value == 0 || Array.IndexOf(keyIds, ldcI4Value) != -1)
				{
					if (!typeof(T).IsAssignableFrom(Type.GetType(method2.FullName.Split(' ')[0])))
					{
						break;
					}
					method.Body.Instructions.RemoveAt(num);
					SetInstrForInjectKey(instructions[num - 1], typeof(T), keys[ldcI4Value]);
				}
			}
			num++;
		}
		throw new ArgumentException("The specified type does not match the type to be injected.");
	}

	public bool GetInstrLocationIndex(MethodDef method, bool removeCall, out int index)
	{
		if (string.IsNullOrWhiteSpace(m_mtFullName))
		{
			throw new ArgumentException();
		}
		int num = 0;
		while (true)
		{
			if (num < method.Body.Instructions.Count)
			{
				Instruction instruction = method.Body.Instructions[num];
				if (instruction.OpCode == OpCodes.Call)
				{
					IMethod method2 = instruction.Operand as IMethod;
					if (method2.DeclaringType.FullName == m_mtFullName && method2.Name == "LocationIndex")
					{
						break;
					}
				}
				num++;
				continue;
			}
			index = -1;
			return false;
		}
		index = num;
		if (removeCall)
		{
			method.Body.Instructions.RemoveAt(num);
		}
		return true;
	}

	public void Dispose()
	{
		m_mtFullName = null;
		GC.SuppressFinalize(this);
	}
}
