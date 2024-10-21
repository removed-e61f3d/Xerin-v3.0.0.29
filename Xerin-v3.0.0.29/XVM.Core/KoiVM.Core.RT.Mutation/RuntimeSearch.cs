using dnlib.DotNet;

namespace KoiVM.Core.RT.Mutation;

internal class RuntimeSearch
{
	private VMRuntime VMRT;

	private ModuleDef RTMD;

	public TypeDef VMData;

	public MethodDef VMData_Ctor;

	public TypeDef TypedRef;

	public MethodDef TypedRef_Ctor;

	public TypeDef VMEntry;

	public MethodDef VMEntry_Ctor;

	public MethodDef VMEntry_Run;

	public RuntimeSearch(ModuleDef rt, VMRuntime runtime)
	{
		RTMD = rt;
		VMRT = runtime;
	}

	public RuntimeSearch Search()
	{
		VMEntry = RTMD.Find(RTMap.VMEntry, isReflectionName: true);
		VMEntry_Run = VMEntry.FindMethod(RTMap.VMEntry_Run);
		foreach (MethodDef item in VMEntry.FindMethods(RTMap.AnyCtor))
		{
			VMEntry_Ctor = item;
		}
		VMData = RTMD.Find(RTMap.VMData, isReflectionName: true);
		foreach (MethodDef item2 in VMData.FindMethods(RTMap.AnyCtor))
		{
			VMData_Ctor = item2;
		}
		TypedRef = RTMD.Find(RTMap.TypedRef, isReflectionName: true);
		foreach (MethodDef item3 in TypedRef.FindMethods(RTMap.AnyCtor))
		{
			TypedRef_Ctor = item3;
		}
		return this;
	}
}
