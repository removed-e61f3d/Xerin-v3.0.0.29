using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace XRuntime;

public static class AntiVMRuntime
{
	[DllImport("kernel32.dll", EntryPoint = "GetModuleHandle")]
	internal static extern IntPtr GenericAcl(string lpModuleName);

	[DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
	internal static extern IntPtr TryCode(IntPtr hModule, string procName);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GetFileAttributes", SetLastError = true)]
	internal static extern uint ISymbolReader(string lpFileName);

	internal static bool SecurityDocumentElement()
	{
		if (!MessageDictionary())
		{
			return false;
		}
		return true;
	}

	internal static string SoapNcName([In] string obj0, [In] string obj1)
	{
		RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(obj0, writable: false);
		if (registryKey == null)
		{
			return "noKey";
		}
		object value = registryKey.GetValue(obj1, "noValueButYesKey");
		if (value is string || registryKey.GetValueKind(obj1) == RegistryValueKind.String || registryKey.GetValueKind(obj1) == RegistryValueKind.ExpandString)
		{
			return value.ToString();
		}
		if (registryKey.GetValueKind(obj1) == RegistryValueKind.DWord)
		{
			return Convert.ToString((int)value);
		}
		if (registryKey.GetValueKind(obj1) == RegistryValueKind.QWord)
		{
			return Convert.ToString((long)value);
		}
		if (registryKey.GetValueKind(obj1) == RegistryValueKind.Binary)
		{
			return Convert.ToString((byte[])value);
		}
		if (registryKey.GetValueKind(obj1) == RegistryValueKind.MultiString)
		{
			return string.Join("", (string[])value);
		}
		return "noValueButYesKey";
	}

	internal static bool MessageDictionary()
	{
		if (SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 0\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VBOX") || SoapNcName("HARDWARE\\Description\\System", "SystemBiosVersion").ToUpper().Contains("VBOX") || SoapNcName("HARDWARE\\Description\\System", "VideoBiosVersion").ToUpper().Contains("VIRTUALBOX") || SoapNcName("SOFTWARE\\Oracle\\VirtualBox Guest Additions", "") == "noValueButYesKey" || ISymbolReader("C:\\WINDOWS\\system32\\drivers\\VBoxMouse.sys") != uint.MaxValue || SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 0\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VMWARE") || SoapNcName("SOFTWARE\\VMware, Inc.\\VMware Tools", "") == "noValueButYesKey" || SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 1\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VMWARE") || SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 2\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VMWARE") || SoapNcName("SYSTEM\\ControlSet001\\Services\\Disk\\Enum", "0").ToUpper().Contains("vmware".ToUpper()) || SoapNcName("SYSTEM\\ControlSet001\\Control\\Class\\{4D36E968-E325-11CE-BFC1-08002BE10318}\\0000", "DriverDesc").ToUpper().Contains("VMWARE") || SoapNcName("SYSTEM\\ControlSet001\\Control\\Class\\{4D36E968-E325-11CE-BFC1-08002BE10318}\\0000\\Settings", "Device Description").ToUpper().Contains("VMWARE") || SoapNcName("SOFTWARE\\VMware, Inc.\\VMware Tools", "InstallPath").ToUpper().Contains("C:\\PROGRAM FILES\\VMWARE\\VMWARE TOOLS\\") || ISymbolReader("C:\\WINDOWS\\system32\\drivers\\vmmouse.sys") != uint.MaxValue || ISymbolReader("C:\\WINDOWS\\system32\\drivers\\vmhgfs.sys") != uint.MaxValue || TryCode(GenericAcl("kernel32.dll"), "wine_get_unix_file_name") != (IntPtr)0 || SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 0\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("QEMU") || SoapNcName("HARDWARE\\Description\\System", "SystemBiosVersion").ToUpper().Contains("QEMU"))
		{
			return true;
		}
		foreach (ManagementBaseObject item in new ManagementObjectSearcher(new ManagementScope("\\\\.\\ROOT\\cimv2"), new ObjectQuery("SELECT * FROM Win32_VideoController")).Get())
		{
			ManagementObject managementObject = (ManagementObject)item;
			if (managementObject["Description"].ToString() == "VM Additions S3 Trio32/64" || managementObject["Description"].ToString() == "S3 Trio32/64" || managementObject["Description"].ToString() == "VirtualBox Graphics Adapter" || managementObject["Description"].ToString() == "VMware SVGA II" || managementObject["Description"].ToString().ToUpper().Contains("VMWARE") || managementObject["Description"].ToString() == "")
			{
				return true;
			}
		}
		return false;
	}

	internal static void CrossAppDomainSerializer(string A_0)
	{
		Process.Start(new ProcessStartInfo("cmd.exe", "/c " + A_0)
		{
			CreateNoWindow = true,
			UseShellExecute = false
		});
	}

	public static void Initialize()
	{
		if (SecurityDocumentElement())
		{
			CrossAppDomainSerializer("START CMD /C \"COLOR 03 && ECHO This assembly can't be executed on virtual machines! && PAUSE\" ");
			ProcessStartInfo processStartInfo = new ProcessStartInfo
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true,
				Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
				FileName = "cmd.exe"
			};
			Terminator.Kill((uint)Process.GetCurrentProcess().Id);
		}
	}
}
