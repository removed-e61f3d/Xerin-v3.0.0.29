using System.Collections.Generic;
using Confuser.DynCipher;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Generator;

namespace Confuser.Protections.ReferenceProxy;

internal class RPContext
{
	public CilBody Body;

	public HashSet<Instruction> BranchTargets;

	public XContext Context;

	public Dictionary<MethodSig, TypeDef> Delegates;

	public int Depth;

	public IDynCipherService DynCipher;

	public EncodingType Encoding;

	public IRPEncoding EncodingHandler;

	public int InitCount;

	public bool InternalAlso;

	public MethodDef Method;

	public Mode Mode;

	public RPMode ModeHandler;

	public ModuleDef Module;

	public RandomGenerator Random;

	public bool TypeErasure;
}
