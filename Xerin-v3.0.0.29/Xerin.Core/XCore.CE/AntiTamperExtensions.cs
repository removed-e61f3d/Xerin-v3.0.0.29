using System;
using System.Collections.Generic;
using dnlib.DotNet.Writer;

namespace XCore.CE;

public static class AntiTamperExtensions
{
	public static void AddBeforeReloc(this List<PESection> sections, PESection newSection)
	{
		if (sections == null)
		{
			throw new ArgumentNullException("sections");
		}
		sections.InsertBeforeReloc(sections.Count, newSection);
	}

	public static void AddAfterText(this List<PESection> sections, PESection newSection)
	{
		if (sections == null)
		{
			throw new ArgumentNullException("sections");
		}
		sections.InsertAfterText(sections.Count, newSection);
	}

	public static void InsertBeforeReloc(this List<PESection> sections, int preferredIndex, PESection newSection)
	{
		if (sections == null)
		{
			throw new ArgumentNullException("sections");
		}
		if (preferredIndex < 0 || preferredIndex > sections.Count)
		{
			throw new ArgumentOutOfRangeException("preferredIndex", preferredIndex, "Preferred index is out of range.");
		}
		if (newSection == null)
		{
			throw new ArgumentNullException("newSection");
		}
		int num = sections.FindIndex(0, Math.Min(preferredIndex + new Random().Next(2, 4), sections.Count), IsRelocSection);
		if (num == -1)
		{
			sections.Insert(preferredIndex, newSection);
		}
		else
		{
			sections.Insert(num, newSection);
		}
	}

	public static void InsertAfterText(this List<PESection> sections, int preferredIndex, PESection newSection)
	{
		if (sections == null)
		{
			throw new ArgumentNullException("sections");
		}
		if (preferredIndex < 0 || preferredIndex > sections.Count)
		{
			throw new ArgumentOutOfRangeException("preferredIndex", preferredIndex, "Preferred index is out of range.");
		}
		if (newSection == null)
		{
			throw new ArgumentNullException("newSection");
		}
		int num = sections.FindIndex(0, Math.Min(preferredIndex + new Random().Next(2, 4), sections.Count), IsTextSection);
		if (num == -1)
		{
			sections.Insert(preferredIndex, newSection);
		}
		else
		{
			sections.Insert(num, newSection);
		}
	}

	public static bool IsRelocSection(PESection section)
	{
		return section.Name.Equals(".reloc", StringComparison.Ordinal);
	}

	public static bool IsTextSection(PESection section)
	{
		return section.Name.Equals(".text", StringComparison.Ordinal);
	}
}
