using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using KoiVM.Core.Helpers;
using XCore.Generator;

namespace KoiVM.Core.RT.Mutation;

internal class NameService
{
	private ModuleDef RTMD;

	private readonly Dictionary<string, string> nameMap = new Dictionary<string, string>();

	private static readonly List<string> stored = new List<string>();

	public NameService(ModuleDef rt)
	{
		RTMD = rt;
		nameMap = new Dictionary<string, string>();
	}

	public string NewName(string name)
	{
		if (!nameMap.TryGetValue(name, out var value))
		{
			return nameMap[name] = GGeneration.GenerateGuidStartingWithLetter();
		}
		return value;
	}

	public void Process()
	{
		foreach (TypeDef type in RTMD.GetTypes())
		{
			if (type.Name == RTMap.Mutation)
			{
				type.Namespace = string.Empty;
				type.Name = NewName(type.Name);
				RTMap.Mutation = type.Name;
				foreach (MethodDef method in type.Methods)
				{
					if (method.Name == RTMap.Mutation_Placeholder)
					{
						method.Name = NewName(method.Name);
						RTMap.Mutation_Placeholder = method.Name;
					}
					if (method.Name == RTMap.Mutation_Value_T)
					{
						method.Name = NewName(method.Name);
						RTMap.Mutation_Value_T = method.Name;
					}
					if (method.Name == RTMap.Mutation_Value_T_Arg0)
					{
						method.Name = NewName(method.Name);
						RTMap.Mutation_Value_T_Arg0 = method.Name;
					}
					if (method.Name == RTMap.Mutation_Crypt)
					{
						method.Name = NewName(method.Name);
						RTMap.Mutation_Crypt = method.Name;
					}
					foreach (Parameter parameter in method.Parameters)
					{
						parameter.Name = NewName(parameter.Name);
					}
				}
				foreach (FieldDef field in type.Fields)
				{
					switch ((string)field.Name)
					{
					case "IntKey0":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 0;
						break;
					case "IntKey1":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 1;
						break;
					case "IntKey2":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 2;
						break;
					case "IntKey3":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 3;
						break;
					case "IntKey4":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 4;
						break;
					case "IntKey5":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 5;
						break;
					case "IntKey6":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 6;
						break;
					case "IntKey7":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 7;
						break;
					case "IntKey8":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 8;
						break;
					case "IntKey9":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 9;
						break;
					case "IntKey10":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 10;
						break;
					case "IntKey11":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 11;
						break;
					case "IntKey12":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 12;
						break;
					case "IntKey13":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 13;
						break;
					case "IntKey14":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 14;
						break;
					case "IntKey15":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 15;
						break;
					case "IntKey16":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 16;
						break;
					case "IntKey17":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 17;
						break;
					case "IntKey18":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 18;
						break;
					case "IntKey19":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 19;
						break;
					case "IntKey20":
						field.Name = NewName(field.Name);
						MutationHelper.Field2IntIndex[field.Name] = 20;
						break;
					case "LongKey0":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 0;
						break;
					case "LongKey1":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 1;
						break;
					case "LongKey2":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 2;
						break;
					case "LongKey3":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 3;
						break;
					case "LongKey4":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 4;
						break;
					case "LongKey5":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 5;
						break;
					case "LongKey6":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 6;
						break;
					case "LongKey7":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 7;
						break;
					case "LongKey8":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 8;
						break;
					case "LongKey9":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 9;
						break;
					case "LongKey10":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 10;
						break;
					case "LongKey11":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 11;
						break;
					case "LongKey12":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 12;
						break;
					case "LongKey13":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 13;
						break;
					case "LongKey14":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 14;
						break;
					case "LongKey15":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 15;
						break;
					case "LongKey16":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 16;
						break;
					case "LongKey17":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 17;
						break;
					case "LongKey18":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 18;
						break;
					case "LongKey19":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 19;
						break;
					case "LongKey20":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LongIndex[field.Name] = 20;
						break;
					case "LdstrKey0":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(0);
						break;
					case "LdstrKey1":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(1);
						break;
					case "LdstrKey2":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(2);
						break;
					case "LdstrKey3":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(3);
						break;
					case "LdstrKey4":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(4);
						break;
					case "LdstrKey5":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(5);
						break;
					case "LdstrKey6":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(6);
						break;
					case "LdstrKey7":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(7);
						break;
					case "LdstrKey8":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(8);
						break;
					case "LdstrKey9":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(9);
						break;
					case "LdstrKey10":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(10);
						break;
					case "LdstrKey11":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(11);
						break;
					case "LdstrKey12":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(12);
						break;
					case "LdstrKey13":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(13);
						break;
					case "LdstrKey14":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(14);
						break;
					case "LdstrKey15":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(15);
						break;
					case "LdstrKey16":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(16);
						break;
					case "LdstrKey17":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(17);
						break;
					case "LdstrKey18":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(18);
						break;
					case "LdstrKey19":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(19);
						break;
					case "LdstrKey20":
						field.Name = NewName(field.Name);
						MutationHelper.Field2LdstrIndex[field.Name] = Convert.ToString(20);
						break;
					}
				}
				continue;
			}
			type.Namespace = string.Empty;
			type.Name = NewName(type.Name);
			foreach (GenericParam genericParameter in type.GenericParameters)
			{
				genericParameter.Name = NewName(genericParameter.Name);
			}
			bool flag = type.BaseType != null && (type.BaseType.FullName == "System.Delegate" || type.BaseType.FullName == "System.MulticastDelegate");
			foreach (MethodDef method2 in type.Methods)
			{
				if (method2.HasBody)
				{
					foreach (Instruction instruction in method2.Body.Instructions)
					{
						if (!(instruction.Operand is MemberRef memberRef))
						{
							continue;
						}
						TypeDef typeDef = memberRef.DeclaringType.ResolveTypeDef();
						if (memberRef.IsMethodRef && typeDef != null)
						{
							MethodDef methodDef = typeDef.ResolveMethod(memberRef);
							if (methodDef != null && methodDef.IsRuntimeSpecialName)
							{
								typeDef = null;
							}
						}
						if (typeDef != null && typeDef.Module == RTMD)
						{
							memberRef.Name = NewName(memberRef.Name);
						}
					}
				}
				foreach (Parameter parameter2 in method2.Parameters)
				{
					parameter2.Name = NewName(parameter2.Name);
				}
				if (!(method2.IsRuntimeSpecialName || flag))
				{
					method2.Name = NewName(method2.Name);
				}
			}
			for (int i = 0; i < type.Fields.Count; i++)
			{
				FieldDef fieldDef = type.Fields[i];
				if (fieldDef.IsLiteral)
				{
					type.Fields.RemoveAt(i--);
				}
				else if (!fieldDef.IsRuntimeSpecialName)
				{
					fieldDef.Name = NewName(fieldDef.Name);
				}
			}
			type.Properties.Clear();
			type.Events.Clear();
		}
	}
}
