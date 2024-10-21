#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;

namespace KoiVM.Core.VMIR.Transforms;

public class EHTransform : ITransform
{
	private ScopeBlock[] thisScopes;

	public void Initialize(IRTransformer tr)
	{
	}

	public void Transform(IRTransformer tr)
	{
		thisScopes = tr.RootScope.SearchBlock(tr.Block);
		AddTryStart(tr);
		if (thisScopes[thisScopes.Length - 1].Type == ScopeType.Handler)
		{
			ScopeBlock tryScope = SearchForTry(tr.RootScope, thisScopes[thisScopes.Length - 1].ExceptionHandler);
			ScopeBlock[] source = tr.RootScope.SearchBlock(tryScope.GetBasicBlocks().First());
			thisScopes = source.TakeWhile((ScopeBlock s) => s != tryScope).ToArray();
		}
		tr.Instructions.VisitInstrs(VisitInstr, tr);
	}

	private void SearchForHandlers(ScopeBlock scope, ExceptionHandler eh, ref IBasicBlock handler, ref IBasicBlock filter)
	{
		if (scope.ExceptionHandler == eh)
		{
			if (scope.Type == ScopeType.Handler)
			{
				handler = scope.GetBasicBlocks().First();
			}
			else if (scope.Type == ScopeType.Filter)
			{
				filter = scope.GetBasicBlocks().First();
			}
		}
		foreach (ScopeBlock child in scope.Children)
		{
			SearchForHandlers(child, eh, ref handler, ref filter);
		}
	}

	private void AddTryStart(IRTransformer tr)
	{
		List<IRInstruction> list = new List<IRInstruction>();
		for (int i = 0; i < thisScopes.Length; i++)
		{
			ScopeBlock scopeBlock = thisScopes[i];
			if (scopeBlock.Type != ScopeType.Try || scopeBlock.GetBasicBlocks().First() != tr.Block)
			{
				continue;
			}
			IBasicBlock handler = null;
			IBasicBlock filter = null;
			SearchForHandlers(tr.RootScope, scopeBlock.ExceptionHandler, ref handler, ref filter);
			Debug.Assert(handler != null && (scopeBlock.ExceptionHandler.HandlerType != ExceptionHandlerType.Filter || filter != null));
			list.Add(new IRInstruction(IROpCode.PUSH, new IRBlockTarget(handler)));
			IIROperand op = null;
			int value;
			if (scopeBlock.ExceptionHandler.HandlerType == ExceptionHandlerType.Catch)
			{
				op = IRConstant.FromI4((int)tr.VM.Data.GetId(scopeBlock.ExceptionHandler.CatchType));
				value = tr.VM.Runtime.RTFlags.EH_CATCH;
			}
			else if (scopeBlock.ExceptionHandler.HandlerType == ExceptionHandlerType.Filter)
			{
				op = new IRBlockTarget(filter);
				value = tr.VM.Runtime.RTFlags.EH_FILTER;
			}
			else if (scopeBlock.ExceptionHandler.HandlerType == ExceptionHandlerType.Fault)
			{
				value = tr.VM.Runtime.RTFlags.EH_FAULT;
			}
			else
			{
				if (scopeBlock.ExceptionHandler.HandlerType != ExceptionHandlerType.Finally)
				{
					throw new InvalidProgramException();
				}
				value = tr.VM.Runtime.RTFlags.EH_FINALLY;
			}
			list.Add(new IRInstruction(IROpCode.TRY, IRConstant.FromI4(value), op)
			{
				Annotation = new EHInfo(scopeBlock.ExceptionHandler)
			});
		}
		tr.Instructions.InsertRange(0, list);
	}

	private ScopeBlock SearchForTry(ScopeBlock scope, ExceptionHandler eh)
	{
		if (scope.ExceptionHandler == eh && scope.Type == ScopeType.Try)
		{
			return scope;
		}
		foreach (ScopeBlock child in scope.Children)
		{
			ScopeBlock scopeBlock = SearchForTry(child, eh);
			if (scopeBlock != null)
			{
				return scopeBlock;
			}
		}
		return null;
	}

	private static ScopeBlock FindCommonAncestor(ScopeBlock[] a, ScopeBlock[] b)
	{
		ScopeBlock result = null;
		for (int i = 0; i < a.Length && i < b.Length && a[i] == b[i]; i++)
		{
			result = a[i];
		}
		return result;
	}

	private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
	{
		if (instr.OpCode != IROpCode.__LEAVE)
		{
			return;
		}
		ScopeBlock[] b = tr.RootScope.SearchBlock(((IRBlockTarget)instr.Operand1).Target);
		ScopeBlock scopeBlock = FindCommonAncestor(thisScopes, b);
		List<IRInstruction> list = new List<IRInstruction>();
		int num = thisScopes.Length - 1;
		while (num >= 0 && thisScopes[num] != scopeBlock)
		{
			if (thisScopes[num].Type == ScopeType.Try)
			{
				IBasicBlock handler = null;
				IBasicBlock filter = null;
				SearchForHandlers(tr.RootScope, thisScopes[num].ExceptionHandler, ref handler, ref filter);
				if (handler == null)
				{
					throw new InvalidProgramException();
				}
				list.Add(new IRInstruction(IROpCode.LEAVE, new IRBlockTarget(handler))
				{
					Annotation = new EHInfo(thisScopes[num].ExceptionHandler)
				});
			}
			num--;
		}
		instr.OpCode = IROpCode.JMP;
		list.Add(instr);
		instrs.Replace(index, list);
	}
}
