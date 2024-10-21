using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;

namespace KoiVM.Core.VMIR.Translation;

public class LeaveHandler : ITranslationHandler
{
	public Code ILCode => Code.Leave;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		tr.Instructions.Add(new IRInstruction(IROpCode.__LEAVE)
		{
			Operand1 = new IRBlockTarget((IBasicBlock)expr.Operand)
		});
		tr.Block.Flags |= BlockFlags.ExitEHLeave;
		return null;
	}
}
