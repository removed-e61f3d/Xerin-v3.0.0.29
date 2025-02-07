using System.Collections.Generic;
using KoiVM.Core.VM;

namespace KoiVM.Core.AST.IL;

public class ILRegister : IILOperand
{
	private static readonly Dictionary<VMRegisters, ILRegister> regMap = new Dictionary<VMRegisters, ILRegister>();

	public static readonly ILRegister R0 = new ILRegister(VMRegisters.R0);

	public static readonly ILRegister R1 = new ILRegister(VMRegisters.R1);

	public static readonly ILRegister R2 = new ILRegister(VMRegisters.R2);

	public static readonly ILRegister R3 = new ILRegister(VMRegisters.R3);

	public static readonly ILRegister R4 = new ILRegister(VMRegisters.R4);

	public static readonly ILRegister R5 = new ILRegister(VMRegisters.R5);

	public static readonly ILRegister R6 = new ILRegister(VMRegisters.R6);

	public static readonly ILRegister R7 = new ILRegister(VMRegisters.R7);

	public static readonly ILRegister BP = new ILRegister(VMRegisters.BP);

	public static readonly ILRegister SP = new ILRegister(VMRegisters.SP);

	public static readonly ILRegister IP = new ILRegister(VMRegisters.IP);

	public static readonly ILRegister FL = new ILRegister(VMRegisters.FL);

	public static readonly ILRegister K1 = new ILRegister(VMRegisters.K1);

	public VMRegisters Register { get; set; }

	private ILRegister(VMRegisters reg)
	{
		Register = reg;
		regMap.Add(reg, this);
	}

	public override string ToString()
	{
		return Register.ToString();
	}

	public static ILRegister LookupRegister(VMRegisters reg)
	{
		return regMap[reg];
	}
}
