using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class LdargHandler : ITranslationHandler
{
	public Code ILCode => Code.Ldarg;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		IRVariable iRVariable = tr.Context.ResolveParameter((Parameter)expr.Operand);
		IRVariable iRVariable2 = tr.Context.AllocateVRegister(iRVariable.Type);
		tr.Instructions.Add(new IRInstruction(IROpCode.MOV, iRVariable2, iRVariable));
		if (iRVariable.RawType.ElementType == ElementType.I1 || iRVariable.RawType.ElementType == ElementType.I2)
		{
			iRVariable2.RawType = iRVariable.RawType;
			IRVariable iRVariable3 = tr.Context.AllocateVRegister(iRVariable.Type);
			tr.Instructions.Add(new IRInstruction(IROpCode.SX, iRVariable3, iRVariable2));
			iRVariable2 = iRVariable3;
		}
		return iRVariable2;
	}
}
