using System.Drawing;
using System.Windows.Forms;

namespace XerinFuscator_G3.UI;

internal static class NotificationPopUp
{
	public static void Position(Form form)
	{
		int num = Screen.GetWorkingArea(form).Width - 14;
		int num2 = Screen.GetWorkingArea(form).Height - 14;
		form.Location = new Point(num - form.Width, num2 - form.Height);
	}

	public static void PopUp(string title, string msg)
	{
		XerinFuscator_G3.PopUp.Title = title;
		XerinFuscator_G3.PopUp.Content = msg;
		new PopUp().ShowDialog();
	}
}
