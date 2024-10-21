using dnlib.DotNet.Emit;

namespace KoiVM.Core.AST;

public class EHInfo : InstrAnnotation
{
	public ExceptionHandler ExceptionHandler { get; set; }

	public EHInfo(ExceptionHandler eh)
		: base("EH_" + eh.GetHashCode())
	{
		ExceptionHandler = eh;
	}
}
