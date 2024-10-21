using System;
using dnlib.DotNet;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class SxHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.SX;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.PushOperand(instr.Operand2);
		switch (instr.Operand1.Type)
		{
		case ASTType.I4:
			if (instr.Operand1 is IRVariable)
			{
				ElementType elementType = ((IRVariable)instr.Operand1).RawType.ElementType;
				if (elementType == ElementType.I2)
				{
					tr.Instructions.Add(new ILInstruction(ILOpCode.SX_WORD));
				}
			}
			tr.Instructions.Add(new ILInstruction(ILOpCode.SX_BYTE));
			break;
		case ASTType.I8:
			tr.Instructions.Add(new ILInstruction(ILOpCode.SX_DWORD));
			break;
		default:
			throw new NotSupportedException();
		}
		tr.PopOperand(instr.Operand1);
	}
}
