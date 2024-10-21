using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace XCore.CE;

public interface IKeyDeriver
{
	void Init();

	uint[] DeriveKey(uint[] a, uint[] b);

	IEnumerable<Instruction> EmitDerivation(MethodDef method, Local dst, Local src);
}
