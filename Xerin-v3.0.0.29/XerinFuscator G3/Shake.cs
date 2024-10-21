using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

internal static class Shake
{
	public static async Task ShakeIT(Form form)
	{
		Point original = form.Location;
		Random rnd = new Random(1337);
		for (int i = 0; i < 10; i++)
		{
			form.Location = new Point(original.X + rnd.Next(-10, 10), original.Y + rnd.Next(-10, 10));
			await Task.Delay(5);
		}
		form.Location = original;
	}
}
