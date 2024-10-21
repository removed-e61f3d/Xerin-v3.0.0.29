using System;
using System.Collections.Generic;

namespace KoiVM.Core.CFG;

public class BasicBlock<TContent> : IBasicBlock
{
	public int Id { get; set; }

	public TContent Content { get; set; }

	public BlockFlags Flags { get; set; }

	public IList<BasicBlock<TContent>> Sources { get; private set; }

	public IList<BasicBlock<TContent>> Targets { get; private set; }

	object IBasicBlock.Content => Content;

	IEnumerable<IBasicBlock> IBasicBlock.Sources => Sources;

	IEnumerable<IBasicBlock> IBasicBlock.Targets => Targets;

	public BasicBlock(int id, TContent content)
	{
		Id = id;
		Content = content;
		Sources = new List<BasicBlock<TContent>>();
		Targets = new List<BasicBlock<TContent>>();
	}

	public void LinkTo(BasicBlock<TContent> target)
	{
		Targets.Add(target);
		target.Sources.Add(this);
	}

	public override string ToString()
	{
		return $"Block_{Id:x2}:{Environment.NewLine}{Content}";
	}
}
