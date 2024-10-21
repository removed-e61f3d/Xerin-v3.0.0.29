using dnlib.DotNet;
using KoiVM.Core.CFG;
using KoiVM.Core.RT;

namespace KoiVM.Core.AST.IL;

public class ILBlock : BasicBlock<ILInstrList>
{
	public ILBlock(int id, ILInstrList content)
		: base(id, content)
	{
	}

	public virtual IChunk CreateChunk(VMRuntime rt, MethodDef method)
	{
		return new BasicBlockChunk(rt, method, this);
	}
}
