using System;
using System.Windows.Forms;

namespace XCore.Logger;

public class Logger
{
	public static void Log(RichTextBox richTextBox, string message)
	{
		if (richTextBox.InvokeRequired)
		{
			richTextBox.Invoke((MethodInvoker)delegate
			{
				Log(richTextBox, message);
			});
		}
		else
		{
			richTextBox.AppendText(message + Environment.NewLine);
			richTextBox.ScrollToCaret();
		}
	}
}
