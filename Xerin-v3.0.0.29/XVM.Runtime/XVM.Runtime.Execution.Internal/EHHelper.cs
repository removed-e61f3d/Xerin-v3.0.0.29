using System;
using System.Reflection;
using System.Reflection.Emit;

namespace XVM.Runtime.Execution.Internal;

internal class EHHelper
{
	private delegate void Throw(Exception ex, string ip, bool rethrow);

	private static Throw rethrow;

	private static readonly object RethrowKey;

	private static bool BuildExceptionDispatchInfo(Type type)
	{
		try
		{
			MethodInfo method = type.GetMethod("Capture");
			MethodInfo method2 = type.GetMethod("Throw");
			DynamicMethod dynamicMethod = new DynamicMethod("", typeof(void), new Type[3]
			{
				typeof(Exception),
				typeof(string),
				typeof(bool)
			});
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Call, method);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Call, method2);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ret);
			rethrow = (Throw)dynamicMethod.CreateDelegate(typeof(Throw));
		}
		catch
		{
			return false;
		}
		return true;
	}

	private static bool BuildInternalPreserve(Type type)
	{
		try
		{
			string text = (string)typeof(Environment).InvokeMember("GetResourceString", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, new object[1] { "Word_At" });
			MethodInfo method = type.GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo field = type.GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
			MethodInfo getMethod = type.GetProperty("StackTrace", BindingFlags.Instance | BindingFlags.Public).GetGetMethod();
			MethodInfo method2 = typeof(string).GetMethod("Format", new Type[3]
			{
				typeof(string),
				typeof(object),
				typeof(object)
			});
			DynamicMethod dynamicMethod = new DynamicMethod("", typeof(void), new Type[3]
			{
				typeof(Exception),
				typeof(string),
				typeof(bool)
			}, restrictedSkipVisibility: true);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			Label label = iLGenerator.DefineLabel();
			Label label2 = iLGenerator.DefineLabel();
			Label label3 = iLGenerator.DefineLabel();
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Dup);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Dup);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldfld, field);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Brtrue, label2);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Callvirt, getMethod);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Br, label3);
			iLGenerator.MarkLabel(label2);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldfld, field);
			iLGenerator.MarkLabel(label3);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Call, method);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Stfld, field);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarg_1);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Brfalse, label);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarg_2);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Brtrue, label);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Dup);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldstr, "{1}" + Environment.NewLine + "   " + text + " KoiVM [{0}]" + Environment.NewLine);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarg_1);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldfld, field);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Call, method2);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Stfld, field);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Throw);
			iLGenerator.MarkLabel(label);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Throw);
			rethrow = (Throw)dynamicMethod.CreateDelegate(typeof(Throw));
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			return false;
		}
		return true;
	}

	static EHHelper()
	{
		RethrowKey = new object();
		if (!BuildInternalPreserve(typeof(Exception)))
		{
			Type type = Type.GetType("System.Runtime.ExceptionServices.ExceptionDispatchInfo");
			if (!(type != null) || !BuildExceptionDispatchInfo(type))
			{
				rethrow = null;
			}
		}
	}

	public static void Rethrow(Exception ex, string tokens)
	{
		if (tokens == null)
		{
			throw ex;
		}
		bool flag = ex.Data.Contains(RethrowKey);
		if (!flag)
		{
			ex.Data[RethrowKey] = RethrowKey;
		}
		if (rethrow != null)
		{
			rethrow(ex, tokens, flag);
		}
		throw ex;
	}
}
