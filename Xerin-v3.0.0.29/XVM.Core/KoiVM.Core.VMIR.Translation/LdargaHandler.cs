using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdargaHandler : ITranslationHandler
{
	public Code ILCode => Code.Ldarga;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		IRVariable op = tr.Context.ResolveParameter((Parameter)expr.Operand);
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.ByRef);
		tr.Instructions.Add(new IRInstruction(IROpCode.__LEA, iRVariable, op));
		return iRVariable;
	}
}
