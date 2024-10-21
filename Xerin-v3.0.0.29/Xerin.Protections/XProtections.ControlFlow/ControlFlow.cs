using System;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Pdb;
using XCore.Context;
using XCore.Protections;
using XCore.Utils;

namespace XProtections.ControlFlow;

public class ControlFlow : Protection
{
	public static bool isPerformance;

	public static bool isStrong;

	public override string name => "Control Flow";

	public override int number => 4;

	public override void Execute(XContext context)
	{
		MethodDef methodDef = context.Module.GlobalType.FindOrCreateStaticConstructor();
		foreach (TypeDef type in context.Module.GetTypes())
		{
			if (type.Namespace == "Costura")
			{
				continue;
			}
			foreach (MethodDef method in type.Methods)
			{
				if (method.HasBody && method.Body.HasInstructions && method.ReturnType != null && !method.MethodHasL2FAttribute() && methodDef != method)
				{
					IMethod operand = context.Module.Import(typeof(Debug).GetMethod("Assert", new Type[1] { typeof(bool) }));
					method.Body.Instructions.Insert(0, new Instruction(OpCodes.Call, operand));
					method.Body.Instructions.Insert(0, new Instruction(OpCodes.Ldc_I4_1));
					if (isPerformance)
					{
						PhasePerfControlFlow(method, context);
					}
					else
					{
						PhaseControlFlow(method, context);
					}
				}
			}
		}
	}

	public static void PhasePerfControlFlow(MethodDef method, XContext context)
	{
		CilBody body = method.Body;
		body.SimplifyBranches();
		BlockParser.ScopeBlock scopeBlock = BlockParser.ParseBody(body);
		new SwitchMangler2().Mangle(body, scopeBlock, context, method, method.ReturnType);
		body.Instructions.Clear();
		scopeBlock.ToBody(body);
		if (body.PdbMethod != null)
		{
			body.PdbMethod = new PdbMethod
			{
				Scope = new PdbScope
				{
					Start = body.Instructions.First(),
					End = body.Instructions.Last()
				}
			};
		}
		method.CustomDebugInfos.RemoveWhere((PdbCustomDebugInfo cdi) => cdi is PdbStateMachineHoistedLocalScopesCustomDebugInfo);
		foreach (ExceptionHandler exceptionHandler in body.ExceptionHandlers)
		{
			int num = body.Instructions.IndexOf(exceptionHandler.TryEnd) + 1;
			exceptionHandler.TryEnd = ((num < body.Instructions.Count) ? body.Instructions[num] : null);
			num = body.Instructions.IndexOf(exceptionHandler.HandlerEnd) + 1;
			exceptionHandler.HandlerEnd = ((num < body.Instructions.Count) ? body.Instructions[num] : null);
		}
	}

	public static void PhaseControlFlow(MethodDef method, XContext context)
	{
		CilBody body = method.Body;
		body.SimplifyBranches();
		BlockParser.ScopeBlock scopeBlock = BlockParser.ParseBody(body);
		new SwitchMangler().Mangle(body, scopeBlock, context, method, method.ReturnType);
		body.Instructions.Clear();
		scopeBlock.ToBody(body);
		if (body.PdbMethod != null)
		{
			body.PdbMethod = new PdbMethod
			{
				Scope = new PdbScope
				{
					Start = body.Instructions.First(),
					End = body.Instructions.Last()
				}
			};
		}
		method.CustomDebugInfos.RemoveWhere((PdbCustomDebugInfo cdi) => cdi is PdbStateMachineHoistedLocalScopesCustomDebugInfo);
		foreach (ExceptionHandler exceptionHandler in body.ExceptionHandlers)
		{
			int num = body.Instructions.IndexOf(exceptionHandler.TryEnd) + 1;
			exceptionHandler.TryEnd = ((num < body.Instructions.Count) ? body.Instructions[num] : null);
			num = body.Instructions.IndexOf(exceptionHandler.HandlerEnd) + 1;
			exceptionHandler.HandlerEnd = ((num < body.Instructions.Count) ? body.Instructions[num] : null);
		}
	}
}
