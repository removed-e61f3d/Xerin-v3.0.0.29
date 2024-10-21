using System;
using dnlib.DotNet;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;
using KoiVM.Core.RT;

namespace KoiVM.Core.VMIL;

public static class TranslationHelpers
{
	public static ILOpCode GetLIND(ASTType type, TypeSig rawType)
	{
		if (rawType != null)
		{
			switch (rawType.ElementType)
			{
			case ElementType.Boolean:
			case ElementType.I1:
			case ElementType.U1:
				return ILOpCode.LIND_BYTE;
			case ElementType.Char:
			case ElementType.I2:
			case ElementType.U2:
				return ILOpCode.LIND_WORD;
			case ElementType.I4:
			case ElementType.U4:
			case ElementType.R4:
				return ILOpCode.LIND_DWORD;
			case ElementType.I8:
			case ElementType.U8:
			case ElementType.R8:
				return ILOpCode.LIND_QWORD;
			case ElementType.Ptr:
			case ElementType.I:
			case ElementType.U:
				return ILOpCode.LIND_PTR;
			default:
				return ILOpCode.LIND_OBJECT;
			}
		}
		switch (type)
		{
		case ASTType.I4:
		case ASTType.R4:
			return ILOpCode.LIND_DWORD;
		case ASTType.I8:
		case ASTType.R8:
			return ILOpCode.LIND_QWORD;
		case ASTType.Ptr:
			return ILOpCode.LIND_PTR;
		default:
			return ILOpCode.LIND_OBJECT;
		}
	}

	public static ILOpCode GetLIND(this IRRegister reg)
	{
		return GetLIND(reg.Type, (reg.SourceVariable == null) ? null : reg.SourceVariable.RawType);
	}

	public static ILOpCode GetLIND(this IRPointer ptr)
	{
		return GetLIND(ptr.Type, (ptr.SourceVariable == null) ? null : ptr.SourceVariable.RawType);
	}

	public static ILOpCode GetSIND(ASTType type, TypeSig rawType)
	{
		if (rawType != null)
		{
			switch (rawType.ElementType)
			{
			case ElementType.Boolean:
			case ElementType.I1:
			case ElementType.U1:
				return ILOpCode.SIND_BYTE;
			case ElementType.Char:
			case ElementType.I2:
			case ElementType.U2:
				return ILOpCode.SIND_WORD;
			case ElementType.I4:
			case ElementType.U4:
			case ElementType.R4:
				return ILOpCode.SIND_DWORD;
			case ElementType.I8:
			case ElementType.U8:
			case ElementType.R8:
				return ILOpCode.SIND_QWORD;
			case ElementType.Ptr:
			case ElementType.I:
			case ElementType.U:
				return ILOpCode.SIND_PTR;
			default:
				return ILOpCode.SIND_OBJECT;
			}
		}
		switch (type)
		{
		case ASTType.I4:
		case ASTType.R4:
			return ILOpCode.SIND_DWORD;
		case ASTType.I8:
		case ASTType.R8:
			return ILOpCode.SIND_QWORD;
		case ASTType.Ptr:
			return ILOpCode.SIND_PTR;
		default:
			return ILOpCode.SIND_OBJECT;
		}
	}

	public static ILOpCode GetSIND(this IRRegister reg)
	{
		return GetSIND(reg.Type, (reg.SourceVariable == null) ? null : reg.SourceVariable.RawType);
	}

	public static ILOpCode GetSIND(this IRPointer ptr)
	{
		return GetSIND(ptr.Type, (ptr.SourceVariable == null) ? null : ptr.SourceVariable.RawType);
	}

	public static ILOpCode GetPUSHR(ASTType type, TypeSig rawType)
	{
		if (rawType != null)
		{
			switch (rawType.ElementType)
			{
			case ElementType.Boolean:
			case ElementType.I1:
			case ElementType.U1:
				return ILOpCode.PUSHR_BYTE;
			case ElementType.Char:
			case ElementType.I2:
			case ElementType.U2:
				return ILOpCode.PUSHR_WORD;
			case ElementType.I4:
			case ElementType.U4:
			case ElementType.R4:
				return ILOpCode.PUSHR_DWORD;
			case ElementType.I8:
			case ElementType.U8:
			case ElementType.R8:
			case ElementType.Ptr:
				return ILOpCode.PUSHR_QWORD;
			default:
				return ILOpCode.PUSHR_OBJECT;
			}
		}
		switch (type)
		{
		case ASTType.I4:
		case ASTType.R4:
			return ILOpCode.PUSHR_DWORD;
		case ASTType.I8:
		case ASTType.R8:
		case ASTType.Ptr:
			return ILOpCode.PUSHR_QWORD;
		default:
			return ILOpCode.PUSHR_OBJECT;
		}
	}

	public static ILOpCode GetPUSHR(this IRRegister reg)
	{
		return GetPUSHR(reg.Type, (reg.SourceVariable == null) ? null : reg.SourceVariable.RawType);
	}

	public static ILOpCode GetPUSHR(this IRPointer ptr)
	{
		return GetPUSHR(ptr.Type, (ptr.SourceVariable == null) ? null : ptr.SourceVariable.RawType);
	}

	public static ILOpCode GetPUSHI(this ASTType type)
	{
		switch (type)
		{
		case ASTType.I4:
		case ASTType.R4:
			return ILOpCode.PUSHI_DWORD;
		case ASTType.I8:
		case ASTType.R8:
		case ASTType.Ptr:
			return ILOpCode.PUSHI_QWORD;
		default:
			throw new NotSupportedException();
		}
	}

	public static void PushOperand(this ILTranslator tr, IIROperand operand)
	{
		if (operand is IRRegister)
		{
			ILRegister operand2 = ILRegister.LookupRegister(((IRRegister)operand).Register);
			tr.Instructions.Add(new ILInstruction(((IRRegister)operand).GetPUSHR(), operand2));
		}
		else if (operand is IRPointer)
		{
			IRPointer iRPointer = (IRPointer)operand;
			ILRegister operand3 = ILRegister.LookupRegister(iRPointer.Register.Register);
			tr.Instructions.Add(new ILInstruction(iRPointer.Register.GetPUSHR(), operand3));
			if (iRPointer.Offset != 0)
			{
				tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, ILImmediate.Create(iRPointer.Offset, ASTType.I4)));
				if (iRPointer.Register.Type == ASTType.I4)
				{
					tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_DWORD));
				}
				else
				{
					tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_QWORD));
				}
			}
			tr.Instructions.Add(new ILInstruction(iRPointer.GetLIND()));
		}
		else if (operand is IRConstant)
		{
			IRConstant iRConstant = (IRConstant)operand;
			if (iRConstant.Value == null)
			{
				tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, ILImmediate.Create(0, ASTType.O)));
			}
			else
			{
				tr.Instructions.Add(new ILInstruction(iRConstant.Type.Value.GetPUSHI(), ILImmediate.Create(iRConstant.Value, iRConstant.Type.Value)));
			}
		}
		else if (operand is IRMetaTarget)
		{
			MethodDef target = (MethodDef)((IRMetaTarget)operand).MetadataItem;
			tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, new ILMethodTarget(target)));
		}
		else if (operand is IRBlockTarget)
		{
			IBasicBlock target2 = ((IRBlockTarget)operand).Target;
			tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, new ILBlockTarget(target2)));
		}
		else if (operand is IRJumpTable)
		{
			IBasicBlock[] targets = ((IRJumpTable)operand).Targets;
			tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, new ILJumpTable(targets)));
		}
		else
		{
			if (!(operand is IRDataTarget))
			{
				throw new NotSupportedException();
			}
			BinaryChunk target3 = ((IRDataTarget)operand).Target;
			tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, new ILDataTarget(target3)));
		}
	}

	public static void PopOperand(this ILTranslator tr, IIROperand operand)
	{
		if (operand is IRRegister)
		{
			ILRegister operand2 = ILRegister.LookupRegister(((IRRegister)operand).Register);
			tr.Instructions.Add(new ILInstruction(ILOpCode.POP, operand2));
			return;
		}
		if (operand is IRPointer)
		{
			IRPointer iRPointer = (IRPointer)operand;
			ILRegister operand3 = ILRegister.LookupRegister(iRPointer.Register.Register);
			tr.Instructions.Add(new ILInstruction(iRPointer.Register.GetPUSHR(), operand3));
			if (iRPointer.Offset != 0)
			{
				tr.Instructions.Add(new ILInstruction(ILOpCode.PUSHI_DWORD, ILImmediate.Create(iRPointer.Offset, ASTType.I4)));
				if (iRPointer.Register.Type == ASTType.I4)
				{
					tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_DWORD));
				}
				else
				{
					tr.Instructions.Add(new ILInstruction(ILOpCode.ADD_QWORD));
				}
			}
			tr.Instructions.Add(new ILInstruction(iRPointer.GetSIND()));
			return;
		}
		throw new NotSupportedException();
	}
}
