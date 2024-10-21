using System;
using System.Reflection;
using System.Runtime.InteropServices;
using XVM.Runtime.Execution.Internal;

namespace XVM.Runtime.Execution;

[StructLayout(LayoutKind.Explicit)]
public struct VMSlot
{
	[FieldOffset(0)]
	private ulong u8;

	[FieldOffset(0)]
	private double r8;

	[FieldOffset(0)]
	private uint u4;

	[FieldOffset(0)]
	private float r4;

	[FieldOffset(0)]
	private ushort u2;

	[FieldOffset(0)]
	private byte u1;

	[FieldOffset(8)]
	private object o;

	public static readonly VMSlot Null;

	public ulong U8
	{
		get
		{
			return u8;
		}
		set
		{
			u8 = value;
			o = null;
		}
	}

	public uint U4
	{
		get
		{
			return u4;
		}
		set
		{
			u4 = value;
			o = null;
		}
	}

	public ushort U2
	{
		get
		{
			return u2;
		}
		set
		{
			u2 = value;
			o = null;
		}
	}

	public byte U1
	{
		get
		{
			return u1;
		}
		set
		{
			u1 = value;
			o = null;
		}
	}

	public double R8
	{
		get
		{
			return r8;
		}
		set
		{
			r8 = value;
			o = null;
		}
	}

	public float R4
	{
		get
		{
			return r4;
		}
		set
		{
			r4 = value;
			o = null;
		}
	}

	public object O
	{
		get
		{
			return o;
		}
		set
		{
			o = value;
			u8 = 0uL;
		}
	}

	public unsafe static VMSlot FromObject(object obj, Type type)
	{
		if (type.IsEnum)
		{
			Type underlyingType = Enum.GetUnderlyingType(type);
			return FromObject(Convert.ChangeType(obj, underlyingType), underlyingType);
		}
		switch (Type.GetTypeCode(type))
		{
		case TypeCode.Byte:
		{
			VMSlot result = default(VMSlot);
			result.u1 = (byte)obj;
			return result;
		}
		case TypeCode.SByte:
		{
			VMSlot result = default(VMSlot);
			result.u1 = (byte)(sbyte)obj;
			return result;
		}
		case TypeCode.Boolean:
		{
			VMSlot result = default(VMSlot);
			result.u1 = (byte)(((bool)obj) ? 1u : 0u);
			return result;
		}
		case TypeCode.UInt16:
		{
			VMSlot result = default(VMSlot);
			result.u2 = (ushort)obj;
			return result;
		}
		case TypeCode.Int16:
		{
			VMSlot result = default(VMSlot);
			result.u2 = (ushort)(short)obj;
			return result;
		}
		case TypeCode.Char:
		{
			VMSlot result = default(VMSlot);
			result.u2 = (char)obj;
			return result;
		}
		case TypeCode.UInt32:
		{
			VMSlot result = default(VMSlot);
			result.u4 = (uint)obj;
			return result;
		}
		case TypeCode.Int32:
		{
			VMSlot result = default(VMSlot);
			result.u4 = (uint)(int)obj;
			return result;
		}
		case TypeCode.UInt64:
		{
			VMSlot result = default(VMSlot);
			result.u8 = (ulong)obj;
			return result;
		}
		case TypeCode.Int64:
		{
			VMSlot result = default(VMSlot);
			result.u8 = (ulong)(long)obj;
			return result;
		}
		case TypeCode.Single:
		{
			VMSlot result = default(VMSlot);
			result.r4 = (float)obj;
			return result;
		}
		case TypeCode.Double:
		{
			VMSlot result = default(VMSlot);
			result.r8 = (double)obj;
			return result;
		}
		default:
		{
			VMSlot result;
			if (obj is Pointer)
			{
				result = default(VMSlot);
				result.u8 = (ulong)Pointer.Unbox(obj);
				return result;
			}
			if (obj is IntPtr)
			{
				result = default(VMSlot);
				result.u8 = (ulong)(long)(IntPtr)obj;
				return result;
			}
			if (obj is UIntPtr)
			{
				result = default(VMSlot);
				result.u8 = (ulong)(UIntPtr)obj;
				return result;
			}
			if (type.IsValueType)
			{
				result = default(VMSlot);
				result.o = ValueTypeBox.Box(obj, type);
				return result;
			}
			result = default(VMSlot);
			result.o = obj;
			return result;
		}
		}
	}

	public unsafe void ToTypedReferencePrimitive(TypedRefPtr typedRef)
	{
		*(TypedReference*)(void*)typedRef = __makeref(u4);
	}

	public unsafe void ToTypedReferenceObject(TypedRefPtr typedRef, Type type)
	{
		if (o is ValueType && type.IsValueType)
		{
			TypedReferenceHelpers.UnboxTypedRef(o, typedRef);
		}
		else
		{
			*(TypedReference*)(void*)typedRef = __makeref(o);
		}
	}

	public unsafe object ToObject(Type type)
	{
		if (type.IsEnum)
		{
			return Enum.ToObject(type, ToObject(Enum.GetUnderlyingType(type)));
		}
		switch (Type.GetTypeCode(type))
		{
		case TypeCode.Byte:
			return u1;
		case TypeCode.SByte:
			return (sbyte)u1;
		case TypeCode.Boolean:
			return u1 != 0;
		case TypeCode.UInt16:
			return u2;
		case TypeCode.Int16:
			return (short)u2;
		case TypeCode.Char:
			return (char)u2;
		case TypeCode.UInt32:
			return u4;
		case TypeCode.Int32:
			return (int)u4;
		case TypeCode.UInt64:
			return u8;
		case TypeCode.Int64:
			return (long)u8;
		case TypeCode.Single:
			return r4;
		case TypeCode.Double:
			return r8;
		default:
			if (type.IsPointer)
			{
				return Pointer.Box((void*)u8, type);
			}
			if (type == typeof(IntPtr))
			{
				return (IntPtr.Size == 8) ? new IntPtr((long)u8) : new IntPtr((int)u4);
			}
			if (type == typeof(UIntPtr))
			{
				return (IntPtr.Size == 8) ? new UIntPtr(u8) : new UIntPtr(u4);
			}
			return ValueTypeBox.Unbox(o);
		}
	}
}
