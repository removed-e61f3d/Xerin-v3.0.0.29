#define DEBUG
using System.Diagnostics;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.VM;
using KoiVM.Core.VMIL;

namespace KoiVM.Core.RT;

internal class BasicBlockChunk : IChunk
{
	private VMRuntime rt;

	private MethodDef method;

	public ILBlock Block { get; set; }

	public uint Length { get; set; }

	public BasicBlockChunk(VMRuntime rt, MethodDef method, ILBlock block)
	{
		this.rt = rt;
		this.method = method;
		Block = block;
		Length = rt.Serializer.ComputeLength(block);
	}

	public void OnOffsetComputed(uint offset)
	{
		uint num = rt.Serializer.ComputeOffset(Block, offset);
		Debug.Assert(num - offset == Length);
	}

	public byte[] GetData()
	{
		MemoryStream memoryStream = new MemoryStream();
		rt.Serializer.WriteData(Block, new BinaryWriter(memoryStream));
		return Encrypt(memoryStream.ToArray());
	}

	private byte[] Encrypt(byte[] data)
	{
		VMBlockKey vMBlockKey = rt.Descriptor.Data.LookupInfo(method).BlockKeys[Block];
		byte b = vMBlockKey.EntryKey;
		ILInstruction iLInstruction = Block.Content[0];
		ILInstruction iLInstruction2 = Block.Content[Block.Content.Count - 1];
		foreach (ILInstruction item in Block.Content)
		{
			uint num = item.Offset - iLInstruction.Offset;
			uint num2 = num + rt.Serializer.ComputeLength(item);
			byte b2 = data[num];
			data[num] ^= b;
			b = (byte)(b * 7 + b2);
			byte? b3 = null;
			if (item.Annotation == InstrAnnotation.JUMP || item == iLInstruction2)
			{
				b3 = vMBlockKey.ExitKey;
			}
			else if (item.OpCode == ILOpCode.LEAVE)
			{
				ExceptionHandler exceptionHandler = ((EHInfo)item.Annotation).ExceptionHandler;
				if (exceptionHandler.HandlerType == ExceptionHandlerType.Finally)
				{
					b3 = vMBlockKey.ExitKey;
				}
			}
			else if (item.OpCode == ILOpCode.CALL)
			{
				InstrCallInfo instrCallInfo = (InstrCallInfo)item.Annotation;
				VMMethodInfo vMMethodInfo = rt.Descriptor.Data.LookupInfo((MethodDef)instrCallInfo.Method);
				b3 = vMMethodInfo.EntryKey;
			}
			if (b3.HasValue)
			{
				byte b4 = CalculateFixupByte(b3.Value, data, b, num + 1, num2);
				data[num + 1] = b4;
			}
			for (uint num3 = num + 1; num3 < num2; num3++)
			{
				byte b5 = data[num3];
				data[num3] ^= b;
				b = (byte)(b * 7 + b5);
			}
			if (b3.HasValue)
			{
				Debug.Assert(b == b3.Value);
			}
			if (item.OpCode == ILOpCode.CALL)
			{
				InstrCallInfo instrCallInfo2 = (InstrCallInfo)item.Annotation;
				VMMethodInfo vMMethodInfo2 = rt.Descriptor.Data.LookupInfo((MethodDef)instrCallInfo2.Method);
				b = vMMethodInfo2.ExitKey;
			}
		}
		return data;
	}

	private static byte CalculateFixupByte(byte target, byte[] data, uint currentKey, uint rangeStart, uint rangeEnd)
	{
		byte b = target;
		for (uint num = rangeEnd - 1; num > rangeStart; num--)
		{
			b = (byte)((b - data[num]) * 183);
		}
		return (byte)(b - (byte)(currentKey * 7));
	}
}
