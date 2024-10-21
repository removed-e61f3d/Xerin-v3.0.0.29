using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;

namespace KoiVM.Core.VMIR.Translation;

public class EndfinallyHandler : ITranslationHandler
{
	public Code ILCode => Code.Endfinally;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		tr.Instructions.Add(new IRInstruction(IROpCode.__EHRET));
		tr.Block.Flags |= BlockFlags.ExitEHReturn;
		return null;
	}
}
