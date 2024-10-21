using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR;

public class IRContext
{
	private readonly IRVariable[] args;

	private readonly Dictionary<ExceptionHandler, IRVariable> ehVars;

	private readonly IRVariable[] locals;

	private readonly Dictionary<ILASTVariable, IRVariable> varMap = new Dictionary<ILASTVariable, IRVariable>();

	private readonly List<IRVariable> vRegs = new List<IRVariable>();

	public MethodDef Method { get; }

	public bool IsRuntime { get; set; }

	public IRContext(MethodDef method, CilBody body)
	{
		Method = method;
		IsRuntime = false;
		locals = new IRVariable[body.Variables.Count];
		for (int i = 0; i < locals.Length; i++)
		{
			if (body.Variables[i].Type.IsPinned)
			{
				throw new NotSupportedException("Pinned variables are not supported.");
			}
			locals[i] = new IRVariable
			{
				Id = i,
				Name = "local_" + i,
				Type = TypeInference.ToASTType(body.Variables[i].Type),
				RawType = body.Variables[i].Type,
				VariableType = IRVariableType.Local
			};
		}
		args = new IRVariable[method.Parameters.Count];
		for (int j = 0; j < args.Length; j++)
		{
			args[j] = new IRVariable
			{
				Id = j,
				Name = "arg_" + j,
				Type = TypeInference.ToASTType(method.Parameters[j].Type),
				RawType = method.Parameters[j].Type,
				VariableType = IRVariableType.Argument
			};
		}
		ehVars = new Dictionary<ExceptionHandler, IRVariable>();
		int num = -1;
		foreach (ExceptionHandler exceptionHandler in body.ExceptionHandlers)
		{
			num++;
			if (exceptionHandler.HandlerType != ExceptionHandlerType.Fault && exceptionHandler.HandlerType != ExceptionHandlerType.Finally)
			{
				TypeSig typeSig = exceptionHandler.CatchType.ToTypeSig();
				ehVars.Add(exceptionHandler, new IRVariable
				{
					Id = num,
					Name = "ex_" + num,
					Type = TypeInference.ToASTType(typeSig),
					RawType = typeSig,
					VariableType = IRVariableType.VirtualRegister
				});
			}
		}
	}

	public IRVariable AllocateVRegister(ASTType type)
	{
		IRVariable iRVariable = new IRVariable
		{
			Id = vRegs.Count,
			Name = "vreg_" + vRegs.Count,
			Type = type,
			VariableType = IRVariableType.VirtualRegister
		};
		vRegs.Add(iRVariable);
		return iRVariable;
	}

	public IRVariable ResolveVRegister(ILASTVariable variable)
	{
		if (variable.VariableType == ILASTVariableType.ExceptionVar)
		{
			return ResolveExceptionVar((ExceptionHandler)variable.Annotation);
		}
		if (varMap.TryGetValue(variable, out var value))
		{
			return value;
		}
		value = AllocateVRegister(variable.Type);
		varMap[variable] = value;
		return value;
	}

	public IRVariable ResolveParameter(Parameter param)
	{
		return args[param.Index];
	}

	public IRVariable ResolveLocal(Local local)
	{
		return locals[local.Index];
	}

	public IRVariable[] GetParameters()
	{
		return args;
	}

	public IRVariable ResolveExceptionVar(ExceptionHandler eh)
	{
		return ehVars[eh];
	}
}
