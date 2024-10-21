using System.Collections.Generic;
using KoiVM.Core.Helpers;

namespace KoiVM.Core.AST.IL;

public class ILInstrList : List<ILInstruction>
{
	public void VisitInstrs<T>(VisitFunc<ILInstrList, ILInstruction, T> visitFunc, T arg)
	{
		for (int i = 0; i < base.Count; i++)
		{
			visitFunc(this, base[i], ref i, arg);
		}
	}
}
