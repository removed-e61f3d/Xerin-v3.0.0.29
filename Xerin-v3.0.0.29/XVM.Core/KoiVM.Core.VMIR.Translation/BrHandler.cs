using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;

namespace KoiVM.Core.VMIR.Translation;

public class BrHandler : ITranslationHandler
{
	public Code ILCode => Code.Br;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		tr.Instructions.Add(new IRInstruction(IROpCode.JMP)
		{
			Operand1 = new IRBlockTarget((IBasicBlock)expr.Operand)
		});
		return null;
	}
}
