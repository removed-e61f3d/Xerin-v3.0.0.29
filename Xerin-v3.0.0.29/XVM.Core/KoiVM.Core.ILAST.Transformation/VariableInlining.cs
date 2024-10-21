#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;

namespace KoiVM.Core.ILAST.Transformation;

public class VariableInlining : ITransformationHandler
{
	public void Initialize(ILASTTransformer tr)
	{
	}

	public static ILASTExpression GetExpression(IILASTStatement node)
	{
		if (node is ILASTExpression)
		{
			ILASTExpression iLASTExpression = (ILASTExpression)node;
			if (iLASTExpression.ILCode == Code.Pop && iLASTExpression.Arguments[0] is ILASTExpression)
			{
				iLASTExpression = (ILASTExpression)iLASTExpression.Arguments[0];
			}
			return iLASTExpression;
		}
		if (node is ILASTAssignment)
		{
			return ((ILASTAssignment)node).Value;
		}
		return null;
	}

	public void Transform(ILASTTransformer tr)
	{
		Dictionary<ILASTVariable, int> dictionary = new Dictionary<ILASTVariable, int>();
		for (int i = 0; i < tr.Tree.Count; i++)
		{
			IILASTStatement iILASTStatement = tr.Tree[i];
			ILASTExpression expression = GetExpression(iILASTStatement);
			if (expression == null)
			{
				continue;
			}
			if (iILASTStatement is ILASTExpression && expression.ILCode == Code.Nop)
			{
				tr.Tree.RemoveAt(i);
				i--;
				continue;
			}
			if (iILASTStatement is ILASTAssignment)
			{
				ILASTAssignment iLASTAssignment = (ILASTAssignment)iILASTStatement;
				if (Array.IndexOf(tr.Tree.StackRemains, iLASTAssignment.Variable) != -1)
				{
					continue;
				}
				Debug.Assert(iLASTAssignment.Variable.VariableType == ILASTVariableType.StackVar);
			}
			IILASTNode[] arguments = expression.Arguments;
			foreach (IILASTNode iILASTNode in arguments)
			{
				Debug.Assert(iILASTNode is ILASTVariable);
				ILASTVariable iLASTVariable = (ILASTVariable)iILASTNode;
				if (iLASTVariable.VariableType == ILASTVariableType.StackVar)
				{
					dictionary.Increment(iLASTVariable);
				}
			}
		}
		ILASTVariable[] stackRemains = tr.Tree.StackRemains;
		foreach (ILASTVariable key in stackRemains)
		{
			dictionary.Remove(key);
		}
		HashSet<ILASTVariable> hashSet = new HashSet<ILASTVariable>(from usage in dictionary
			where usage.Value == 1
			select usage into pair
			select pair.Key);
		bool flag;
		do
		{
			flag = false;
			for (int l = 0; l < tr.Tree.Count - 1; l++)
			{
				if (!(tr.Tree[l] is ILASTAssignment iLASTAssignment2) || !hashSet.Contains(iLASTAssignment2.Variable))
				{
					continue;
				}
				ILASTExpression expression2 = GetExpression(tr.Tree[l + 1]);
				if (expression2 == null || expression2.ILCode.ToOpCode().Name.StartsWith("stelem"))
				{
					continue;
				}
				for (int m = 0; m < expression2.Arguments.Length && expression2.Arguments[m] is ILASTVariable iLASTVariable2; m++)
				{
					if (iLASTVariable2 == iLASTAssignment2.Variable)
					{
						expression2.Arguments[m] = iLASTAssignment2.Value;
						tr.Tree.RemoveAt(l);
						l--;
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		while (flag);
	}
}
