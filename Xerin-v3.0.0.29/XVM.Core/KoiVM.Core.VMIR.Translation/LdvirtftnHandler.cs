#define DEBUG
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdvirtftnHandler : ITranslationHandler
{
	public Code ILCode => Code.Ldvirtftn;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		IIROperand op = tr.Translate(expr.Arguments[0]);
		IMethod memberRef = (IMethod)expr.Operand;
		int id = (int)tr.VM.Data.GetId(memberRef);
		int lDFTN = tr.VM.Runtime.VMCall.LDFTN;
		tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, op));
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(lDFTN), IRConstant.FromI4(id)));
		tr.Instructions.Add(new IRInstruction(IROpCode.POP, iRVariable));
		return iRVariable;
	}
}
