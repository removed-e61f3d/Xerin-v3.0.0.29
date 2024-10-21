#define DEBUG
using System.Diagnostics;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR;

public static class TranslationHelpers
{
	public static void EmitCompareEq(IRTranslator tr, ASTType type, IIROperand a, IIROperand b)
	{
		if (type == ASTType.O || type == ASTType.ByRef || type == ASTType.R4 || type == ASTType.R8)
		{
			tr.Instructions.Add(new IRInstruction(IROpCode.CMP, a, b));
			return;
		}
		Debug.Assert(type == ASTType.I4 || type == ASTType.I8 || type == ASTType.Ptr);
		tr.Instructions.Add(new IRInstruction(IROpCode.CMP, a, b));
	}
}
