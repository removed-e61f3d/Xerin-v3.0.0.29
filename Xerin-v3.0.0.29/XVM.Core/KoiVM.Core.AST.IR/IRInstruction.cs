using System.Text;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.AST.IR;

public class IRInstruction : ASTNode
{
	public IROpCode OpCode { get; set; }

	public IILASTStatement ILAST { get; set; }

	public IIROperand Operand1 { get; set; }

	public IIROperand Operand2 { get; set; }

	public object Annotation { get; set; }

	public IRInstruction(IROpCode opCode)
	{
		OpCode = opCode;
	}

	public IRInstruction(IROpCode opCode, IIROperand op1)
	{
		OpCode = opCode;
		Operand1 = op1;
	}

	public IRInstruction(IROpCode opCode, IIROperand op1, IIROperand op2)
	{
		OpCode = opCode;
		Operand1 = op1;
		Operand2 = op2;
	}

	public IRInstruction(IROpCode opCode, IIROperand op1, IIROperand op2, object annotation)
	{
		OpCode = opCode;
		Operand1 = op1;
		Operand2 = op2;
		Annotation = annotation;
	}

	public IRInstruction(IROpCode opCode, IIROperand op1, IIROperand op2, IRInstruction origin)
	{
		OpCode = opCode;
		Operand1 = op1;
		Operand2 = op2;
		Annotation = origin.Annotation;
		ILAST = origin.ILAST;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("{0}", OpCode.ToString().PadLeft(16));
		if (Operand1 != null)
		{
			stringBuilder.AppendFormat(" {0}", Operand1);
			if (Operand2 != null)
			{
				stringBuilder.AppendFormat(", {0}", Operand2);
			}
		}
		if (Annotation != null)
		{
			stringBuilder.AppendFormat("    ; {0}", Annotation);
		}
		return stringBuilder.ToString();
	}
}
