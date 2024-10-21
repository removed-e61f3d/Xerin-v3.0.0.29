#define DEBUG
using System.Diagnostics;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Transforms;

public class LeaTransform : ITransform
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
		if (instr.OpCode == IROpCode.__LEA)
		{
			IRPointer iRPointer = (IRPointer)instr.Operand2;
			IIROperand operand = instr.Operand1;
			Debug.Assert(iRPointer.Register == IRRegister.BP);
			instrs.Replace(index, new IRInstruction[2]
			{
				new IRInstruction(IROpCode.MOV, operand, IRRegister.BP, instr),
				new IRInstruction(IROpCode.ADD, operand, IRConstant.FromI4(iRPointer.Offset), instr)
			});
		}
	}
}
