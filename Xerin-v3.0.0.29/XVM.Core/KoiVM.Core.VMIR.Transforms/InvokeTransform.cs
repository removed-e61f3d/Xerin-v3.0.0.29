using System.Collections.Generic;
using dnlib.DotNet;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VM;

namespace KoiVM.Core.VMIR.Transforms;

public class InvokeTransform : ITransform
{
	public void Initialize(IRTransformer tr)
	{
	}

	public void Transform(IRTransformer tr)
	{
		tr.Instructions.VisitInstrs(VisitInstr, tr);
	}

	private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
	{
		if (instr.OpCode == IROpCode.__CALL || instr.OpCode == IROpCode.__CALLVIRT || instr.OpCode == IROpCode.__NEWOBJ)
		{
			MethodDef methodDef = ((IMethod)((IRMetaTarget)instr.Operand1).MetadataItem).ResolveMethodDef();
			InstrCallInfo instrCallInfo = (InstrCallInfo)instr.Annotation;
			if (methodDef == null || methodDef.Module != tr.Context.Method.Module || !tr.VM.Settings.IsVirtualized(methodDef) || instr.OpCode != IROpCode.__CALL)
			{
				instrCallInfo.IsECall = true;
				ProcessECall(instrs, instr, index, tr);
			}
			else
			{
				instrCallInfo.IsECall = false;
				ProcessDCall(instrs, instr, index, tr, methodDef);
			}
		}
	}

	private void ProcessECall(IRInstrList instrs, IRInstruction instr, int index, IRTransformer tr)
	{
		IMethod memberRef = (IMethod)((IRMetaTarget)instr.Operand1).MetadataItem;
		IRVariable iRVariable = (IRVariable)instr.Operand2;
		uint num = 0u;
		ITypeDefOrRef constrainType = ((InstrCallInfo)instr.Annotation).ConstrainType;
		if (instr.OpCode == IROpCode.__CALL)
		{
			num = tr.VM.Runtime.VCallOps.ECALL_CALL;
		}
		else if (instr.OpCode == IROpCode.__CALLVIRT)
		{
			num = ((constrainType == null) ? tr.VM.Runtime.VCallOps.ECALL_CALLVIRT : tr.VM.Runtime.VCallOps.ECALL_CALLVIRT_CONSTRAINED);
		}
		else if (instr.OpCode == IROpCode.__NEWOBJ)
		{
			num = tr.VM.Runtime.VCallOps.ECALL_NEWOBJ;
		}
		int value = (int)(tr.VM.Data.GetId(memberRef) | (num << 30));
		int eCALL = tr.VM.Runtime.VMCall.ECALL;
		List<IRInstruction> list = new List<IRInstruction>();
		if (constrainType != null)
		{
			list.Add(new IRInstruction(IROpCode.PUSH)
			{
				Operand1 = IRConstant.FromI4((int)tr.VM.Data.GetId(constrainType)),
				Annotation = instr.Annotation,
				ILAST = instr.ILAST
			});
		}
		list.Add(new IRInstruction(IROpCode.VCALL)
		{
			Operand1 = IRConstant.FromI4(eCALL),
			Operand2 = IRConstant.FromI4(value),
			Annotation = instr.Annotation,
			ILAST = instr.ILAST
		});
		if (iRVariable != null)
		{
			list.Add(new IRInstruction(IROpCode.POP, iRVariable)
			{
				Annotation = instr.Annotation,
				ILAST = instr.ILAST
			});
		}
		instrs.Replace(index, list);
	}

	private void ProcessDCall(IRInstrList instrs, IRInstruction instr, int index, IRTransformer tr, MethodDef method)
	{
		IRVariable iRVariable = (IRVariable)instr.Operand2;
		InstrCallInfo instrCallInfo = (InstrCallInfo)instr.Annotation;
		instrCallInfo.Method = method;
		List<IRInstruction> list = new List<IRInstruction>();
		list.Add(new IRInstruction(IROpCode.CALL, new IRMetaTarget(method)
		{
			LateResolve = true
		})
		{
			Annotation = instr.Annotation,
			ILAST = instr.ILAST
		});
		if (iRVariable != null)
		{
			list.Add(new IRInstruction(IROpCode.MOV, iRVariable, new IRRegister(VMRegisters.R0, iRVariable.Type))
			{
				Annotation = instr.Annotation,
				ILAST = instr.ILAST
			});
		}
		int value = -instrCallInfo.Arguments.Length;
		list.Add(new IRInstruction(IROpCode.ADD, IRRegister.SP, IRConstant.FromI4(value))
		{
			Annotation = instr.Annotation,
			ILAST = instr.ILAST
		});
		instrs.Replace(index, list);
	}
}
