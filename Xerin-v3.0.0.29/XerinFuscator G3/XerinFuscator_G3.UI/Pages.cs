using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace XerinFuscator_G3.UI;

internal class Pages
{
	public static string current { get; set; }

	private static async void fadeIn(Form form)
	{
		form.Opacity = 0.0;
		while (!(form.Opacity >= 0.98))
		{
			form.Opacity += 0.02;
			await Task.Delay(30);
		}
	}

	public static void changeButtons(Form form, Control container, Panel page, Guna2Button clickedButton, Guna2VSeparator separator)
	{
		Guna2Button[] array = container.Controls.OfType<Guna2Button>().ToArray();
		foreach (Guna2Button guna2Button in array)
		{
			bool flag = guna2Button == clickedButton;
			guna2Button.ForeColor = ((!flag) ? Color.FromArgb(90, 90, 90) : Color.White);
			switch (guna2Button.Name)
			{
			case "gotoColors":
				guna2Button.Image = ((!flag) ? nigger1.invert_colors_24px2 : nigger1.invert_colors_24px);
				break;
			case "gotoRenamer":
				guna2Button.Image = ((!flag) ? nigger1.translation_24px2 : nigger1.translation_24px);
				break;
			case "gotoProject":
				guna2Button.Image = ((!flag) ? nigger1.merge_24px2 : nigger1.merge_24px);
				break;
			case "gotoCodeEnc":
				guna2Button.Image = ((!flag) ? nigger1.shield_24px2 : nigger1.shield_24px);
				break;
			case "gotoCodeVirt":
				guna2Button.Image = (flag ? nigger1.data_protection_24px : nigger1.data_protection_24px2);
				break;
			case "gotoAssembly":
				guna2Button.Image = ((!flag) ? nigger1.file_24px2 : nigger1.file_24px);
				break;
			case "gotoProtections":
				guna2Button.Image = (flag ? nigger1.security_configuration_24px : nigger1.security_configuration_24px2);
				break;
			}
			if (flag)
			{
				page.BringToFront();
			}
			fadeIn(form);
			separator.Location = new Point(0, clickedButton.Location.Y);
		}
	}
}
