using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Confuser.Core;
using Confuser.Core.Project;
using dnlib.DotNet;
using XCore.Context;
using XCore.Protections;

namespace cfex.Renamer;

public class cfexRenamer : XCore.Protections.Protection
{
	public static XContext context;

	public override string name => "cfex Renamer";

	public override int number => 0;

	public override void Execute(XContext ctx)
	{
		string text = "";
		try
		{
			ConfuserProject confuserProject = new ConfuserProject
			{
				BaseDirectory = Path.GetDirectoryName(ctx.Path)
			};
			confuserProject.OutputDirectory = Path.Combine(confuserProject.BaseDirectory, "tmp");
			text = confuserProject.OutputDirectory;
			ProjectModule item = new ProjectModule
			{
				Path = Path.GetFileName(ctx.Path)
			};
			confuserProject.Add(item);
			Rule rule = new Rule();
			SettingItem<Confuser.Core.Protection> item2 = new SettingItem<Confuser.Core.Protection>("rename");
			rule.Add(item2);
			confuserProject.Rules.Add(rule);
			XmlDocument xmlDocument = confuserProject.Save();
			string text2 = Path.Combine(Directory.GetCurrentDirectory(), "Renamer", "temp.crproj");
			xmlDocument.Save(text2);
			Process process = new Process();
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.FileName = "Renamer\\Confuser.CLI.exe";
			process.StartInfo.Arguments = "-n Renamer\\temp.crproj";
			process.Start();
			process.WaitForExit();
			File.Delete(text2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		string text3 = Path.Combine(text);
		string text4 = Path.Combine(text3, Path.GetFileName(ctx.Path));
		ctx.tmp = text3;
		if (File.Exists(text4))
		{
			ctx.Module = ModuleDefMD.Load(text4);
		}
	}
}
