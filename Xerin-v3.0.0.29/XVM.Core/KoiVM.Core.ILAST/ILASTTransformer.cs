using System;
using System.Collections.Generic;
using dnlib.DotNet;
using KoiVM.Core.AST.ILAST;
using KoiVM.Core.CFG;
using KoiVM.Core.ILAST.Transformation;
using KoiVM.Core.RT;
using KoiVM.Core.VM;

namespace KoiVM.Core.ILAST;

public class ILASTTransformer
{
	private ITransformationHandler[] pipeline;

	public MethodDef Method { get; private set; }

	public ScopeBlock RootScope { get; private set; }

	public VMRuntime Runtime { get; private set; }

	public VMDescriptor VM => Runtime.Descriptor;

	internal Dictionary<object, object> Annotations { get; private set; }

	internal BasicBlock<ILASTTree> Block { get; private set; }

	internal ILASTTree Tree => Block.Content;

	public ILASTTransformer(MethodDef method, ScopeBlock rootScope, VMRuntime runtime)
	{
		method.Body.SimplifyMacros(method.Parameters);
		method.Body.SimplifyBranches();
		RootScope = rootScope;
		Method = method;
		Runtime = runtime;
		Annotations = new Dictionary<object, object>();
		InitPipeline();
	}

	private void InitPipeline()
	{
		pipeline = new ITransformationHandler[7]
		{
			new VariableInlining(),
			new StringTransform(),
			new ArrayTransform(),
			new IndirectTransform(),
			new ILASTTypeInference(),
			new NullTransform(),
			new BranchTransform()
		};
	}

	public void Transform()
	{
		if (pipeline == null)
		{
			throw new InvalidOperationException("Transformer already used.");
		}
		ITransformationHandler[] array = pipeline;
		foreach (ITransformationHandler handler in array)
		{
			handler.Initialize(this);
			RootScope.ProcessBasicBlocks(delegate(BasicBlock<ILASTTree> block)
			{
				Block = block;
				handler.Transform(this);
			});
		}
		pipeline = null;
	}
}
