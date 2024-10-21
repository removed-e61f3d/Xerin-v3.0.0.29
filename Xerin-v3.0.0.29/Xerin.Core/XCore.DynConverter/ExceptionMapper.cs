using System.Collections.Generic;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace XCore.DynConverter;

public class ExceptionMapper
{
	private IList<ExceptionHandler> Exceptions { get; }

	public ExceptionMapper(MethodDef method)
	{
		Exceptions = method.Body.ExceptionHandlers;
	}

	public void MapAndWrite(BinaryWriter writer, Instruction instr)
	{
		int num = 0;
		List<int> list = new List<int>();
		foreach (ExceptionHandler exception in Exceptions)
		{
			if (exception.TryStart == instr)
			{
				list.Add(0);
				num++;
			}
			else if (exception.HandlerEnd == instr)
			{
				list.Add(5);
				num++;
			}
			else if (exception.HandlerType == ExceptionHandlerType.Filter && exception.FilterStart == instr)
			{
				list.Add(1);
				num++;
			}
			else
			{
				if (exception.HandlerStart != instr)
				{
					continue;
				}
				switch (exception.HandlerType)
				{
				case ExceptionHandlerType.Catch:
					list.Add(2);
					if (exception.CatchType == null)
					{
						list.Add(-1);
					}
					else
					{
						list.Add(exception.CatchType.MDToken.ToInt32());
					}
					break;
				case ExceptionHandlerType.Finally:
					list.Add(3);
					break;
				case ExceptionHandlerType.Fault:
					list.Add(4);
					break;
				}
				num++;
			}
		}
		writer.Write(num);
		foreach (int item in list)
		{
			writer.Write(item);
		}
	}
}
