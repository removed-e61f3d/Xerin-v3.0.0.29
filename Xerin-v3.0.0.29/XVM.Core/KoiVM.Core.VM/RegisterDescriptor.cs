using System.Linq;
using KoiVM.Core.Services;

namespace KoiVM.Core.VM;

public class RegisterDescriptor
{
	private readonly byte[] regOrder = (from x in Enumerable.Range(0, 13)
		select (byte)x).ToArray();

	public byte this[VMRegisters reg] => regOrder[(int)reg];

	internal RegisterDescriptor(RandomGenerator randomGenerator)
	{
		randomGenerator.Shuffle(regOrder);
	}
}
