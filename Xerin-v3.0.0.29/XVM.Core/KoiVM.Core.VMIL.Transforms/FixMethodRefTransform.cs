using System.Collections.Generic;
using KoiVM.Core.AST.IL;
using KoiVM.Core.VM;

namespace KoiVM.Core.VMIL.Transforms;

public class FixMethodRefTransform : IPostTransform
{
	private HashSet<VMRegisters> saveRegs;

	public void Initialize(ILPostTransformer tr)
	{
		saveRegs = tr.Runtime.Descriptor.Data.LookupInfo(tr.Method).UsedRegister;
	}

	public void Transform(ILPostTransformer tr)
	{
		tr.Instructions.VisitInstrs(VisitInstr, tr);
	}

	private void VisitInstr(ILInstrList instrs, ILInstruction instr, ref int index, ILPostTransformer tr)
	{
		if (instr.Operand is ILRelReference { Target: ILMethodTarget target })
		{
			target.Resolve(tr.Runtime);
		}
	}
}
