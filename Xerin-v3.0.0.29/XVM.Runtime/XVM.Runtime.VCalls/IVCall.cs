using XVM.Runtime.Execution;

namespace XVM.Runtime.VCalls;

internal interface IVCall
{
	byte Code { get; }

	void Run(VMContext ctx, out ExecutionState state);
}
