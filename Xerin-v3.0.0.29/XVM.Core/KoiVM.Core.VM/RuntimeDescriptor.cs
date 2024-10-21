using KoiVM.Core.Services;

namespace KoiVM.Core.VM;

public class RuntimeDescriptor
{
	public VMCallDescriptor VMCall { get; }

	public VCallOpsDescriptor VCallOps { get; }

	public RTFlagDescriptor RTFlags { get; }

	internal RuntimeDescriptor(RandomGenerator randomGenerator)
	{
		VMCall = new VMCallDescriptor(randomGenerator);
		VCallOps = new VCallOpsDescriptor(randomGenerator);
		RTFlags = new RTFlagDescriptor(randomGenerator);
	}
}
