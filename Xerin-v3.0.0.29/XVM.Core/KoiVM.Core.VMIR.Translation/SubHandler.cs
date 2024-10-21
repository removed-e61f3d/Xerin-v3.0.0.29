#define DEBUG
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class SubHandler : ITranslationHandler
{
	public Code ILCode => Code.Sub;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 2);
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		if (expr.Type.HasValue && (expr.Type.Value == ASTType.R4 || expr.Type.Value == ASTType.R8))
		{
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = iRVariable,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.SUB)
			{
				Operand1 = iRVariable,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
		}
		else
		{
			IRVariable iRVariable2 = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = iRVariable,
				Operand2 = tr.Translate(expr.Arguments[0])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.MOV)
			{
				Operand1 = iRVariable2,
				Operand2 = tr.Translate(expr.Arguments[1])
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.__NOT)
			{
				Operand1 = iRVariable2
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.ADD)
			{
				Operand1 = iRVariable,
				Operand2 = iRVariable2
			});
			tr.Instructions.Add(new IRInstruction(IROpCode.ADD)
			{
				Operand1 = iRVariable,
				Operand2 = IRConstant.FromI4(1)
			});
		}
		return iRVariable;
	}
}
