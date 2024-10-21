#define DEBUG
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;

[AttributeUsage(AttributeTargets.All)]
internal sealed class SRDescriptionAttribute : DescriptionAttribute
{
	public SRDescriptionAttribute(string description)
	{
		base.DescriptionValue = SR.GetString(description);
	}

	public SRDescriptionAttribute(string description, string resourceSet)
	{
		ResourceManager resourceManager = new ResourceManager(resourceSet, Assembly.GetExecutingAssembly());
		base.DescriptionValue = resourceManager.GetString(description);
		Debug.Assert(base.DescriptionValue != null, string.Format(CultureInfo.CurrentCulture, "String resource {0} not found.", new object[1] { description }));
	}
}
