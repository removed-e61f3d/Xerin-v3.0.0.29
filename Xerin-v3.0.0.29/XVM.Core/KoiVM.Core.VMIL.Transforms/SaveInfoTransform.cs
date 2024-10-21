using KoiVM.Core.AST.IL;
using KoiVM.Core.VM;

namespace KoiVM.Core.VMIL.Transforms;

public class SaveInfoTransform : ITransform
{
	private VMMethodInfo methodInfo;

	public void Initialize(ILTransformer tr)
	{
		methodInfo = tr.VM.Data.LookupInfo(tr.Method);
		methodInfo.RootScope = tr.RootScope;
		tr.VM.Data.SetInfo(tr.Method, methodInfo);
	}

	public void Transform(ILTransformer tr)
	{
		tr.Instructions.VisitInstrs(VisitInstr, tr);
	}

	private void VisitInstr(ILInstrList instrs, ILInstruction instr, ref int index, ILTransformer tr)
	{
		if (instr.Operand is ILRegister)
		{
			VMRegisters register = ((ILRegister)instr.Operand).Register;
			if (register.IsGPR())
			{
				methodInfo.UsedRegister.Add(register);
			}
		}
	}
}
