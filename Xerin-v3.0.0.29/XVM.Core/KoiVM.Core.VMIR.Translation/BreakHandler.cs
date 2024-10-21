using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class BreakHandler : ITranslationHandler
{
	public Code ILCode => Code.Break;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		int bREAK = tr.VM.Runtime.VMCall.BREAK;
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(bREAK)));
		return null;
	}
}
