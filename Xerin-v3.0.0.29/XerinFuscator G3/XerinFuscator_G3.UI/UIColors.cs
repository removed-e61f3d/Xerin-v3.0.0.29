using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace XerinFuscator_G3.UI;

internal class UIColors
{
	public static Color readColor(int R, int G, int B)
	{
		return Color.FromArgb(R, G, B);
	}

	public static void setColor(Control header, Control[] controls, Control[] containers, Guna2VSeparator separator, Color color)
	{
		Guna2ControlBox[] array = header.Controls.OfType<Guna2ControlBox>().ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].HoverState.IconColor = color;
		}
		Control[] array2 = controls;
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j].ForeColor = color;
		}
		separator.FillColor = color;
		array2 = containers;
		foreach (Control control in array2)
		{
			Guna2ToggleSwitch[] array3 = control.Controls.OfType<Guna2ToggleSwitch>().ToArray();
			for (int k = 0; k < array3.Length; k++)
			{
				array3[k].CheckedState.FillColor = color;
			}
			Guna2TextBox[] array4 = control.Controls.OfType<Guna2TextBox>().ToArray();
			foreach (Guna2TextBox obj in array4)
			{
				obj.HoverState.BorderColor = color;
				obj.FocusedState.BorderColor = color;
			}
			Panel[] array5 = control.Controls.OfType<Panel>().ToArray();
			foreach (Panel obj2 in array5)
			{
				Guna2ToggleSwitch[] array6 = obj2.Controls.OfType<Guna2ToggleSwitch>().ToArray();
				Guna2VSeparator[] array7 = obj2.Controls.OfType<Guna2VSeparator>().ToArray();
				for (int n = 0; n < array6.Length; n++)
				{
					array6[n].CheckedState.FillColor = color;
				}
				for (int num = 0; num < array7.Length; num++)
				{
					array7[num].FillColor = color;
				}
			}
		}
	}
}
