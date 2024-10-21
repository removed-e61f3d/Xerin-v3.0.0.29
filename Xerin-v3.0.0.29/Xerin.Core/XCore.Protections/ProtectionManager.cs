using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using XCore.Context;
using XCore.Decompression;
using XCore.Embed;
using XCore.Logger;
using XCore.Terminator;
using XProtections;

namespace XCore.Protections;

public class ProtectionManager
{
	public static List<Protection> Protections = new List<Protection>();

	public void AddProtection(Protection protection)
	{
		Protections.Add(protection);
	}

	public void RemoveProtection(Protection protection)
	{
		Protection protection2 = Protections.FirstOrDefault((Protection p) => p.name == protection.name);
		if (protection2 != null)
		{
			Protections.Remove(protection2);
		}
	}

	public void ExecuteProtections(XContext context, RichTextBox logBox)
	{
		DateTime now = DateTime.Now;
		SortProtectionsByNumber();
		if (!Protections.Contains(new cctorHider()))
		{
			AddProtection(new cctorHider());
		}
		RemoveDuplicatedProtections();
		XCore.Logger.Logger.Log(logBox, $"Obfuscation process has been started at: {now}");
		foreach (Protection protection in Protections)
		{
			if (protection.number >= 5 && QuickLZ.QLZDecompression == null)
			{
				new QuickLZ().injectQuickLZ(context);
			}
			if (protection.number >= 5 && XCore.Terminator.Terminator.Kill == null)
			{
				new XCore.Terminator.Terminator().injectKill(context);
			}
			XCore.Logger.Logger.Log(logBox, "Executing " + protection.name + "....");
			if (protection.number == 14 && Embeder.isEmptyList)
			{
				XCore.Logger.Logger.Log(logBox, "Warning: dlls list is empty, so embed has been canceled!");
				continue;
			}
			protection.Execute(context);
			XCore.Logger.Logger.Log(logBox, "Execution of " + protection.name + " completed.");
		}
		XCore.Logger.Logger.Log(logBox, "All protections executed successfully.");
		QuickLZ.QLZDecompression = null;
		XCore.Terminator.Terminator.Kill = null;
	}

	public static void RemoveDuplicatedProtections()
	{
		Protections = (from p in Protections
			group p by new { p.number, p.name } into g
			select g.First()).ToList();
	}

	private void SortProtectionsByNumber()
	{
		Protections.Sort((Protection x, Protection y) => x.number.CompareTo(y.number));
	}
}
