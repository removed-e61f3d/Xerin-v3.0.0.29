using System;
using System.IO;
using dnlib.DotNet;
using KoiVM.Core.CFG;
using KoiVM.Core.ILAST;
using KoiVM.Core.RT;
using KoiVM.Core.VMIL;
using KoiVM.Core.VMIR;

namespace KoiVM.Core;

public class MethodVirtualizer
{
	protected VMRuntime Runtime { get; private set; }

	protected MethodDef Method { get; private set; }

	protected ScopeBlock RootScope { get; private set; }

	protected IRContext IRContext { get; private set; }

	public MethodVirtualizer(VMRuntime runtime)
	{
		Runtime = runtime;
	}

	public ScopeBlock Run(MethodDef method)
	{
		try
		{
			Method = method;
			Init();
			BuildILAST();
			TransformILAST();
			BuildVMIR();
			TransformVMIR();
			BuildVMIL();
			TransformVMIL();
			Deinitialize();
			ScopeBlock rootScope = RootScope;
			RootScope = null;
			Method = null;
			return rootScope;
		}
		catch (Exception ex)
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			using (StreamWriter streamWriter = new StreamWriter(folderPath + "\\error.txt", append: true))
			{
				streamWriter.WriteLine($"Error processing method: {method.Name} & {method.MDToken}");
				streamWriter.WriteLine("Exception: " + ex.ToString());
				streamWriter.WriteLine();
			}
			RootScope = null;
			Method = null;
			return null;
		}
	}

	protected virtual void Init()
	{
		RootScope = BlockParser.Parse(Method, Method.Body);
		IRContext = new IRContext(Method, Method.Body);
	}

	protected virtual void BuildILAST()
	{
		ILASTBuilder.BuildAST(Method, Method.Body, RootScope);
	}

	protected virtual void TransformILAST()
	{
		ILASTTransformer iLASTTransformer = new ILASTTransformer(Method, RootScope, Runtime);
		iLASTTransformer.Transform();
	}

	protected virtual void BuildVMIR()
	{
		IRTranslator iRTranslator = new IRTranslator(IRContext, Runtime);
		iRTranslator.Translate(RootScope);
	}

	protected virtual void TransformVMIR()
	{
		IRTransformer iRTransformer = new IRTransformer(RootScope, IRContext, Runtime);
		iRTransformer.Transform();
	}

	protected virtual void BuildVMIL()
	{
		ILTranslator iLTranslator = new ILTranslator(Runtime);
		iLTranslator.Translate(RootScope);
	}

	protected virtual void TransformVMIL()
	{
		ILTransformer iLTransformer = new ILTransformer(Method, RootScope, Runtime);
		iLTransformer.Transform();
	}

	protected virtual void Deinitialize()
	{
		IRContext = null;
		Runtime.AddMethod(Method, RootScope);
		Runtime.ExportMethod(Method);
	}
}
