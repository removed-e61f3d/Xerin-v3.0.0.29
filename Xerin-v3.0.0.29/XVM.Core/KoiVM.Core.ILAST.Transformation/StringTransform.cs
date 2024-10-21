using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.ILAST;

namespace KoiVM.Core.ILAST.Transformation;

public class StringTransform : ITransformationHandler
{
	public void Initialize(ILASTTransformer tr)
	{
	}

	public void Transform(ILASTTransformer tr)
	{
		tr.Tree.TraverseTree(Transform, tr);
	}

	private static void Transform(ILASTExpression expr, ILASTTransformer tr)
	{
		if (expr.ILCode == Code.Ldstr)
		{
			string str = (string)expr.Operand;
			expr.ILCode = Code.Box;
			expr.Operand = tr.Method.Module.CorLibTypes.String.ToTypeDefOrRef();
			expr.Arguments = new IILASTNode[1]
			{
				new ILASTExpression
				{
					ILCode = Code.Ldc_I4,
					Operand = (int)tr.VM.Data.GetId(str),
					Arguments = new IILASTNode[0]
				}
			};
		}
	}
}
