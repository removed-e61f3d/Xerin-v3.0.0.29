using dnlib.DotNet;

namespace KoiVM.Core;

public interface IVMSettings
{
	int Seed { get; }

	bool IsVirtualized(MethodDef method);
}
