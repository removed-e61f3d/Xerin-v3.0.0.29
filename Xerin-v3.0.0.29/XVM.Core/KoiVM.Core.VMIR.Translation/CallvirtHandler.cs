using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class CallvirtHandler : ITranslationHandler
{
	public Code ILCode => Code.Callvirt;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		InstrCallInfo instrCallInfo = new InstrCallInfo("CALLVIRT")
		{
			Method = (IMethod)expr.Operand
		};
		if (expr.Prefixes != null && expr.Prefixes[0].OpCode == OpCodes.Constrained)
		{
			instrCallInfo.ConstrainType = (ITypeDefOrRef)expr.Prefixes[0].Operand;
		}
		tr.Instructions.Add(new IRInstruction(IROpCode.__BEGINCALL)
		{
			Annotation = instrCallInfo
		});
		IIROperand[] array = new IIROperand[expr.Arguments.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = tr.Translate(expr.Arguments[i]);
			tr.Instructions.Add(new IRInstruction(IROpCode.PUSH)
			{
				Operand1 = array[i],
				Annotation = instrCallInfo
			});
		}
		instrCallInfo.Arguments = array;
		IIROperand iIROperand = null;
		if (expr.Type.HasValue)
		{
			iIROperand = tr.Context.AllocateVRegister(expr.Type.Value);
			tr.Instructions.Add(new IRInstruction(IROpCode.__CALLVIRT)
			{
				Operand1 = new IRMetaTarget(instrCallInfo.Method),
				Operand2 = iIROperand,
				Annotation = instrCallInfo
			});
		}
		else
		{
			tr.Instructions.Add(new IRInstruction(IROpCode.__CALLVIRT)
			{
				Operand1 = new IRMetaTarget(instrCallInfo.Method),
				Annotation = instrCallInfo
			});
		}
		instrCallInfo.ReturnValue = iIROperand;
		tr.Instructions.Add(new IRInstruction(IROpCode.__ENDCALL)
		{
			Annotation = instrCallInfo
		});
		return iIROperand;
	}
}
