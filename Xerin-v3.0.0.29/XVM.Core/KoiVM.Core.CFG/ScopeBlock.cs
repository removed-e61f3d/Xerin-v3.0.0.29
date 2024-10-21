using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet.Emit;

namespace KoiVM.Core.CFG;

public class ScopeBlock
{
	public ScopeType Type { get; private set; }

	public ExceptionHandler ExceptionHandler { get; private set; }

	public IList<ScopeBlock> Children { get; private set; }

	public IList<IBasicBlock> Content { get; private set; }

	public ScopeBlock()
	{
		Type = ScopeType.None;
		ExceptionHandler = null;
		Children = new List<ScopeBlock>();
		Content = new List<IBasicBlock>();
	}

	public ScopeBlock(ScopeType type, ExceptionHandler eh)
	{
		if (type == ScopeType.None)
		{
			throw new ArgumentException("type");
		}
		Type = type;
		ExceptionHandler = eh;
		Children = new List<ScopeBlock>();
		Content = new List<IBasicBlock>();
	}

	public IEnumerable<IBasicBlock> GetBasicBlocks()
	{
		Validate();
		if (Content.Count > 0)
		{
			return Content;
		}
		return Children.SelectMany((ScopeBlock child) => child.GetBasicBlocks());
	}

	public Dictionary<BasicBlock<TOld>, BasicBlock<TNew>> UpdateBasicBlocks<TOld, TNew>(Func<BasicBlock<TOld>, TNew> updateFunc)
	{
		return UpdateBasicBlocks(updateFunc, (int id, TNew content) => new BasicBlock<TNew>(id, content));
	}

	public Dictionary<BasicBlock<TOld>, BasicBlock<TNew>> UpdateBasicBlocks<TOld, TNew>(Func<BasicBlock<TOld>, TNew> updateFunc, Func<int, TNew, BasicBlock<TNew>> factoryFunc)
	{
		Dictionary<BasicBlock<TOld>, BasicBlock<TNew>> dictionary = new Dictionary<BasicBlock<TOld>, BasicBlock<TNew>>();
		UpdateBasicBlocksInternal(updateFunc, dictionary, factoryFunc);
		foreach (KeyValuePair<BasicBlock<TOld>, BasicBlock<TNew>> item in dictionary)
		{
			foreach (BasicBlock<TOld> source in item.Key.Sources)
			{
				item.Value.Sources.Add(dictionary[source]);
			}
			foreach (BasicBlock<TOld> target in item.Key.Targets)
			{
				item.Value.Targets.Add(dictionary[target]);
			}
		}
		return dictionary;
	}

	private void UpdateBasicBlocksInternal<TOld, TNew>(Func<BasicBlock<TOld>, TNew> updateFunc, Dictionary<BasicBlock<TOld>, BasicBlock<TNew>> blockMap, Func<int, TNew, BasicBlock<TNew>> factoryFunc)
	{
		Validate();
		if (Content.Count > 0)
		{
			for (int i = 0; i < Content.Count; i++)
			{
				BasicBlock<TOld> basicBlock = (BasicBlock<TOld>)Content[i];
				TNew arg = updateFunc(basicBlock);
				BasicBlock<TNew> basicBlock2 = factoryFunc(basicBlock.Id, arg);
				basicBlock2.Flags = basicBlock.Flags;
				Content[i] = basicBlock2;
				blockMap[basicBlock] = basicBlock2;
			}
			return;
		}
		foreach (ScopeBlock child in Children)
		{
			child.UpdateBasicBlocksInternal(updateFunc, blockMap, factoryFunc);
		}
	}

	public void ProcessBasicBlocks<T>(Action<BasicBlock<T>> processFunc)
	{
		Validate();
		if (Content.Count > 0)
		{
			foreach (IBasicBlock item in Content)
			{
				processFunc((BasicBlock<T>)item);
			}
			return;
		}
		foreach (ScopeBlock child in Children)
		{
			child.ProcessBasicBlocks(processFunc);
		}
	}

	public void Validate()
	{
		if (Children.Count != 0 && Content.Count != 0)
		{
			throw new InvalidOperationException("Children and Content cannot be set at the same time.");
		}
	}

	public ScopeBlock[] SearchBlock(IBasicBlock target)
	{
		Stack<ScopeBlock> stack = new Stack<ScopeBlock>();
		SearchBlockInternal(this, target, stack);
		return stack.Reverse().ToArray();
	}

	private static bool SearchBlockInternal(ScopeBlock scope, IBasicBlock target, Stack<ScopeBlock> scopeStack)
	{
		if (scope.Content.Count > 0)
		{
			if (scope.Content.Contains(target))
			{
				scopeStack.Push(scope);
				return true;
			}
			return false;
		}
		scopeStack.Push(scope);
		foreach (ScopeBlock child in scope.Children)
		{
			if (SearchBlockInternal(child, target, scopeStack))
			{
				return true;
			}
		}
		scopeStack.Pop();
		return false;
	}

	private static string ToString(ExceptionHandler eh)
	{
		return $"{eh.GetHashCode():x8}:{eh.HandlerType}";
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (Type == ScopeType.Try)
		{
			stringBuilder.AppendLine("try @ " + ToString(ExceptionHandler) + " {");
		}
		else if (Type == ScopeType.Handler)
		{
			stringBuilder.AppendLine("handler @ " + ToString(ExceptionHandler) + " {");
		}
		else if (Type == ScopeType.Filter)
		{
			stringBuilder.AppendLine("filter @ " + ToString(ExceptionHandler) + " {");
		}
		else
		{
			stringBuilder.AppendLine("{");
		}
		if (Children.Count > 0)
		{
			foreach (ScopeBlock child in Children)
			{
				stringBuilder.AppendLine(child.ToString());
			}
		}
		else
		{
			foreach (IBasicBlock item in Content)
			{
				stringBuilder.AppendLine(item.ToString());
			}
		}
		stringBuilder.AppendLine("}");
		return stringBuilder.ToString();
	}
}
