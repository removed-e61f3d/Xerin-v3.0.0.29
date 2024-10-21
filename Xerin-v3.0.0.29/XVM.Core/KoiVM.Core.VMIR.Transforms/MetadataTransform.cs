using System;
using dnlib.DotNet;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Transforms;

public class MetadataTransform : ITransform
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
		instr.Operand1 = TransformMD(instr.Operand1, tr);
		instr.Operand2 = TransformMD(instr.Operand2, tr);
	}

	private IIROperand TransformMD(IIROperand operand, IRTransformer tr)
	{
		if (operand is IRMetaTarget)
		{
			IRMetaTarget iRMetaTarget = (IRMetaTarget)operand;
			if (!iRMetaTarget.LateResolve)
			{
				if (!(iRMetaTarget.MetadataItem is IMemberRef))
				{
					throw new NotSupportedException();
				}
				return IRConstant.FromI4((int)tr.VM.Data.GetId((IMemberRef)iRMetaTarget.MetadataItem));
			}
		}
		return operand;
	}
}
