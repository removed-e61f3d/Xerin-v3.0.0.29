#define DEBUG
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class UnboxHandler : ITranslationHandler
{
	public Code ILCode => Code.Unbox;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand op = tr.Translate(expr.Arguments[0]);
		TypeSig typeSig = ((ITypeDefOrRef)expr.Operand).ToTypeSig();
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		int value = (int)tr.VM.Data.GetId((ITypeDefOrRef)expr.Operand) | int.MinValue;
		int uNBOX = tr.VM.Runtime.VMCall.UNBOX;
		tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, op));
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(uNBOX), IRConstant.FromI4(value)));
		tr.Instructions.Add(new IRInstruction(IROpCode.POP, iRVariable));
		return iRVariable;
	}
}
