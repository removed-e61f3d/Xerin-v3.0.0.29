using System;
using System.Text;
using dnlib.DotNet;
using XCore.Context;
using XCore.Generator;

namespace XProtections;

public class SpamResources
{
	public static void Execute(XContext ctx)
	{
		for (int i = 0; i < 25; i++)
		{
			string s = GGeneration.GenerateGuidStartingWithLetter();
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			string s2 = Convert.ToBase64String(bytes);
			byte[] bytes2 = Encoding.ASCII.GetBytes(s2);
			EmbeddedResource item = new EmbeddedResource(new UTF8String(GGeneration.GenerateGuidStartingWithLetter()), bytes2);
			ctx.Module.Resources.Add(item);
		}
	}
}
