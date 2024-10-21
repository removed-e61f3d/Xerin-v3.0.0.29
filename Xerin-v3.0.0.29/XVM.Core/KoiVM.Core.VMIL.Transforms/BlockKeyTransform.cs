using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet.Emit;
using KoiVM.Core.AST.IL;
using KoiVM.Core.CFG;
using KoiVM.Core.RT;
using KoiVM.Core.VM;

namespace KoiVM.Core.VMIL.Transforms;

public class BlockKeyTransform : IPostTransform
{
	private struct BlockKey
	{
		public uint Entry;

		public uint Exit;
	}

	private class FinallyInfo
	{
		public readonly HashSet<ILBlock> FinallyEnds = new HashSet<ILBlock>();

		public readonly HashSet<ILBlock> TryEndNexts = new HashSet<ILBlock>();
	}

	private class EHMap
	{
		public readonly Dictionary<ExceptionHandler, FinallyInfo> Finally = new Dictionary<ExceptionHandler, FinallyInfo>();

		public readonly HashSet<ILBlock> Starts = new HashSet<ILBlock>();
	}

	private Dictionary<ILBlock, BlockKey> Keys;

	private VMMethodInfo methodInfo;

	private VMRuntime runtime;

	public void Initialize(ILPostTransformer tr)
	{
		runtime = tr.Runtime;
		methodInfo = tr.Runtime.Descriptor.Data.LookupInfo(tr.Method);
		ComputeBlockKeys(tr.RootScope);
	}

	public void Transform(ILPostTransformer tr)
	{
		BlockKey blockKey = Keys[tr.Block];
		methodInfo.BlockKeys[tr.Block] = new VMBlockKey
		{
			EntryKey = (byte)blockKey.Entry,
			ExitKey = (byte)blockKey.Exit
		};
	}

	private void ComputeBlockKeys(ScopeBlock rootScope)
	{
		List<ILBlock> list = rootScope.GetBasicBlocks().OfType<ILBlock>().ToList();
		uint id = 1u;
		Keys = list.ToDictionary((ILBlock block) => block, delegate
		{
			BlockKey result = default(BlockKey);
			result.Entry = id++;
			result.Exit = id++;
			return result;
		});
		EHMap map = MapEHs(rootScope);
		bool updated;
		do
		{
			updated = false;
			BlockKey value = Keys[list[0]];
			value.Entry = 4294967294u;
			Keys[list[0]] = value;
			value = Keys[list[list.Count - 1]];
			value.Exit = 4294967293u;
			Keys[list[list.Count - 1]] = value;
			foreach (ILBlock item in list)
			{
				value = Keys[item];
				if (item.Sources.Count > 0)
				{
					uint num = item.Sources.Select((BasicBlock<ILInstrList> b) => Keys[(ILBlock)b].Exit).Max();
					if (value.Entry != num)
					{
						value.Entry = num;
						updated = true;
					}
				}
				if (item.Targets.Count > 0)
				{
					uint num2 = item.Targets.Select((BasicBlock<ILInstrList> b) => Keys[(ILBlock)b].Entry).Max();
					if (value.Exit != num2)
					{
						value.Exit = num2;
						updated = true;
					}
				}
				Keys[item] = value;
			}
			MatchHandlers(map, ref updated);
		}
		while (updated);
		Dictionary<uint, uint> dictionary = new Dictionary<uint, uint>();
		dictionary[uint.MaxValue] = 0u;
		dictionary[4294967294u] = methodInfo.EntryKey;
		dictionary[4294967293u] = methodInfo.ExitKey;
		foreach (ILBlock item2 in list)
		{
			BlockKey value2 = Keys[item2];
			uint entry = value2.Entry;
			if (!dictionary.TryGetValue(entry, out value2.Entry))
			{
				uint entry2 = (dictionary[entry] = runtime.Descriptor.RandomGenerator.NextByte());
				value2.Entry = entry2;
			}
			uint exit = value2.Exit;
			if (!dictionary.TryGetValue(exit, out value2.Exit))
			{
				uint entry2 = (dictionary[exit] = runtime.Descriptor.RandomGenerator.NextByte());
				value2.Exit = entry2;
			}
			Keys[item2] = value2;
		}
	}

	private EHMap MapEHs(ScopeBlock rootScope)
	{
		EHMap eHMap = new EHMap();
		MapEHsInternal(rootScope, eHMap);
		return eHMap;
	}

	private void MapEHsInternal(ScopeBlock scope, EHMap map)
	{
		if (scope.Type == ScopeType.Filter)
		{
			map.Starts.Add((ILBlock)scope.GetBasicBlocks().First());
		}
		else if (scope.Type != 0)
		{
			if (scope.ExceptionHandler.HandlerType == ExceptionHandlerType.Finally)
			{
				if (!map.Finally.TryGetValue(scope.ExceptionHandler, out var value))
				{
					value = (map.Finally[scope.ExceptionHandler] = new FinallyInfo());
				}
				if (scope.Type == ScopeType.Try)
				{
					HashSet<IBasicBlock> scopeBlocks = new HashSet<IBasicBlock>(scope.GetBasicBlocks());
					foreach (ILBlock item in scopeBlocks)
					{
						if ((item.Flags & BlockFlags.ExitEHLeave) == 0 || (item.Targets.Count != 0 && !item.Targets.Any((BasicBlock<ILInstrList> target) => !scopeBlocks.Contains(target))))
						{
							continue;
						}
						foreach (BasicBlock<ILInstrList> target in item.Targets)
						{
							value.TryEndNexts.Add((ILBlock)target);
						}
					}
				}
				else if (scope.Type == ScopeType.Handler)
				{
					IEnumerable<IBasicBlock> enumerable = ((scope.Children.Count <= 0) ? scope.Content : scope.Children.Where((ScopeBlock s) => s.Type == ScopeType.None).SelectMany((ScopeBlock s) => s.GetBasicBlocks()));
					foreach (ILBlock item2 in enumerable)
					{
						if ((item2.Flags & BlockFlags.ExitEHReturn) != 0 && item2.Targets.Count == 0)
						{
							value.FinallyEnds.Add(item2);
						}
					}
				}
			}
			if (scope.Type == ScopeType.Handler)
			{
				map.Starts.Add((ILBlock)scope.GetBasicBlocks().First());
			}
		}
		foreach (ScopeBlock child in scope.Children)
		{
			MapEHsInternal(child, map);
		}
	}

	private void MatchHandlers(EHMap map, ref bool updated)
	{
		foreach (ILBlock start in map.Starts)
		{
			BlockKey value = Keys[start];
			if (value.Entry != uint.MaxValue)
			{
				value.Entry = uint.MaxValue;
				Keys[start] = value;
				updated = true;
			}
		}
		foreach (FinallyInfo value4 in map.Finally.Values)
		{
			uint val = value4.FinallyEnds.Max((ILBlock block) => Keys[block].Exit);
			uint val2 = value4.TryEndNexts.Max((ILBlock block) => Keys[block].Entry);
			uint num = Math.Max(val, val2);
			foreach (ILBlock finallyEnd in value4.FinallyEnds)
			{
				BlockKey value2 = Keys[finallyEnd];
				if (value2.Exit != num)
				{
					value2.Exit = num;
					Keys[finallyEnd] = value2;
					updated = true;
				}
			}
			foreach (ILBlock tryEndNext in value4.TryEndNexts)
			{
				BlockKey value3 = Keys[tryEndNext];
				if (value3.Entry != num)
				{
					value3.Entry = num;
					Keys[tryEndNext] = value3;
					updated = true;
				}
			}
		}
	}
}
