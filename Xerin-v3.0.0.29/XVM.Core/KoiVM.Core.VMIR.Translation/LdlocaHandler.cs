using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdlocaHandler : ITranslationHandler
{
	public Code ILCode => Code.Ldloca;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		IRVariable op = tr.Context.ResolveLocal((Local)expr.Operand);
		IRVariable iRVariable = tr.Context.AllocateVRegister(ASTType.ByRef);
		tr.Instructions.Add(new IRInstruction(IROpCode.__LEA, iRVariable, op));
		return iRVariable;
	}
}
