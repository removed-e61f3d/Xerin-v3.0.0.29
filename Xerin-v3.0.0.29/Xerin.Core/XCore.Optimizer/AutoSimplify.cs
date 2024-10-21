using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace XCore.Optimizer;

internal class AutoSimplify : IDisposable
{
	private CilBody _methodBody;

	private bool _optimizeOnDispose;

	public AutoSimplify(MethodDef methodBody, bool optimizeOnDispose)
	{
		_methodBody = methodBody.Body;
		_optimizeOnDispose = optimizeOnDispose;
		_methodBody.SimplifyMacros(methodBody.Parameters);
	}

	public void Dispose()
	{
		if (_optimizeOnDispose)
		{
			_methodBody.OptimizeMacros();
		}
	}
}
