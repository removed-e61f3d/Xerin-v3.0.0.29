#define DEBUG
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using dnlib.DotNet;
using KoiVM.Core.AST;
using KoiVM.Core.AST.IL;
using KoiVM.Core.CFG;
using KoiVM.Core.Helpers;
using KoiVM.Core.Helpers.System;
using KoiVM.Core.RT.Mutation;
using KoiVM.Core.VM;

namespace KoiVM.Core.RT;

public class VMRuntime
{
	internal Dictionary<MethodDef, Tuple<ScopeBlock, ILBlock>> MethodMap;

	private List<Tuple<MethodDef, ILBlock>> BasicBlocks;

	private List<IChunk> ExtraChunks;

	private List<IChunk> FinalChunks;

	public ModuleDefMD RTModule { get; private set; }

	public IVMSettings VMSettings { get; private set; }

	public VMDescriptor Descriptor { get; private set; }

	public SaveRuntime SaveRT { get; private set; }

	internal BasicBlockSerializer Serializer { get; private set; }

	internal RuntimeMutator RTMutator { get; private set; }

	internal RuntimeSearch RTSearch { get; set; }

	internal NameService RNMService { get; private set; }

	internal MemoryStream RuntimeLibrary { get; set; }

	public VMRuntime(IVMSettings settings, ModuleDef rt, SaveRuntime savert)
	{
		MethodMap = new Dictionary<MethodDef, Tuple<ScopeBlock, ILBlock>>();
		BasicBlocks = new List<Tuple<MethodDef, ILBlock>>();
		ExtraChunks = new List<IChunk>();
		FinalChunks = new List<IChunk>();
		RTModule = (ModuleDefMD)rt;
		VMSettings = settings;
		SaveRT = savert;
		Descriptor = new VMDescriptor(settings);
		Serializer = new BasicBlockSerializer(this);
		RTMutator = new RuntimeMutator(this);
		RTSearch = new RuntimeSearch(rt, this).Search();
		RNMService = new NameService(rt);
		SaveRT.Runtime = this;
	}

	public void AddMethod(MethodDef method, ScopeBlock rootScope)
	{
		ILBlock iLBlock = null;
		foreach (ILBlock basicBlock in rootScope.GetBasicBlocks())
		{
			if (basicBlock.Id == 0)
			{
				iLBlock = basicBlock;
			}
			BasicBlocks.Add(Tuple.Create(method, basicBlock));
		}
		Debug.Assert(iLBlock != null);
		MethodMap[method] = Tuple.Create(rootScope, iLBlock);
	}

	internal void AddHelper(MethodDef method, ScopeBlock rootScope, ILBlock entry)
	{
		MethodMap[method] = Tuple.Create(rootScope, entry);
	}

	public void AddBlock(MethodDef method, ILBlock block)
	{
		BasicBlocks.Add(Tuple.Create(method, block));
	}

	public ScopeBlock LookupMethod(MethodDef method)
	{
		Tuple<ScopeBlock, ILBlock> tuple = MethodMap[method];
		return tuple.Item1;
	}

	public ScopeBlock LookupMethod(MethodDef method, out ILBlock entry)
	{
		Tuple<ScopeBlock, ILBlock> tuple = MethodMap[method];
		entry = tuple.Item2;
		return tuple.Item1;
	}

	public void AddChunk(IChunk chunk)
	{
		ExtraChunks.Add(chunk);
	}

	public void ExportMethod(MethodDef method)
	{
		MethodPatcher.Patch(RTSearch, method, Descriptor.Data.GetExportId(method));
	}

	public void OnKoiRequested()
	{
		HeaderChunk headerChunk = new HeaderChunk(this);
		foreach (Tuple<MethodDef, ILBlock> basicBlock in BasicBlocks)
		{
			FinalChunks.Add(basicBlock.Item2.CreateChunk(this, basicBlock.Item1));
		}
		FinalChunks.AddRange(ExtraChunks);
		Descriptor.RandomGenerator.Shuffle(FinalChunks);
		FinalChunks.Insert(0, headerChunk);
		ComputeOffsets();
		FixupReferences();
		headerChunk.WriteData(this);
		List<byte> list = new List<byte>();
		foreach (IChunk finalChunk in FinalChunks)
		{
			list.AddRange(finalChunk.GetData());
		}
		MutationHelper.InjectKeys_Int(RTSearch.VMData_Ctor, new int[3] { 1, 2, 3 }, new int[3]
		{
			Descriptor.Data.strMap.Count,
			Descriptor.Data.refMap.Count,
			Descriptor.Data.sigs.Count
		});
		TypeDefUser typeDefUser = new TypeDefUser(RNMService.NewName(Descriptor.RandomGenerator.NextString()), RTModule.CorLibTypes.GetTypeRef("System", "ValueType"))
		{
			Layout = TypeAttributes.ExplicitLayout,
			Visibility = TypeAttributes.Sealed,
			IsSealed = true,
			ClassLayout = new ClassLayoutUser(0, (uint)list.Count)
		};
		RTModule.Types.Add(typeDefUser);
		FieldDefUser fieldDefUser = new FieldDefUser(RNMService.NewName(Descriptor.RandomGenerator.NextString()), new FieldSig(typeDefUser.ToTypeSig()), FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.HasFieldRVA)
		{
			HasFieldRVA = true,
			InitialValue = list.ToArray()
		};
		RTSearch.VMData.Fields.Add(fieldDefUser);
		MutationHelper.InjectKey_Int(RTSearch.VMData_Ctor, 0, list.Count);
		MutationHelper.InjectKey_String(RTSearch.VMData_Ctor, 0, fieldDefUser.Name);
	}

	private void ComputeOffsets()
	{
		uint num = 0u;
		foreach (IChunk finalChunk in FinalChunks)
		{
			finalChunk.OnOffsetComputed(num);
			num += finalChunk.Length;
		}
	}

	private void FixupReferences()
	{
		foreach (Tuple<MethodDef, ILBlock> basicBlock in BasicBlocks)
		{
			foreach (ILInstruction item in basicBlock.Item2.Content)
			{
				if (item.Operand is ILRelReference)
				{
					ILRelReference iLRelReference = (ILRelReference)item.Operand;
					item.Operand = ILImmediate.Create(iLRelReference.Resolve(this), ASTType.I4);
				}
			}
		}
	}

	public void ResetData()
	{
		MethodMap = new Dictionary<MethodDef, Tuple<ScopeBlock, ILBlock>>();
		BasicBlocks = new List<Tuple<MethodDef, ILBlock>>();
		ExtraChunks = new List<IChunk>();
		FinalChunks = new List<IChunk>();
		Descriptor.ResetData();
	}
}
