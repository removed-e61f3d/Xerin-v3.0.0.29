using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdtokenHandler : ITranslationHandler
{
	public Code ILCode => Code.Ldtoken;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		int id = (int)tr.VM.Data.GetId((IMemberRef)expr.Operand);
		int tOKEN = tr.VM.Runtime.VMCall.TOKEN;
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(tOKEN), IRConstant.FromI4(id)));
		tr.Instructions.Add(new IRInstruction(IROpCode.POP, iRVariable));
		return iRVariable;
	}
}
