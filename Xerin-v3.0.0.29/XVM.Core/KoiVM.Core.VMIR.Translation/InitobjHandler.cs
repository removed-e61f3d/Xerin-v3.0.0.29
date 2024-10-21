#define DEBUG
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class InitobjHandler : ITranslationHandler
{
	public Code ILCode => Code.Initobj;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand op = tr.Translate(expr.Arguments[0]);
		int id = (int)tr.VM.Data.GetId((ITypeDefOrRef)expr.Operand);
		int iNITOBJ = tr.VM.Runtime.VMCall.INITOBJ;
		tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, op));
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(iNITOBJ), IRConstant.FromI4(id)));
		return null;
	}
}
