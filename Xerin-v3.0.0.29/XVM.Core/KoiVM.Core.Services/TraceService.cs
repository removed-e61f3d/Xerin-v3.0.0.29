using System;
using System.Collections.Generic;
using dnlib.DotNet;

namespace KoiVM.Core.Services;

internal class TraceService
{
	private readonly Dictionary<MethodDef, MethodTrace> cache = new Dictionary<MethodDef, MethodTrace>();

	public MethodTrace Trace(MethodDef method)
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		return cache.GetValueOrDefaultLazy(method, (MethodDef m) => cache[m] = new MethodTrace(m)).Trace();
	}
}
