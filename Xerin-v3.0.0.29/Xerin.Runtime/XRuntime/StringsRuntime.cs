using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace XRuntime;

public static class StringsRuntime
{
	public static Dictionary<string, string> hTable;

	public static string[] stringsList;

	public static byte[] Read(Stream stream)
	{
		using MemoryStream memoryStream = new MemoryStream();
		stream.CopyTo(memoryStream);
		return memoryStream.ToArray();
	}

	public static void Extract()
	{
		stringsList = new string[0];
		Stream manifestResourceStream = typeof(StringsRuntime).Assembly.GetManifestResourceStream("CallMeInx");
		StreamReader streamReader = new StreamReader(new MemoryStream(QuickLZDecompression.decompress(Read(manifestResourceStream))));
		string text;
		while ((text = streamReader.ReadLine()) != null)
		{
			Array.Resize(ref stringsList, stringsList.Length + 1);
			stringsList[stringsList.Length - 1] = text;
		}
	}

	public static string Call(string[] arr, Dictionary<string, string> dictionary, int index)
	{
		if (dictionary == null)
		{
			dictionary = new Dictionary<string, string>();
		}
		if (hTable == null)
		{
			hTable = new Dictionary<string, string>();
		}
		if (index < 0 || index >= arr.Length)
		{
			return null;
		}
		string text = arr[index];
		lock (hTable)
		{
			if (hTable.TryGetValue(text, out var value))
			{
				return value;
			}
			using Aes aes = Aes.Create();
			aes.Key = Encoding.UTF8.GetBytes("Key");
			aes.IV = Encoding.UTF8.GetBytes("IV");
			ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
			byte[] buffer = Convert.FromBase64String(text);
			using MemoryStream stream = new MemoryStream(buffer);
			using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
			using StreamReader streamReader = new StreamReader(stream2);
			string text2 = streamReader.ReadToEnd();
			hTable[text] = text2;
			dictionary[text] = text2;
			return text2;
		}
	}
}
