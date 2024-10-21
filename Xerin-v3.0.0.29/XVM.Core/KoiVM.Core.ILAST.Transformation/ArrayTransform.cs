using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST;
using KoiVM.Core.AST.ILAST;

namespace KoiVM.Core.ILAST.Transformation;

public class ArrayTransform : ITransformationHandler
{
	public void Initialize(ILASTTransformer tr)
	{
	}

	public void Transform(ILASTTransformer tr)
	{
		ModuleDef module = tr.Method.Module;
		tr.Tree.TraverseTree(Transform, module);
		for (int i = 0; i < tr.Tree.Count; i++)
		{
			IILASTStatement node = tr.Tree[i];
			ILASTExpression expression = VariableInlining.GetExpression(node);
			if (expression != null)
			{
				switch (expression.ILCode)
				{
				case Code.Stelem:
					TransformSTELEM(expression, module, (ITypeDefOrRef)expression.Operand, tr.Tree, ref i);
					break;
				case Code.Stelem_I1:
					TransformSTELEM(expression, module, module.CorLibTypes.SByte.ToTypeDefOrRef(), tr.Tree, ref i);
					break;
				case Code.Stelem_I2:
					TransformSTELEM(expression, module, module.CorLibTypes.Int16.ToTypeDefOrRef(), tr.Tree, ref i);
					break;
				case Code.Stelem_I4:
					TransformSTELEM(expression, module, module.CorLibTypes.Int32.ToTypeDefOrRef(), tr.Tree, ref i);
					break;
				case Code.Stelem_I8:
					TransformSTELEM(expression, module, module.CorLibTypes.Int64.ToTypeDefOrRef(), tr.Tree, ref i);
					break;
				case Code.Stelem_R4:
					TransformSTELEM(expression, module, module.CorLibTypes.Single.ToTypeDefOrRef(), tr.Tree, ref i);
					break;
				case Code.Stelem_R8:
					TransformSTELEM(expression, module, module.CorLibTypes.Double.ToTypeDefOrRef(), tr.Tree, ref i);
					break;
				case Code.Stelem_I:
					TransformSTELEM(expression, module, module.CorLibTypes.IntPtr.ToTypeDefOrRef(), tr.Tree, ref i);
					break;
				case Code.Stelem_Ref:
					TransformSTELEM(expression, module, module.CorLibTypes.Object.ToTypeDefOrRef(), tr.Tree, ref i);
					break;
				}
			}
		}
	}

	private static void Transform(ILASTExpression expr, ModuleDef module)
	{
		switch (expr.ILCode)
		{
		case Code.Ldlen:
		{
			expr.ILCode = Code.Call;
			TypeRef typeRef = module.CorLibTypes.GetTypeRef("System", "Array");
			MethodSig sig3 = MethodSig.CreateInstance(module.CorLibTypes.Int32);
			MemberRefUser operand3 = new MemberRefUser(module, "get_Length", sig3, typeRef);
			expr.Operand = operand3;
			break;
		}
		case Code.Newarr:
		{
			expr.ILCode = Code.Newobj;
			ITypeDefOrRef class2 = new SZArraySig(((ITypeDefOrRef)expr.Operand).ToTypeSig()).ToTypeDefOrRef();
			MethodSig sig2 = MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.Int32);
			MemberRefUser operand2 = new MemberRefUser(module, ".ctor", sig2, class2);
			expr.Operand = operand2;
			break;
		}
		case Code.Ldelema:
		{
			expr.ILCode = Code.Call;
			TypeSig nextSig = ((ITypeDefOrRef)expr.Operand).ToTypeSig();
			ITypeDefOrRef @class = new SZArraySig(nextSig).ToTypeDefOrRef();
			MethodSig sig = MethodSig.CreateInstance(new ByRefSig(nextSig), module.CorLibTypes.Int32);
			MemberRefUser operand = new MemberRefUser(module, "Address", sig, @class);
			expr.Operand = operand;
			break;
		}
		case Code.Ldelem:
			TransformLDELEM(expr, module, (ITypeDefOrRef)expr.Operand);
			break;
		case Code.Ldelem_I1:
			TransformLDELEM(expr, module, module.CorLibTypes.SByte.ToTypeDefOrRef());
			break;
		case Code.Ldelem_U1:
			TransformLDELEM(expr, module, module.CorLibTypes.Byte.ToTypeDefOrRef());
			break;
		case Code.Ldelem_I2:
			TransformLDELEM(expr, module, module.CorLibTypes.Int16.ToTypeDefOrRef());
			break;
		case Code.Ldelem_U2:
			TransformLDELEM(expr, module, module.CorLibTypes.UInt16.ToTypeDefOrRef());
			break;
		case Code.Ldelem_I4:
			TransformLDELEM(expr, module, module.CorLibTypes.Int32.ToTypeDefOrRef());
			break;
		case Code.Ldelem_U4:
			TransformLDELEM(expr, module, module.CorLibTypes.UInt32.ToTypeDefOrRef());
			break;
		case Code.Ldelem_I8:
			TransformLDELEM(expr, module, module.CorLibTypes.Int64.ToTypeDefOrRef());
			break;
		case Code.Ldelem_R4:
			TransformLDELEM(expr, module, module.CorLibTypes.Single.ToTypeDefOrRef());
			break;
		case Code.Ldelem_R8:
			TransformLDELEM(expr, module, module.CorLibTypes.Double.ToTypeDefOrRef());
			break;
		case Code.Ldelem_I:
			TransformLDELEM(expr, module, module.CorLibTypes.IntPtr.ToTypeDefOrRef());
			break;
		case Code.Ldelem_Ref:
			TransformLDELEM(expr, module, module.CorLibTypes.Object.ToTypeDefOrRef());
			break;
		case Code.Stelem_I:
		case Code.Stelem_I1:
		case Code.Stelem_I2:
		case Code.Stelem_I4:
		case Code.Stelem_I8:
		case Code.Stelem_R4:
		case Code.Stelem_R8:
		case Code.Stelem_Ref:
			break;
		}
	}

	private static void TransformLDELEM(ILASTExpression expr, ModuleDef module, ITypeDefOrRef type)
	{
		TypeRef typeRef = module.CorLibTypes.GetTypeRef("System", "Array");
		MethodSig sig = MethodSig.CreateInstance(module.CorLibTypes.Object, module.CorLibTypes.Int32);
		MemberRefUser operand = new MemberRefUser(module, "GetValue", sig, typeRef);
		ILASTExpression iLASTExpression = new ILASTExpression
		{
			ILCode = Code.Call,
			Operand = operand,
			Arguments = expr.Arguments
		};
		expr.ILCode = Code.Unbox_Any;
		expr.Operand = (type.IsValueType ? module.CorLibTypes.Object.ToTypeDefOrRef() : type);
		expr.Type = TypeInference.ToASTType(type.ToTypeSig());
		expr.Arguments = new IILASTNode[1] { iLASTExpression };
	}

	private static void TransformSTELEM(ILASTExpression expr, ModuleDef module, ITypeDefOrRef type, ILASTTree tree, ref int index)
	{
		TypeRef typeRef = module.CorLibTypes.GetTypeRef("System", "Array");
		MethodSig sig = MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.Object, module.CorLibTypes.Int32);
		MemberRefUser operand = new MemberRefUser(module, "SetValue", sig, typeRef);
		ILASTVariable iLASTVariable;
		if (expr.Arguments[1] is ILASTVariable)
		{
			iLASTVariable = (ILASTVariable)expr.Arguments[1];
		}
		else
		{
			iLASTVariable = new ILASTVariable
			{
				Name = $"arr_{expr.CILInstr.Offset:x4}_1",
				VariableType = ILASTVariableType.StackVar
			};
			tree.Insert(index++, new ILASTAssignment
			{
				Variable = iLASTVariable,
				Value = (ILASTExpression)expr.Arguments[1]
			});
		}
		ILASTVariable iLASTVariable2;
		if (expr.Arguments[2] is ILASTVariable)
		{
			iLASTVariable2 = (ILASTVariable)expr.Arguments[2];
		}
		else
		{
			iLASTVariable2 = new ILASTVariable
			{
				Name = $"arr_{expr.CILInstr.Offset:x4}_2",
				VariableType = ILASTVariableType.StackVar
			};
			tree.Insert(index++, new ILASTAssignment
			{
				Variable = iLASTVariable2,
				Value = (ILASTExpression)expr.Arguments[2]
			});
		}
		if (type.IsPrimitive)
		{
			ILASTExpression iLASTExpression = new ILASTExpression();
			iLASTExpression.ILCode = Code.Box;
			iLASTExpression.Operand = type;
			IILASTNode[] arguments = new ILASTVariable[1] { iLASTVariable2 };
			iLASTExpression.Arguments = arguments;
			ILASTExpression iLASTExpression2 = iLASTExpression;
			expr.Arguments[2] = iLASTVariable;
			expr.Arguments[1] = iLASTExpression2;
		}
		else
		{
			expr.Arguments[2] = iLASTVariable;
			expr.Arguments[1] = iLASTVariable2;
		}
		expr.ILCode = Code.Call;
		expr.Operand = operand;
	}
}
