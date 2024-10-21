using System;
using System.Windows.Forms;

namespace XerinFuscator_G3;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: true);
		Application.Run(new XGui());
	}
}
