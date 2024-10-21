using System.Collections.Generic;
using dnlib.DotNet.Emit;

namespace XProtections.ControlFlow;

internal interface IPredicate
{
	void Inititalize(CilBody body);

	void EmitSwitchLoad(IList<Instruction> instrs);

	int GetSwitchKey(int key);
}
