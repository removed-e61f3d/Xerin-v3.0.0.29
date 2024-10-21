using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace XRuntime;

public static class CodeEncryptionRuntime
{
	[DllImport("kernel32.dll")]
	private static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

	[DllImport("kernel32.dll")]
	private static extern void ExitProcess(uint uExitCode);

	public unsafe static void Initialize()
	{
		Module module = typeof(CodeEncryptionRuntime).Module;
		string fullyQualifiedName = module.FullyQualifiedName;
		bool flag = fullyQualifiedName.Length > 0 && fullyQualifiedName[0] == '<';
		byte* ptr = (byte*)(void*)Marshal.GetHINSTANCE(module);
		byte* ptr2 = ptr + (uint)(*(int*)(ptr + 60));
		ushort num = *(ushort*)(ptr2 + 6);
		ushort num2 = *(ushort*)(ptr2 + 20);
		uint* ptr3 = null;
		uint? num3 = 0u;
		uint* ptr4 = (uint*)(ptr2 + 24 + (int)num2);
		uint[] array = new uint[1] { (uint)XMutationClass.Key<int>(1) };
		uint[] array2 = new uint[1] { (uint)XMutationClass.Key<int>(2) };
		uint[] array3 = new uint[1] { (uint)XMutationClass.Key<int>(3) };
		uint[] array4 = new uint[1] { (uint)XMutationClass.Key<int>(4) };
		IntPtr intPtr = (IntPtr)(ptr2 + 24);
		VirtualProtect((IntPtr)(void*)intPtr, 1u, 64u, out var lpflOldProtect);
		VirtualProtect((IntPtr)(void*)intPtr, 1u, lpflOldProtect, out lpflOldProtect);
		uint[] array5 = new uint[1] { (uint)XMutationClass.Key<int>(0) };
		for (int i = 0; i < num; i++)
		{
			uint num4 = *(ptr4++) * *(ptr4++);
			if (num4 == array5[0])
			{
				ptr3 = (uint*)(ptr + (flag ? ptr4[3] : ptr4[1]));
				num3 = (flag ? ptr4[2] : (*ptr4)) >> 2;
			}
			else if (num4 != 0)
			{
				uint* ptr5 = (uint*)(ptr + (flag ? ptr4[3] : ptr4[1]));
				uint num5 = ptr4[2] >> 2;
				for (uint num6 = 0u; num6 < num5; num6++)
				{
					uint num7 = (array[0] ^ *(ptr5++)) + array2[0] + array3[0] * array4[0];
					array[0] = array2[0];
					array2[0] = array3[0];
					array2[0] = array4[0];
					array4[0] = num7;
				}
			}
			ptr4 += 8;
		}
		uint[] array6 = new uint[16];
		uint[] array7 = new uint[16];
		for (int j = 0; j < 16; j++)
		{
			array6[j] = array4[0];
			array7[j] = array2[0];
			array[0] = (array2[0] >> 5) | (array2[0] << 27);
			array2[0] = (array3[0] >> 3) | (array3[0] << 29);
			array3[0] = (array4[0] >> 7) | (array4[0] << 25);
			array4[0] = (array[0] >> 11) | (array[0] << 21);
			array4[0] = (array[0] >> 11) | (array[0] << 21);
		}
		XMutationClass.Crypt(array6, array7);
		uint lpflOldProtect2 = 64u;
		VirtualProtect((IntPtr)ptr3, num3.Value << 2, lpflOldProtect2, out lpflOldProtect2);
		if (lpflOldProtect2 == 64)
		{
			ExitProcess(1u);
			return;
		}
		uint num8 = 0u;
		for (uint num9 = 0u; num9 < num3; num9++)
		{
			*ptr3 ^= array6[num8 & 0xF];
			array6[num8 & 0xF] = (array6[num8 & 0xF] ^ *(ptr3++)) + 1035675673;
			num8++;
		}
	}
}
