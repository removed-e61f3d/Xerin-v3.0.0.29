#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class AddOvfUnHandler : ITranslationHandler
{
	public Code ILCode => Code.Add_Ovf_Un;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 2);
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
		{
			Operand1 = iRVariable,
			Operand2 = tr.Translate(expr.Arguments[0])
		});
		tr.Instructions.Add(new IRInstruction(IROpCode.ADD)
		{
			Operand1 = iRVariable,
			Operand2 = tr.Translate(expr.Arguments[1])
		});
		int cKOVERFLOW = tr.VM.Runtime.VMCall.CKOVERFLOW;
		IRVariable iRVariable2 = tr.Context.AllocateVRegister(expr.Type.Value);
		tr.Instructions.Add(new IRInstruction(IROpCode.__GETF)
		{
			Operand1 = iRVariable2,
			Operand2 = IRConstant.FromI4(1 << tr.Arch.Flags.CARRY)
		});
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(cKOVERFLOW), iRVariable2));
		return iRVariable;
	}
}
