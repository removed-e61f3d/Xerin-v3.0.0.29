using System.Collections.Generic;
using KoiVM.Core.Helpers;

namespace KoiVM.Core.AST.IR;

public class IRInstrList : List<IRInstruction>
{
	public void VisitInstrs<T>(VisitFunc<IRInstrList, IRInstruction, T> visitFunc, T arg)
	{
		for (int i = 0; i < base.Count; i++)
		{
			visitFunc(this, base[i], ref i, arg);
		}
	}
}
