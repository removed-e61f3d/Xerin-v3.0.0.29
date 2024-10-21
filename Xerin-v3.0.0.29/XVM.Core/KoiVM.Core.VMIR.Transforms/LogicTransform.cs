using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Transforms;

public class LogicTransform : ITransform
{
	public void Initialize(IRTransformer tr)
	{
	}

	public void Transform(IRTransformer tr)
	{
		tr.Instructions.VisitInstrs(VisitInstr, tr);
	}

	private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
	{
		if (instr.OpCode == IROpCode.__NOT)
		{
			instrs.Replace(index, new IRInstruction[1]
			{
				new IRInstruction(IROpCode.NOR, instr.Operand1, instr.Operand1, instr)
			});
		}
		else if (instr.OpCode == IROpCode.__AND)
		{
			IRVariable iRVariable = tr.Context.AllocateVRegister(instr.Operand2.Type);
			instrs.Replace(index, new IRInstruction[4]
			{
				new IRInstruction(IROpCode.MOV, iRVariable, instr.Operand2, instr),
				new IRInstruction(IROpCode.NOR, instr.Operand1, instr.Operand1, instr),
				new IRInstruction(IROpCode.NOR, iRVariable, iRVariable, instr),
				new IRInstruction(IROpCode.NOR, instr.Operand1, iRVariable, instr)
			});
		}
		else if (instr.OpCode == IROpCode.__OR)
		{
			instrs.Replace(index, new IRInstruction[2]
			{
				new IRInstruction(IROpCode.NOR, instr.Operand1, instr.Operand2, instr),
				new IRInstruction(IROpCode.NOR, instr.Operand1, instr.Operand1, instr)
			});
		}
		else if (instr.OpCode == IROpCode.__XOR)
		{
			IRVariable iRVariable2 = tr.Context.AllocateVRegister(instr.Operand2.Type);
			IRVariable iRVariable3 = tr.Context.AllocateVRegister(instr.Operand2.Type);
			instrs.Replace(index, new IRInstruction[7]
			{
				new IRInstruction(IROpCode.MOV, iRVariable2, instr.Operand1, instr),
				new IRInstruction(IROpCode.NOR, iRVariable2, instr.Operand2, instr),
				new IRInstruction(IROpCode.MOV, iRVariable3, instr.Operand2, instr),
				new IRInstruction(IROpCode.NOR, instr.Operand1, instr.Operand1, instr),
				new IRInstruction(IROpCode.NOR, iRVariable3, iRVariable3, instr),
				new IRInstruction(IROpCode.NOR, instr.Operand1, iRVariable3, instr),
				new IRInstruction(IROpCode.NOR, instr.Operand1, iRVariable2, instr)
			});
		}
	}
}
