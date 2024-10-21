using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace XCore.Generator;

public class GGeneration
{
	private static readonly HashSet<string> stored = new HashSet<string>();

	private static Random random = new Random();

	public static string Custom { get; set; }

	public static bool CustomRN { get; set; }

	public static string RandomString(int length)
	{
		return new string((from s in Enumerable.Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", length)
			select s[random.Next(s.Length)]).ToArray());
	}

	public static string RandomStringWithRandomLength()
	{
		int count = random.Next(5, 101);
		return new string((from s in Enumerable.Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", count)
			select s[random.Next(s.Length)]).ToArray());
	}

	private static string MD5Hash(string input)
	{
		StringBuilder stringBuilder = new StringBuilder();
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] array = mD5CryptoServiceProvider.ComputeHash(new UTF8Encoding().GetBytes(input));
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	private static char GetLetter()
	{
		Random random = new Random();
		int num = random.Next(0, 25);
		return (char)(97 + num);
	}

	public static string GenerateRandomString()
	{
		Random random = new Random();
		string input = GenerateRandomString(random.Next(2, 24));
		string text = MD5Hash(input);
		if (char.IsDigit(text[0]))
		{
			char letter = GetLetter();
			text = text.Replace(text[0], letter);
		}
		return text;
	}

	public static string GenerateRandomString(int size)
	{
		string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		char[] array = text.ToCharArray();
		byte[] data = new byte[1];
		RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		rNGCryptoServiceProvider.GetNonZeroBytes(data);
		data = new byte[size];
		rNGCryptoServiceProvider.GetNonZeroBytes(data);
		StringBuilder stringBuilder = new StringBuilder(size);
		byte[] array2 = data;
		foreach (byte b in array2)
		{
			stringBuilder.Append(array[b % array.Length]);
		}
		return stringBuilder.ToString();
	}

	public static string GenerateGuidStartingWithLetter()
	{
		string text;
		do
		{
			text = (CustomRN ? string.Concat(Custom, "_" + GenerateRandomString()) : GenerateRandomString());
		}
		while (!stored.Add(text));
		return text;
	}
}
