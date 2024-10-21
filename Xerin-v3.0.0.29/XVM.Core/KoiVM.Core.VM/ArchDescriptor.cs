using KoiVM.Core.Services;

namespace KoiVM.Core.VM;

public class ArchDescriptor
{
	public OpCodeDescriptor OpCodes { get; }

	public FlagDescriptor Flags { get; }

	public RegisterDescriptor Registers { get; }

	internal ArchDescriptor(RandomGenerator randomGenerator)
	{
		OpCodes = new OpCodeDescriptor(randomGenerator);
		Flags = new FlagDescriptor(randomGenerator);
		Registers = new RegisterDescriptor(randomGenerator);
	}
}
