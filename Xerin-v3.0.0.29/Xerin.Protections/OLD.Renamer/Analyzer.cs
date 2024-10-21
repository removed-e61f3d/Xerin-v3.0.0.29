using dnlib.DotNet;
using XCore.Utils;

namespace OLD.Renamer;

internal class Analyzer
{
	public static bool CanRename(TypeDef type)
	{
		if (type.FullName == "TrinityAttribute")
		{
			return false;
		}
		if (type.Namespace == "Costura")
		{
			return false;
		}
		if (type.Name.StartsWith("<"))
		{
			return false;
		}
		if (type.IsGlobalModuleType)
		{
			return false;
		}
		if (type.IsInterface)
		{
			return false;
		}
		if (type.IsForwarder)
		{
			return false;
		}
		if (type.IsSerializable)
		{
			return false;
		}
		if (type.IsEnum)
		{
			return false;
		}
		if (type.IsRuntimeSpecialName)
		{
			return false;
		}
		if (type.IsSpecialName)
		{
			return false;
		}
		if (type.IsWindowsRuntime)
		{
			return false;
		}
		if (type.IsNestedFamilyOrAssembly)
		{
			return false;
		}
		if (type.IsNestedFamilyAndAssembly)
		{
			return false;
		}
		return true;
	}

	public static bool CanRename(EventDef e)
	{
		if (e.IsSpecialName)
		{
			return false;
		}
		if (e.IsRuntimeSpecialName)
		{
			return false;
		}
		return true;
	}

	public static bool CanRename(TypeDef type, PropertyDef p)
	{
		if (p.DeclaringType.Implements("System.ComponentModel.INotifyPropertyChanged"))
		{
			return false;
		}
		if (type.Namespace.String.Contains(".Properties"))
		{
			return false;
		}
		if (p.DeclaringType.Name.String.Contains("AnonymousType"))
		{
			return false;
		}
		if (p.IsRuntimeSpecialName)
		{
			return false;
		}
		if (p.IsEmpty)
		{
			return false;
		}
		if (p.IsSpecialName)
		{
			return false;
		}
		return true;
	}

	public static bool CanRename(TypeDef type, FieldDef field)
	{
		if (field.DeclaringType.IsSerializable && !field.IsNotSerialized)
		{
			return false;
		}
		if (field.DeclaringType.BaseType.Name.Contains("Delegate"))
		{
			return false;
		}
		if (field.Name.StartsWith("<"))
		{
			return false;
		}
		if (field.IsLiteral && field.DeclaringType.IsEnum)
		{
			return false;
		}
		if (field.IsFamilyOrAssembly)
		{
			return false;
		}
		if (field.IsSpecialName)
		{
			return false;
		}
		if (field.IsRuntimeSpecialName)
		{
			return false;
		}
		if (field.IsFamily)
		{
			return false;
		}
		if (field.DeclaringType.IsEnum)
		{
			return false;
		}
		if (field.DeclaringType.BaseType.Name.Contains("Delegate"))
		{
			return false;
		}
		return true;
	}

	public static bool CanRename(MethodDef method, TypeDef type)
	{
		if (method.DeclaringType.IsComImport() && !method.HasAttribute("System.Runtime.InteropServices.DispIdAttribute"))
		{
			return false;
		}
		if (!method.HasBody || !method.Body.HasInstructions)
		{
			return false;
		}
		if (method.DeclaringType.BaseType != null && method.DeclaringType.BaseType.Name.Contains("Delegate"))
		{
			return false;
		}
		if (method.DeclaringType.IsDelegate())
		{
			return false;
		}
		if (method.DeclaringType.FullName == "System.Windows.Forms.Binding" && method.Name.String == ".ctor")
		{
			return false;
		}
		if (method.DeclaringType.FullName == "System.Windows.Forms.ControlBindingsCollection")
		{
			return false;
		}
		if (method.DeclaringType.FullName == "System.Windows.Forms.BindingsCollection")
		{
			return false;
		}
		if (method.DeclaringType.FullName == "System.Windows.Forms.DataGridViewColumn")
		{
			return false;
		}
		if (method.Name == "Invoke")
		{
			return false;
		}
		if (method.IsSetter || method.IsGetter)
		{
			return false;
		}
		if (method.IsSpecialName)
		{
			return false;
		}
		if (method.IsFamilyAndAssembly)
		{
			return false;
		}
		if (method.IsFamily)
		{
			return false;
		}
		if (method.IsRuntime)
		{
			return false;
		}
		if (method.IsRuntimeSpecialName)
		{
			return false;
		}
		if (method.IsConstructor)
		{
			return false;
		}
		if (method.IsNative)
		{
			return false;
		}
		if (method.IsPinvokeImpl || method.IsUnmanaged || method.IsUnmanagedExport)
		{
			return false;
		}
		if (method == null)
		{
			return false;
		}
		if (method.Name.StartsWith("<"))
		{
			return false;
		}
		if (method.Overrides.Count > 0)
		{
			return false;
		}
		if (method.IsStaticConstructor)
		{
			return false;
		}
		if (method.DeclaringType.IsGlobalModuleType)
		{
			return false;
		}
		if (method.DeclaringType.IsForwarder)
		{
			return false;
		}
		if (method.IsVirtual)
		{
			return false;
		}
		if (method.HasImplMap)
		{
			return false;
		}
		return true;
	}

	public static bool CanRename(TypeDef type, Parameter p)
	{
		if (type.FullName == "<Module>")
		{
			return false;
		}
		if (p.IsHiddenThisParameter)
		{
			return false;
		}
		if (p.Name == string.Empty)
		{
			return false;
		}
		return true;
	}
}
