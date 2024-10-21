using dnlib.DotNet;

namespace KoiVM.Core.VM;

internal class FuncSigDesc
{
	public readonly ITypeDefOrRef DeclaringType;

	public readonly FuncSig FuncSig;

	public readonly uint Id;

	public readonly MethodDef Method;

	public readonly MethodSig Signature;

	public FuncSigDesc(uint id, MethodDef method)
	{
		Id = id;
		Method = method;
		DeclaringType = method.DeclaringType;
		Signature = method.MethodSig;
		FuncSig = new FuncSig();
	}

	public FuncSigDesc(uint id, ITypeDefOrRef declType, MethodSig sig)
	{
		Id = id;
		Method = null;
		DeclaringType = declType;
		Signature = sig;
		FuncSig = new FuncSig();
	}
}
