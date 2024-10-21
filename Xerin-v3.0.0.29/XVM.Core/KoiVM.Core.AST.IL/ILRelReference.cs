using KoiVM.Core.RT;

namespace KoiVM.Core.AST.IL;

public class ILRelReference : IILOperand
{
	public IHasOffset Target { get; set; }

	public IHasOffset Base { get; set; }

	public ILRelReference(IHasOffset target, IHasOffset relBase)
	{
		Target = target;
		Base = relBase;
	}

	public virtual uint Resolve(VMRuntime runtime)
	{
		uint num = Base.Offset;
		if (Base is ILInstruction)
		{
			num += runtime.Serializer.ComputeLength((ILInstruction)Base);
		}
		return Target.Offset - num;
	}

	public override string ToString()
	{
		return $"[{Base.Offset:x8}:{Target.Offset:x8}]";
	}
}
