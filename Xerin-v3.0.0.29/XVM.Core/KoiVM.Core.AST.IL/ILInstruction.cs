using System.Text;
using KoiVM.Core.AST.IR;
using KoiVM.Core.VMIL;

namespace KoiVM.Core.AST.IL;

public class ILInstruction : ASTNode, IHasOffset
{
	public uint Offset { get; set; }

	public IRInstruction IR { get; set; }

	public ILOpCode OpCode { get; set; }

	public IILOperand Operand { get; set; }

	public object Annotation { get; set; }

	public ILInstruction(ILOpCode opCode)
	{
		OpCode = opCode;
	}

	public ILInstruction(ILOpCode opCode, IILOperand operand)
	{
		OpCode = opCode;
		Operand = operand;
	}

	public ILInstruction(ILOpCode opCode, IILOperand operand, object annotation)
	{
		OpCode = opCode;
		Operand = operand;
		Annotation = annotation;
	}

	public ILInstruction(ILOpCode opCode, IILOperand operand, ILInstruction origin)
	{
		OpCode = opCode;
		Operand = operand;
		Annotation = origin.Annotation;
		IR = origin.IR;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("{0}", OpCode.ToString().PadLeft(16));
		if (Operand != null)
		{
			stringBuilder.AppendFormat(" {0}", Operand);
		}
		if (Annotation != null)
		{
			stringBuilder.AppendFormat("    ; {0}", Annotation);
		}
		return stringBuilder.ToString();
	}
}
