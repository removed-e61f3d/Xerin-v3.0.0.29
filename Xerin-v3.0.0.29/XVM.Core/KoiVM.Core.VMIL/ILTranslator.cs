using System;
using System.Collections.Generic;
using KoiVM.Core.AST.IL;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;
using KoiVM.Core.RT;
using KoiVM.Core.VM;
using KoiVM.Core.VMIR;

namespace KoiVM.Core.VMIL;

public class ILTranslator
{
	private static readonly Dictionary<IROpCode, ITranslationHandler> handlers;

	public VMRuntime Runtime { get; }

	public VMDescriptor VM => Runtime.Descriptor;

	internal ILInstrList Instructions { get; private set; }

	static ILTranslator()
	{
		handlers = new Dictionary<IROpCode, ITranslationHandler>();
		Type[] exportedTypes = typeof(ILTranslator).Assembly.GetExportedTypes();
		foreach (Type type in exportedTypes)
		{
			if (typeof(ITranslationHandler).IsAssignableFrom(type) && !type.IsAbstract)
			{
				ITranslationHandler translationHandler = (ITranslationHandler)Activator.CreateInstance(type);
				handlers.Add(translationHandler.IRCode, translationHandler);
			}
		}
	}

	public ILTranslator(VMRuntime runtime)
	{
		Runtime = runtime;
	}

	public ILInstrList Translate(IRInstrList instrs)
	{
		Instructions = new ILInstrList();
		int i = 0;
		foreach (IRInstruction instr in instrs)
		{
			if (!handlers.TryGetValue(instr.OpCode, out var value))
			{
				throw new NotSupportedException(instr.OpCode.ToString());
			}
			try
			{
				value.Translate(instr, this);
			}
			catch (Exception innerException)
			{
				throw new Exception($"Failed to translate ir {instr.ILAST}.", innerException);
			}
			for (; i < Instructions.Count; i++)
			{
				Instructions[i].IR = instr;
			}
		}
		ILInstrList instructions = Instructions;
		Instructions = null;
		return instructions;
	}

	public void Translate(ScopeBlock rootScope)
	{
		Dictionary<BasicBlock<IRInstrList>, BasicBlock<ILInstrList>> blockMap = rootScope.UpdateBasicBlocks((BasicBlock<IRInstrList> block) => Translate(block.Content), (int id, ILInstrList content) => new ILBlock(id, content));
		rootScope.ProcessBasicBlocks(delegate(BasicBlock<ILInstrList> block)
		{
			foreach (ILInstruction item in block.Content)
			{
				if (item.Operand is ILBlockTarget)
				{
					ILBlockTarget iLBlockTarget = (ILBlockTarget)item.Operand;
					iLBlockTarget.Target = blockMap[(BasicBlock<IRInstrList>)iLBlockTarget.Target];
				}
				else if (item.Operand is ILJumpTable)
				{
					ILJumpTable iLJumpTable = (ILJumpTable)item.Operand;
					for (int i = 0; i < iLJumpTable.Targets.Length; i++)
					{
						iLJumpTable.Targets[i] = blockMap[(BasicBlock<IRInstrList>)iLJumpTable.Targets[i]];
					}
				}
			}
		});
	}
}
