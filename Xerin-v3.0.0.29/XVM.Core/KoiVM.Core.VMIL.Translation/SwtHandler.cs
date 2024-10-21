#define DEBUG
using System.Diagnostics;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL.Translation;

public class SwtHandler : ITranslationHandler
{
	public IROpCode IRCode => IROpCode.SWT;

	public void Translate(IRInstruction instr, ILTranslator tr)
	{
		tr.PushOperand(instr.Operand2);
		tr.PushOperand(instr.Operand1);
		ILInstruction iLInstruction = tr.Instructions[tr.Instructions.Count - 1];
		Debug.Assert(iLInstruction.OpCode == ILOpCode.PUSHI_DWORD && iLInstruction.Operand is ILJumpTable);
		ILInstruction iLInstruction2 = new ILInstruction(ILOpCode.SWT)
		{
			Annotation = InstrAnnotation.JUMP
		};
		tr.Instructions.Add(iLInstruction2);
		ILJumpTable iLJumpTable = (ILJumpTable)iLInstruction.Operand;
		iLJumpTable.Chunk.runtime = tr.Runtime;
		iLJumpTable.RelativeBase = iLInstruction2;
		tr.Runtime.AddChunk(iLJumpTable.Chunk);
	}
}
