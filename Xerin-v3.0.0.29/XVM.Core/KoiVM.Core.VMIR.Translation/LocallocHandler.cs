#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LocallocHandler : ITranslationHandler
{
	public Code ILCode => Code.Localloc;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand op = tr.Translate(expr.Arguments[0]);
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		int lOCALLOC = tr.VM.Runtime.VMCall.LOCALLOC;
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(lOCALLOC), op));
		tr.Instructions.Add(new IRInstruction(IROpCode.POP, iRVariable));
		return iRVariable;
	}
}
