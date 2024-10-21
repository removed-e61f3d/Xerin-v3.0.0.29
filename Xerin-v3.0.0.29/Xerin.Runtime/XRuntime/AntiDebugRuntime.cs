using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace XRuntime;

public static class AntiDebugRuntime
{
	internal delegate int GetProcA();

	internal delegate int GetProcA2(IntPtr hProcess, ref int pbDebuggerPresent);

	internal delegate int WL(IntPtr wnd, IntPtr lParam);

	internal delegate int GetProcA3(WL lpEnumFunc, IntPtr lParam);

	internal struct ParentProcessUtilities
	{
		internal IntPtr Reserved1;

		internal IntPtr PebBaseAddress;

		internal IntPtr Reserved2_0;

		internal IntPtr Reserved2_1;

		internal IntPtr UniqueProcessId;

		internal IntPtr InheritedFromUniqueProcessId;

		[DllImport("ntdll.dll")]
		private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcessUtilities processInformation, uint processInformationLength, out int returnLength);

		internal static Process GetParentProcess()
		{
			return GetParentProcess(Process.GetCurrentProcess().Handle);
		}

		public static Process GetParentProcess(int id)
		{
			Process processById = Process.GetProcessById(id);
			return GetParentProcess(processById.Handle);
		}

		public static Process GetParentProcess(IntPtr handle)
		{
			ParentProcessUtilities processInformation = default(ParentProcessUtilities);
			if (NtQueryInformationProcess(handle, 0, ref processInformation, (uint)Marshal.SizeOf(processInformation), out var _) != 0)
			{
				return null;
			}
			try
			{
				return Process.GetProcessById(processInformation.InheritedFromUniqueProcessId.ToInt32());
			}
			catch (ArgumentException)
			{
				return null;
			}
		}
	}

	private static ParentProcessUtilities PPU;

	[DllImport("kernel32.dll")]
	private static extern bool IsDebuggerPresent();

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	private static extern int OutputDebugString(string str);

	[DllImport("kernel32.dll")]
	internal static extern int CloseHandle(IntPtr hModule);

	[DllImport("kernel32.dll")]
	internal static extern IntPtr OpenProcess(uint hModule, int procName, uint procId);

	[DllImport("kernel32.dll")]
	internal static extern uint GetCurrentProcessId();

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern IntPtr LoadLibrary(string hModule);

	[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
	internal static extern GetProcA GetProcAddress(IntPtr hModule, string procName);

	[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress")]
	internal static extern GetProcA2 GetProcAddress_2(IntPtr hModule, string procName);

	[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress")]
	internal static extern GetProcA3 GetProcAddress_3(IntPtr hModule, string procName);

	[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress")]
	public static extern IntPtr GetProcAddress2(IntPtr hModule, string procedureName);

	[DllImport("kernel32.dll")]
	internal static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

	public static Process GetParentProcess()
	{
		return ParentProcessUtilities.GetParentProcess();
	}

	internal static void Initialize()
	{
		string text = "COR";
		if (Environment.GetEnvironmentVariable(text + "_PROFILER") != null || Environment.GetEnvironmentVariable(text + "_ENABLE_PROFILING") != null)
		{
			Terminator.Kill((uint)Process.GetCurrentProcess().Id);
		}
		Process parentProcess = GetParentProcess();
		if (parentProcess != null && parentProcess.ProcessName.ToLower().Contains("dnspy"))
		{
			Terminator.Kill((uint)Process.GetCurrentProcess().Id);
		}
		if (parentProcess != null && parentProcess.ProcessName.ToLower().Contains("dbg"))
		{
			Terminator.Kill((uint)Process.GetCurrentProcess().Id);
		}
		Thread thread = new Thread(Detector)
		{
			IsBackground = true,
			Priority = ThreadPriority.Lowest
		};
		thread.Start(null);
	}

	internal static void Detector(object thread)
	{
		Thread thread2 = thread as Thread;
		if (thread2 == null)
		{
			thread2 = new Thread(Detector)
			{
				IsBackground = true,
				Priority = ThreadPriority.Lowest
			};
			thread2.Start(Thread.CurrentThread);
			Thread.Sleep(500);
		}
		while (true)
		{
			if (Debugger.IsAttached || Debugger.IsLogging())
			{
				Terminator.Kill((uint)Process.GetCurrentProcess().Id);
			}
			if (IsDebuggerPresent())
			{
				Terminator.Kill((uint)Process.GetCurrentProcess().Id);
			}
			Process currentProcess = Process.GetCurrentProcess();
			if (currentProcess.Handle == IntPtr.Zero)
			{
				Terminator.Kill((uint)Process.GetCurrentProcess().Id);
			}
			currentProcess.Close();
			if (OutputDebugString("") > IntPtr.Size)
			{
				Terminator.Kill((uint)Process.GetCurrentProcess().Id);
			}
			try
			{
				CloseHandle(IntPtr.Zero);
			}
			catch
			{
				Terminator.Kill((uint)Process.GetCurrentProcess().Id);
			}
			if (!thread2.IsAlive)
			{
				Environment.FailFast("");
			}
			try
			{
				byte[] array = new byte[1];
				Type typeFromHandle = typeof(Debugger);
				MethodInfo[] methods = typeFromHandle.GetMethods();
				MethodInfo method = typeFromHandle.GetMethod("get_IsAttached");
				IntPtr functionPointer = method.MethodHandle.GetFunctionPointer();
				Marshal.Copy(functionPointer, array, 0, 1);
				if (array[0] == 51)
				{
					Terminator.Kill((uint)Process.GetCurrentProcess().Id);
				}
				IntPtr hModule = LoadLibrary("kernel32.dll");
				if (Debugger.IsAttached)
				{
					Terminator.Kill((uint)Process.GetCurrentProcess().Id);
				}
				GetProcA procAddress = GetProcAddress(hModule, "IsDebuggerPresent");
				if (procAddress != null && procAddress() != 0)
				{
					Terminator.Kill((uint)Process.GetCurrentProcess().Id);
				}
				IntPtr intPtr = OpenProcess(1024u, 0, GetCurrentProcessId());
				if (intPtr != IntPtr.Zero)
				{
					try
					{
						GetProcA2 procAddress_ = GetProcAddress_2(hModule, "CheckRemoteDebuggerPresent");
						if (procAddress_ != null)
						{
							int pbDebuggerPresent = 0;
							if (procAddress_(intPtr, ref pbDebuggerPresent) != 0 && pbDebuggerPresent != 0)
							{
								Terminator.Kill((uint)Process.GetCurrentProcess().Id);
							}
						}
					}
					finally
					{
						CloseHandle(intPtr);
					}
				}
				bool flag = false;
				try
				{
					CloseHandle(new IntPtr(305419896));
				}
				catch
				{
					flag = true;
				}
				if (flag)
				{
					Terminator.Kill((uint)Process.GetCurrentProcess().Id);
				}
			}
			catch
			{
			}
			Thread.Sleep(1000);
		}
	}
}
