using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class NewobjHandler : ITranslationHandler
{
	public Code ILCode => Code.Newobj;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		InstrCallInfo instrCallInfo = new InstrCallInfo("NEWOBJ")
		{
			Method = (IMethod)expr.Operand
		};
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
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		tr.Instructions.Add(new IRInstruction(IROpCode.__NEWOBJ)
		{
			Operand1 = new IRMetaTarget(instrCallInfo.Method),
			Operand2 = iRVariable,
			Annotation = instrCallInfo
		});
		instrCallInfo.ReturnValue = iRVariable;
		tr.Instructions.Add(new IRInstruction(IROpCode.__ENDCALL)
		{
			Annotation = instrCallInfo
		});
		return iRVariable;
	}
}
