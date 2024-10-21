using System;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class SubHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.SUB;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.PushOperand(instr.Operand1);
		tr.PushOperand(instr.Operand2);
		switch (TypeInference.InferBinaryOp(instr.Operand1.Type, instr.Operand2.Type))
		{
		case ASTType.R4:
			tr.Instructions.Add(new ILInstruction(ILOpCode.SUB_R32));
			break;
		case ASTType.R8:
			tr.Instructions.Add(new ILInstruction(ILOpCode.SUB_R64));
			break;
		default:
			throw new NotSupportedException();
		}
		tr.PopOperand(instr.Operand1);
	}
}
