using System.Collections.Generic;

namespace XProtections;

public static class sUtils
{
	public static Dictionary<string, string> hTable;

	public static string XorCipher(string cipherText, int key)
	{
		if (hTable == null)
		{
			hTable = new Dictionary<string, string>();
		}
		lock (hTable)
		{
			if (hTable.TryGetValue(cipherText, out var value))
			{
				return value;
			}
			char[] array = cipherText.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (char)(array[i] ^ key);
			}
			string text = new string(array);
			hTable[cipherText] = text;
			return text;
		}
	}
}
