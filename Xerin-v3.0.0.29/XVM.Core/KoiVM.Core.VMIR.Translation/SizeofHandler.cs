using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class SizeofHandler : ITranslationHandler
{
	public Code ILCode => Code.Sizeof;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		int id = (int)tr.Runtime.Descriptor.Data.GetId((ITypeDefOrRef)expr.Operand);
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		int sIZEOF = tr.VM.Runtime.VMCall.SIZEOF;
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(sIZEOF), IRConstant.FromI4(id)));
		tr.Instructions.Add(new IRInstruction(IROpCode.POP, iRVariable));
		return iRVariable;
	}
}
