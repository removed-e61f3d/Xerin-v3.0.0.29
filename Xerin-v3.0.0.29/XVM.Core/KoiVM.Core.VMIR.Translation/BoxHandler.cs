#define DEBUG
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.AST.IR;

namespace KoiVM.Core.VMIR.Translation;

public class BoxHandler : ITranslationHandler
{
	public Code ILCode => Code.Box;

	public IIROperand Translate(ILASTExpression expr, IRTranslator tr)
	{
		Debug.Assert(expr.Arguments.Length == 1);
		IIROperand iIROperand = tr.Translate(expr.Arguments[0]);
		TypeSig typeSig = ((ITypeDefOrRef)expr.Operand).ToTypeSig();
		TypeDef typeDef = ((ITypeDefOrRef)expr.Operand).ResolveTypeDef();
		if (!typeSig.GetElementType().IsPrimitive() && (typeDef == null || !typeDef.IsEnum) && typeSig.ElementType != ElementType.String)
		{
			return iIROperand;
		}
		IRVariable iRVariable = tr.Context.AllocateVRegister(expr.Type.Value);
		int id = (int)tr.VM.Data.GetId((ITypeDefOrRef)expr.Operand);
		int bOX = tr.VM.Runtime.VMCall.BOX;
		tr.Instructions.Add(new IRInstruction(IROpCode.PUSH, iIROperand));
		tr.Instructions.Add(new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(bOX), IRConstant.FromI4(id)));
		tr.Instructions.Add(new IRInstruction(IROpCode.POP, iRVariable));
		return iRVariable;
	}
}
