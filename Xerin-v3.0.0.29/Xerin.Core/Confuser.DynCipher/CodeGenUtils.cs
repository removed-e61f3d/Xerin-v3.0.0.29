using System.IO;
using Confuser.DynCipher.Generation;

namespace Confuser.DynCipher;

public static class CodeGenUtils
{
	public static byte[] AssembleCode(x86CodeGen codeGen, x86Register reg)
	{
		MemoryStream memoryStream = new MemoryStream();
		using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
		{
			binaryWriter.Write(new byte[2] { 137, 224 });
			binaryWriter.Write(new byte[1] { 83 });
			binaryWriter.Write(new byte[1] { 87 });
			binaryWriter.Write(new byte[1] { 86 });
			binaryWriter.Write(new byte[2] { 41, 224 });
			binaryWriter.Write(new byte[3] { 131, 248, 24 });
			binaryWriter.Write(new byte[2] { 116, 7 });
			binaryWriter.Write(new byte[4] { 139, 68, 36, 16 });
			binaryWriter.Write(new byte[1] { 80 });
			binaryWriter.Write(new byte[2] { 235, 1 });
			binaryWriter.Write(new byte[1] { 81 });
			foreach (x86Instruction instruction in codeGen.Instructions)
			{
				binaryWriter.Write(instruction.Assemble());
			}
			if (reg != 0)
			{
				binaryWriter.Write(x86Instruction.Create((x86OpCode)0, new x86RegisterOperand((x86Register)0), new x86RegisterOperand(reg)).Assemble());
			}
			binaryWriter.Write(new byte[1] { 94 });
			binaryWriter.Write(new byte[1] { 95 });
			binaryWriter.Write(new byte[1] { 91 });
			binaryWriter.Write(new byte[1] { 195 });
		}
		return memoryStream.ToArray();
	}
}
