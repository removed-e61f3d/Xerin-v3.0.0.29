using System;
using System.Runtime.InteropServices;

namespace XRuntime;

public static class Terminator
{
	public const uint PROCESS_KILLRIGHTS = 1u;

	[DllImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

	[DllImport("kernel32.dll")]
	public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool CloseHandle(IntPtr hSnapshot);

	public static bool Kill(uint uiProcessId)
	{
		IntPtr intPtr = OpenProcess(1u, bInheritHandle: false, uiProcessId);
		bool result = TerminateProcess(intPtr, 0u);
		CloseHandle(intPtr);
		return result;
	}
}
