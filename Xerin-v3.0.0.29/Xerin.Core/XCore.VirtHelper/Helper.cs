using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using dnlib.DotNet;
using KoiVM.Internal;
using XCore.Context;
using XCore.Utils;

namespace XCore.VirtHelper;

public class Helper
{
	public static bool isEnabled = false;

	public static List<string> names = new List<string>();

	public static void AddProtection(string item)
	{
		names.Add(item);
		InitializePhase.mnames.Add(item);
	}

	public static void RemoveProtection(string item)
	{
		string text = names.FirstOrDefault((string p) => p == item);
		if (text != null)
		{
			names.Remove(item);
			InitializePhase.mnames.Remove(item);
		}
	}

	public static void loadMethods(TreeView MethodTree, XContext context)
	{
		MethodTree.Nodes.Clear();
		HashSet<string> hashSet = new HashSet<string>();
		foreach (TypeDef type in context.Module.GetTypes())
		{
			hashSet.Add(type.Namespace);
		}
		hashSet.Distinct();
		foreach (TypeDef type2 in context.Module.Types)
		{
			if (!(type2.Namespace == string.Empty) || type2.Name.Contains("ImplementationDetails>") || type2.Name.Contains("<>f__AnonymousType"))
			{
				continue;
			}
			TreeNode treeNode = new TreeNode(type2.Name, 0, 0);
			treeNode.Tag = 1;
			foreach (MethodDef method in type2.Methods)
			{
				if (method != context.Module.GlobalType.FindOrCreateStaticConstructor() && XCore.Utils.Utils.OVMAnalyzer(method))
				{
					TreeNode treeNode2 = new TreeNode(string.Concat(method.Name, "  (", $"0x{method.MDToken:X4}", ")"));
					if (method.IsPublic && method.IsConstructor)
					{
						treeNode2.ForeColor = Color.FromArgb(5163440);
					}
					else if (method.IsPrivate && method.IsConstructor)
					{
						treeNode2.ForeColor = Color.FromArgb(4567708);
					}
					else if (method.IsAssembly && method.IsConstructor)
					{
						treeNode2.ForeColor = Color.FromArgb(5163440);
					}
					else if (method.IsFamily && method.IsConstructor)
					{
						treeNode2.ForeColor = Color.FromArgb(5163440);
					}
					else if (method.IsFamilyOrAssembly && method.IsConstructor)
					{
						treeNode2.ForeColor = Color.FromArgb(5163440);
					}
					else if (method.IsPublic)
					{
						treeNode2.ForeColor = Color.FromArgb(16744448);
					}
					else if (method.IsPrivate)
					{
						treeNode2.ForeColor = Color.FromArgb(13395456);
					}
					else if (method.IsAssembly)
					{
						treeNode2.ForeColor = Color.FromArgb(16744448);
					}
					else if (method.IsFamily)
					{
						treeNode2.ForeColor = Color.FromArgb(16744448);
					}
					else if (method.IsFamilyOrAssembly)
					{
						treeNode2.ForeColor = Color.FromArgb(16744448);
					}
					treeNode2.Tag = 2;
					treeNode.Nodes.Add(treeNode2);
				}
			}
			MethodTree.Nodes.Add(treeNode);
		}
		TreeNode treeNode3 = null;
		foreach (string item in hashSet)
		{
			if (!(item != string.Empty))
			{
				continue;
			}
			treeNode3 = new TreeNode(item, 0, 0);
			treeNode3.Tag = 0;
			MethodTree.Nodes.Add(treeNode3);
			foreach (TypeDef type3 in context.Module.Types)
			{
				if (!(treeNode3.Text == type3.Namespace) || !(type3.Namespace != string.Empty) || type3.IsValueType || type3.IsInterface)
				{
					continue;
				}
				string text = (type3.Name.Contains("`") ? type3.Name.Substring(0, type3.Name.IndexOf('`')) : type3.Name.Replace("`", string.Empty));
				TreeNode treeNode4 = new TreeNode(text, 0, 0);
				treeNode4.Tag = 1;
				foreach (MethodDef method2 in type3.Methods)
				{
					if (method2 != context.Module.GlobalType.FindOrCreateStaticConstructor() && XCore.Utils.Utils.OVMAnalyzer(method2))
					{
						TreeNode treeNode5 = new TreeNode(string.Concat(method2.Name, "  (", $"0x{method2.MDToken:X4}", ")"));
						if (method2.IsPublic && method2.IsConstructor)
						{
							treeNode5.ForeColor = Color.FromArgb(5163440);
						}
						else if (method2.IsPrivate && method2.IsConstructor)
						{
							treeNode5.ForeColor = Color.FromArgb(4567708);
						}
						else if (method2.IsAssembly && method2.IsConstructor)
						{
							treeNode5.ForeColor = Color.FromArgb(5163440);
						}
						else if (method2.IsFamily && method2.IsConstructor)
						{
							treeNode5.ForeColor = Color.FromArgb(5163440);
						}
						else if (method2.IsFamilyOrAssembly && method2.IsConstructor)
						{
							treeNode5.ForeColor = Color.FromArgb(5163440);
						}
						else if (method2.IsPublic)
						{
							treeNode5.ForeColor = Color.FromArgb(16744448);
						}
						else if (method2.IsPrivate)
						{
							treeNode5.ForeColor = Color.FromArgb(13395456);
						}
						else if (method2.IsAssembly)
						{
							treeNode5.ForeColor = Color.FromArgb(16744448);
						}
						else if (method2.IsFamily)
						{
							treeNode5.ForeColor = Color.FromArgb(16744448);
						}
						else if (method2.IsFamilyOrAssembly)
						{
							treeNode5.ForeColor = Color.FromArgb(16744448);
						}
						treeNode5.Tag = 2;
						treeNode4.Nodes.Add(treeNode5);
					}
				}
				treeNode3.Nodes.Add(treeNode4);
			}
		}
		try
		{
			TreeNode treeNode6 = null;
			int num = 0;
			while (num < MethodTree.Nodes.Count)
			{
				treeNode6 = MethodTree.Nodes[num];
				if (treeNode6.Nodes.Count == 0)
				{
					MethodTree.Nodes.Remove(treeNode6);
				}
				else
				{
					num++;
				}
			}
		}
		catch
		{
		}
		MethodTree.Sort();
	}
}
