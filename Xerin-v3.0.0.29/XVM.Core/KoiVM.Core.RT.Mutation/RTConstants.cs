using System.Collections.Generic;
using System.IO;
using KoiVM.Core.VM;
using KoiVM.Core.VMIL;

namespace KoiVM.Core.RT.Mutation;

public class RTConstants
{
	private readonly Dictionary<string, byte> Constants = new Dictionary<string, byte>();

	private void AddField(string fieldName, byte fieldValue)
	{
		Constants[fieldName] = fieldValue;
	}

	public void ReadConstants(VMDescriptor desc)
	{
		for (int i = 0; i < 13; i++)
		{
			VMRegisters reg = (VMRegisters)i;
			byte fieldValue = desc.Architecture.Registers[reg];
			string fieldName = reg.ToString();
			AddField(fieldName, fieldValue);
		}
		for (int j = 0; j < 5; j++)
		{
			VMFlags flag = (VMFlags)j;
			int num = desc.Architecture.Flags[flag];
			string fieldName2 = flag.ToString();
			AddField(fieldName2, (byte)(1 << num));
		}
		for (int k = 0; k < 68; k++)
		{
			ILOpCode opCode = (ILOpCode)k;
			byte fieldValue2 = desc.Architecture.OpCodes[opCode];
			string fieldName3 = opCode.ToString();
			AddField(fieldName3, fieldValue2);
		}
		for (int l = 0; l < 17; l++)
		{
			VMCalls call = (VMCalls)l;
			int num2 = desc.Runtime.VMCall[call];
			string fieldName4 = call.ToString();
			AddField(fieldName4, (byte)num2);
		}
		AddField(ConstantFields.E_CALL.ToString(), (byte)desc.Runtime.VCallOps.ECALL_CALL);
		AddField(ConstantFields.E_CALLVIRT.ToString(), (byte)desc.Runtime.VCallOps.ECALL_CALLVIRT);
		AddField(ConstantFields.E_NEWOBJ.ToString(), (byte)desc.Runtime.VCallOps.ECALL_NEWOBJ);
		AddField(ConstantFields.E_CALLVIRT_CONSTRAINED.ToString(), (byte)desc.Runtime.VCallOps.ECALL_CALLVIRT_CONSTRAINED);
		AddField(ConstantFields.CATCH.ToString(), desc.Runtime.RTFlags.EH_CATCH);
		AddField(ConstantFields.FILTER.ToString(), desc.Runtime.RTFlags.EH_FILTER);
		AddField(ConstantFields.FAULT.ToString(), desc.Runtime.RTFlags.EH_FAULT);
		AddField(ConstantFields.FINALLY.ToString(), desc.Runtime.RTFlags.EH_FINALLY);
		MemoryStream memoryStream = new MemoryStream();
		using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
		{
			List<byte> list = new List<byte>();
			list.AddRange(Constants.Values);
			byte[] array = list.ToArray();
			foreach (byte value in array)
			{
				binaryWriter.Write(value);
			}
		}
		desc.Data.constantsMap = memoryStream.ToArray();
	}
}
