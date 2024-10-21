#define DEBUG
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;

[AttributeUsage(AttributeTargets.All)]
internal sealed class SRDisplayNameAttribute : DisplayNameAttribute
{
	public SRDisplayNameAttribute(string name)
	{
		base.DisplayNameValue = SR.GetString(name);
	}

	public SRDisplayNameAttribute(string name, string resourceSet)
	{
		ResourceManager resourceManager = new ResourceManager(resourceSet, Assembly.GetExecutingAssembly());
		base.DisplayNameValue = resourceManager.GetString(name);
		Debug.Assert(base.DisplayNameValue != null, string.Format(CultureInfo.CurrentCulture, "String resource {0} not found.", new object[1] { name }));
	}
}
