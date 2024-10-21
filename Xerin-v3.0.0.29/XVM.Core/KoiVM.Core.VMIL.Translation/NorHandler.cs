using System;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class NorHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.NOR;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.PushOperand(instr.Operand1);
		tr.PushOperand(instr.Operand2);
		switch (TypeInference.InferIntegerOp(instr.Operand1.Type, instr.Operand2.Type))
		{
		case ASTType.I4:
			tr.Instructions.Add(new ILInstruction(ILOpCode.NOR_DWORD));
			break;
		case ASTType.I8:
		case ASTType.Ptr:
			tr.Instructions.Add(new ILInstruction(ILOpCode.NOR_QWORD));
			break;
		default:
			throw new NotSupportedException();
		}
		tr.PopOperand(instr.Operand1);
	}
}
