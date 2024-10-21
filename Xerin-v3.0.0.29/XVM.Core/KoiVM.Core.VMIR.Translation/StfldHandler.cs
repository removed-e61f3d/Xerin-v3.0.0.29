#define DEBUG
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class StfldHandler : ITranslationHandler
{
	public Code ILCode => Code.Stfld;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 2);
		IIROperand op = tr.Translate(expr.Arguments[0]);
		IIROperand op2 = tr.Translate(expr.Arguments[1]);
		int id = (int)tr.VM.Data.GetId((IField)expr.Operand);
		int sTFLD = tr.VM.Runtime.VMCall.STFLD;
		tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, op));
		tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, op2));
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(sTFLD), IRConstant.FromI4(id)));
		return null;
	}
}
