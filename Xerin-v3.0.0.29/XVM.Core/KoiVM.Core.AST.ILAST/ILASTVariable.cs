namespace KoiVM.Core.AST.ILAST;

public class ILASTVariable : ASTVariable, IILASTNode
{
	ASTType? IILASTNode.Type => base.Type;

	public ILASTVariableType VariableType { get; set; }

	public object Annotation { get; set; }
}
