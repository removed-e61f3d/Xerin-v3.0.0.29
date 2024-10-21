#define DEBUG
using System;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;

namespace KoiVM.Core.VMIR.Translation;

public class RethrowHandler : ITranslationHandler
{
	public Code ILCode => Code.Rethrow;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 0);
		ScopeBlock[] array = tr.RootScope.SearchBlock(tr.Block);
		ScopeBlock scopeBlock = array[array.Length - 1];
		if (scopeBlock.Type != ScopeType.Handler || scopeBlock.ExceptionHandler.HandlerType != 0)
		{
			throw new InvalidProgramException();
		}
		IRVariable iRVariable = tr.Context.ResolveExceptionVar(scopeBlock.ExceptionHandler);
		Debug.Assert(iRVariable != null);
		int tHROW = tr.VM.Runtime.VMCall.THROW;
		tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, iRVariable));
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL)
		{
			Operand1 = IRConstant.FromI4(tHROW),
			Operand2 = IRConstant.FromI4(1)
		});
		return null;
	}
}
