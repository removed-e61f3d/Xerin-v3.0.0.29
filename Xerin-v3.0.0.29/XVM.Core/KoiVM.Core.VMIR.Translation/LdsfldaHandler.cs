using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdsfldaHandler : ITranslationHandler
{
	public Code ILCode => Code.Ldsflda;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		int value = (int)tr.VM.Data.GetId((IField)expr.Operand) | int.MinValue;
		int lDFLD = tr.VM.Runtime.VMCall.LDFLD;
		tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, IRConstant.Null()));
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(lDFLD), IRConstant.FromI4(value)));
		tr.Instructions.Add(new IRInstruction(IROpCode.POP, iRVariable));
		return iRVariable;
	}
}
