using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdftnHandler : ITranslationHandler
{
	public Code ILCode => Code.Ldftn;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		int lDFTN = tr.VM.Runtime.VMCall.LDFTN;
		int id = (int)tr.VM.Data.GetId((IMethod)expr.Operand);
		tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.FromI4(0)));
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(lDFTN), IRConstant.FromI4(id)));
		tr.Instructions.Add(new IRInstruction(IROpCode.POP, iRVariable));
		return iRVariable;
	}
}
