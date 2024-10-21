using dnlib.DotNet;
using dnlib.DotNet.Emit;
using XCore.Context;
using XCore.Generator;
using XCore.Protections;

namespace OLD.Renamer;

public class Renamer : Protection
{
	public override string name => "Renamer";

	public override int number => 0;

	public override void Execute(XContext ctx)
	{
		foreach (TypeDef type in ctx.Module.Types)
		{
			if (Globals.props)
			{
				foreach (PropertyDef property in type.Properties)
				{
					if (Analyzer.CanRename(type, property))
					{
						property.Name = GGeneration.GenerateGuidStartingWithLetter();
					}
				}
			}
			if (Globals.flds)
			{
				foreach (FieldDef field in type.Fields)
				{
					if (Analyzer.CanRename(type, field))
					{
						field.Name = GGeneration.GenerateGuidStartingWithLetter();
					}
				}
			}
			if (Globals.events)
			{
				foreach (EventDef @event in type.Events)
				{
					if (Analyzer.CanRename(@event))
					{
						@event.Name = GGeneration.GenerateGuidStartingWithLetter();
					}
				}
			}
			if (Globals.methods)
			{
				foreach (MethodDef method in type.Methods)
				{
					if (Analyzer.CanRename(method, type))
					{
						method.Name = GGeneration.GenerateGuidStartingWithLetter();
					}
				}
			}
			if (Globals.parameters)
			{
				foreach (MethodDef method2 in type.Methods)
				{
					foreach (Parameter parameter in method2.Parameters)
					{
						foreach (GenericParam genericParameter in type.GenericParameters)
						{
							if (Analyzer.CanRename(type, parameter))
							{
								genericParameter.Name = GGeneration.GenerateGuidStartingWithLetter();
							}
							parameter.Name = GGeneration.GenerateGuidStartingWithLetter();
						}
					}
				}
			}
			if (!Globals.types || !Analyzer.CanRename(type))
			{
				continue;
			}
			string text = GGeneration.GenerateGuidStartingWithLetter();
			string text2 = GGeneration.GenerateGuidStartingWithLetter();
			foreach (MethodDef method3 in type.Methods)
			{
				if (type.BaseType != null && type.BaseType.FullName.ToLower().Contains("form"))
				{
					foreach (Resource resource in ctx.Module.Resources)
					{
						if (resource.Name.Contains(string.Concat(type.Name, ".resources")))
						{
							resource.Name = text + "." + text2 + ".resources";
						}
					}
				}
				type.Namespace = text;
				type.Name = text2;
				if (!method3.Name.Equals("InitializeComponent") || !method3.HasBody)
				{
					continue;
				}
				foreach (Instruction instruction in method3.Body.Instructions)
				{
					if (instruction.OpCode.Equals(OpCodes.Ldstr))
					{
						string text3 = (string)instruction.Operand;
						if (text3 == type.Name)
						{
							instruction.Operand = text2;
							break;
						}
					}
				}
			}
		}
	}
}
