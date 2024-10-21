using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace XProtections.Mutation;

internal class SizeofMutation : ConstantMutation
{
	private TypeRef _valueTypeRef;

	private TypeRef _intPtrTypeRef;

	public SizeofMutation(ModuleDef module)
		: base(module)
	{
	}

	protected override void Initialise(ModuleDef module)
	{
		_valueTypeRef = module.CorLibTypes.GetTypeRef("System", "ValueType");
		_intPtrTypeRef = module.CorLibTypes.GetTypeRef("System", "IntPtr");
	}

	public override void Mutate(IList<Instruction> instructions, int index)
	{
		Instruction instruction = instructions[index];
		int num = instruction.GetLdcI4Value();
		bool flag;
		if (flag = num % 2 != 0)
		{
			num++;
		}
		instruction.OpCode = OpCodes.Sizeof;
		instruction.Operand = _intPtrTypeRef;
		instructions.Add(OpCodes.Ldc_I4_4.ToInstruction());
		Instruction instruction2 = OpCodes.Nop.ToInstruction();
		Instruction instruction3 = OpCodes.Nop.ToInstruction();
		instructions.Add(OpCodes.Ceq.ToInstruction());
		instructions.Add(OpCodes.Brfalse.ToInstruction(instruction2));
		instructions.Add(OpCodes.Sizeof.ToInstruction(_intPtrTypeRef));
		if (num % 4 == 0)
		{
			instructions.Add(Instruction.CreateLdcI4(num / 4));
		}
		else
		{
			instructions.Add(OpCodes.Ldc_I4_2.ToInstruction());
			instructions.Add(OpCodes.Div.ToInstruction());
			instructions.Add(Instruction.CreateLdcI4(num / 2));
		}
		instructions.Add(OpCodes.Mul.ToInstruction());
		instructions.Add(OpCodes.Br.ToInstruction(instruction3));
		instructions.Add(instruction2);
		instructions.Add(OpCodes.Sizeof.ToInstruction(_intPtrTypeRef));
		if (num % 8 == 0)
		{
			instructions.Add(Instruction.CreateLdcI4(num / 8));
		}
		else
		{
			instructions.Add(OpCodes.Ldc_I4_4.ToInstruction());
			instructions.Add(OpCodes.Div.ToInstruction());
			instructions.Add(Instruction.CreateLdcI4(num / 2));
		}
		instructions.Add(OpCodes.Mul.ToInstruction());
		instructions.Add(instruction3);
		if (flag)
		{
			instructions.Add(OpCodes.Ldc_I4_1.ToInstruction());
			instructions.Add(OpCodes.Sub.ToInstruction());
		}
	}
}
