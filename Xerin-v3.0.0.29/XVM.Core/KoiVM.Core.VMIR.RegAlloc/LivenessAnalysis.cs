#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KoiVM.Core.AST.IR;
using KoiVM.Core.CFG;

namespace KoiVM.Core.VMIR.RegAlloc;

public class LivenessAnalysis
{
	private enum LiveFlags
	{
		GEN1 = 1,
		GEN2 = 2,
		KILL1 = 4,
		KILL2 = 8
	}

	private static readonly Dictionary<IROpCode, LiveFlags> opCodeLiveness = new Dictionary<IROpCode, LiveFlags>
	{
		{
			IROpCode.MOV,
			(LiveFlags)6
		},
		{
			IROpCode.POP,
			LiveFlags.KILL1
		},
		{
			IROpCode.PUSH,
			LiveFlags.GEN1
		},
		{
			IROpCode.CALL,
			(LiveFlags)9
		},
		{
			IROpCode.NOR,
			(LiveFlags)7
		},
		{
			IROpCode.CMP,
			(LiveFlags)3
		},
		{
			IROpCode.JZ,
			LiveFlags.GEN2
		},
		{
			IROpCode.JNZ,
			LiveFlags.GEN2
		},
		{
			IROpCode.SWT,
			LiveFlags.GEN2
		},
		{
			IROpCode.ADD,
			(LiveFlags)7
		},
		{
			IROpCode.SUB,
			(LiveFlags)7
		},
		{
			IROpCode.MUL,
			(LiveFlags)7
		},
		{
			IROpCode.DIV,
			(LiveFlags)7
		},
		{
			IROpCode.REM,
			(LiveFlags)7
		},
		{
			IROpCode.SHR,
			(LiveFlags)7
		},
		{
			IROpCode.SHL,
			(LiveFlags)7
		},
		{
			IROpCode.FCONV,
			(LiveFlags)6
		},
		{
			IROpCode.ICONV,
			(LiveFlags)6
		},
		{
			IROpCode.SX,
			(LiveFlags)6
		},
		{
			IROpCode.VCALL,
			LiveFlags.GEN1
		},
		{
			IROpCode.TRY,
			(LiveFlags)3
		},
		{
			IROpCode.LEAVE,
			LiveFlags.GEN1
		},
		{
			IROpCode.__EHRET,
			LiveFlags.GEN1
		},
		{
			IROpCode.__LEA,
			(LiveFlags)6
		},
		{
			IROpCode.__LDOBJ,
			(LiveFlags)9
		},
		{
			IROpCode.__STOBJ,
			(LiveFlags)3
		},
		{
			IROpCode.__GEN,
			LiveFlags.GEN1
		},
		{
			IROpCode.__KILL,
			LiveFlags.KILL1
		}
	};

	public static Dictionary<BasicBlock<IRInstrList>, BlockLiveness> ComputeLiveness(IList<BasicBlock<IRInstrList>> blocks)
	{
		Dictionary<BasicBlock<IRInstrList>, BlockLiveness> dictionary = new Dictionary<BasicBlock<IRInstrList>, BlockLiveness>();
		List<BasicBlock<IRInstrList>> list = blocks.Where((BasicBlock<IRInstrList> block) => block.Sources.Count == 0).ToList();
		List<BasicBlock<IRInstrList>> order = new List<BasicBlock<IRInstrList>>();
		HashSet<BasicBlock<IRInstrList>> visited = new HashSet<BasicBlock<IRInstrList>>();
		foreach (BasicBlock<IRInstrList> item in list)
		{
			PostorderTraversal(item, visited, delegate(BasicBlock<IRInstrList> block)
			{
				order.Add(block);
			});
		}
		bool flag = false;
		do
		{
			foreach (BasicBlock<IRInstrList> item2 in order)
			{
				BlockLiveness blockLiveness = BlockLiveness.Empty();
				foreach (BasicBlock<IRInstrList> target in item2.Targets)
				{
					if (dictionary.TryGetValue(target, out var value))
					{
						blockLiveness.OutLive.UnionWith(value.InLive);
					}
				}
				HashSet<IRVariable> hashSet = new HashSet<IRVariable>(blockLiveness.OutLive);
				for (int num = item2.Content.Count - 1; num >= 0; num--)
				{
					IRInstruction instr = item2.Content[num];
					ComputeInstrLiveness(instr, hashSet);
				}
				blockLiveness.InLive.UnionWith(hashSet);
				if (!flag && dictionary.TryGetValue(item2, out var value2))
				{
					flag = !value2.InLive.SetEquals(blockLiveness.InLive) || !value2.OutLive.SetEquals(blockLiveness.OutLive);
				}
				dictionary[item2] = blockLiveness;
			}
		}
		while (flag);
		return dictionary;
	}

	public static Dictionary<IRInstruction, HashSet<IRVariable>> ComputeLiveness(BasicBlock<IRInstrList> block, BlockLiveness liveness)
	{
		Dictionary<IRInstruction, HashSet<IRVariable>> dictionary = new Dictionary<IRInstruction, HashSet<IRVariable>>();
		HashSet<IRVariable> hashSet = new HashSet<IRVariable>(liveness.OutLive);
		for (int num = block.Content.Count - 1; num >= 0; num--)
		{
			IRInstruction iRInstruction = block.Content[num];
			ComputeInstrLiveness(iRInstruction, hashSet);
			dictionary[iRInstruction] = new HashSet<IRVariable>(hashSet);
		}
		Debug.Assert(hashSet.SetEquals(liveness.InLive));
		return dictionary;
	}

	private static void PostorderTraversal(BasicBlock<IRInstrList> block, HashSet<BasicBlock<IRInstrList>> visited, Action<BasicBlock<IRInstrList>> visitFunc)
	{
		visited.Add(block);
		foreach (BasicBlock<IRInstrList> target in block.Targets)
		{
			if (!visited.Contains(target))
			{
				PostorderTraversal(target, visited, visitFunc);
			}
		}
		visitFunc(block);
	}

	private static void ComputeInstrLiveness(IRInstruction instr, HashSet<IRVariable> live)
	{
		if (!opCodeLiveness.TryGetValue(instr.OpCode, out var value))
		{
			value = (LiveFlags)0;
		}
		IRVariable iRVariable = instr.Operand1 as IRVariable;
		IRVariable iRVariable2 = instr.Operand2 as IRVariable;
		if ((value & LiveFlags.KILL1) != 0 && iRVariable != null)
		{
			live.Remove(iRVariable);
		}
		if ((value & LiveFlags.KILL2) != 0 && iRVariable2 != null)
		{
			live.Remove(iRVariable2);
		}
		if ((value & LiveFlags.GEN1) != 0 && iRVariable != null)
		{
			live.Add(iRVariable);
		}
		if ((value & LiveFlags.GEN2) != 0 && iRVariable2 != null)
		{
			live.Add(iRVariable2);
		}
	}
}
