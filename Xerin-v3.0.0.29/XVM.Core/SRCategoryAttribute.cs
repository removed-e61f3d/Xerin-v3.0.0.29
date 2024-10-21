#define DEBUG
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;

[AttributeUsage(AttributeTargets.All)]
internal sealed class SRCategoryAttribute : CategoryAttribute
{
	private string resourceSet = string.Empty;

	public SRCategoryAttribute(string category)
		: base(category)
	{
	}

	public SRCategoryAttribute(string category, string resourceSet)
		: base(category)
	{
		this.resourceSet = resourceSet;
	}

	protected override string GetLocalizedString(string value)
	{
		if (resourceSet.Length > 0)
		{
			ResourceManager resourceManager = new ResourceManager(resourceSet, Assembly.GetExecutingAssembly());
			string @string = resourceManager.GetString(value);
			Debug.Assert(@string != null, string.Format(CultureInfo.CurrentCulture, "String resource {0} not found.", new object[1] { value }));
			return @string;
		}
		return SR.GetString(value);
	}
}
