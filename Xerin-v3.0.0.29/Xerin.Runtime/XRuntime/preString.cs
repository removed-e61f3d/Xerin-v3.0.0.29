namespace XRuntime;

public static class preString
{
	public static string subIt(this string data)
	{
		return string.IsNullOrEmpty(data) ? data : data.Substring(0, data.Length - 1);
	}
}
