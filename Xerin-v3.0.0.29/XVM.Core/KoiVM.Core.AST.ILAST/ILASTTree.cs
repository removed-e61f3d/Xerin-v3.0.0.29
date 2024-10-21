using System;
using System.Collections.Generic;
using System.Text;

namespace KoiVM.Core.AST.ILAST;

public class ILASTTree : List<IILASTStatement>
{
	public ILASTVariable[] StackRemains { get; set; }

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (Enumerator enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				IILASTStatement current = enumerator.Current;
				stringBuilder.AppendLine(current.ToString());
			}
		}
		stringBuilder.AppendLine();
		stringBuilder.Append("[");
		for (int i = 0; i < StackRemains.Length; i++)
		{
			if (i != 0)
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(StackRemains[i]);
		}
		stringBuilder.AppendLine("]");
		return stringBuilder.ToString();
	}

	public void TraverseTree<T>(Action<ILASTExpression, T> visitFunc, T state)
	{
		using Enumerator enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			IILASTStatement current = enumerator.Current;
			if (current is ILASTExpression)
			{
				TraverseTreeInternal((ILASTExpression)current, visitFunc, state);
			}
			else if (current is ILASTAssignment)
			{
				TraverseTreeInternal(((ILASTAssignment)current).Value, visitFunc, state);
			}
		}
	}

	private void TraverseTreeInternal<T>(ILASTExpression expr, Action<ILASTExpression, T> visitFunc, T state)
	{
		IILASTNode[] arguments = expr.Arguments;
		foreach (IILASTNode iILASTNode in arguments)
		{
			if (iILASTNode is ILASTExpression)
			{
				TraverseTreeInternal((ILASTExpression)iILASTNode, visitFunc, state);
			}
		}
		visitFunc(expr, state);
	}
}
