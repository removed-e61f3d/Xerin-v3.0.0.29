using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Transforms;

public class InitLocalTransform : ITransform
{
	private bool done;

	public void Initialize(IRTransformer tr)
	{
	}

	public void Transform(IRTransformer tr)
	{
		if (tr.Context.Method.Body.InitLocals)
		{
			tr.Instructions.VisitInstrs(VisitInstr, tr);
		}
	}

	private void VisitInstr(IRInstrList instrs, IRInstruction instr, ref int index, IRTransformer tr)
	{
		if (instr.OpCode != IROpCode.__ENTRY || done)
		{
			return;
		}
		List<IRInstruction> list = new List<IRInstruction>();
		list.Add(instr);
		foreach (Local variable in tr.Context.Method.Body.Variables)
		{
			if (variable.Type.IsValueType && !variable.Type.IsPrimitive)
			{
				IRVariable op = tr.Context.AllocateVRegister(ASTType.ByRef);
				list.Add(new IRInstruction(IROpCode.__LEA, op, tr.Context.ResolveLocal(variable)));
				int id = (int)tr.VM.Data.GetId(variable.Type.RemovePinnedAndModifiers().ToTypeDefOrRef());
				int iNITOBJ = tr.VM.Runtime.VMCall.INITOBJ;
				list.Add(new IRInstruction(IROpCode.PUSH, op));
				list.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(iNITOBJ), IRConstant.FromI4(id)));
			}
		}
		instrs.Replace(index, list);
		done = true;
	}
}
