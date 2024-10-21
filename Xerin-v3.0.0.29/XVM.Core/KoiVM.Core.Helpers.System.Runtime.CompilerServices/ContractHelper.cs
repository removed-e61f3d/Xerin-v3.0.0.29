using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using KoiVM.Core.Helpers.System.Diagnostics.Contracts;

namespace KoiVM.Core.Helpers.System.Runtime.CompilerServices;

public static class ContractHelper
{
	[DebuggerNonUserCode]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static string RaiseContractFailedEvent(ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException)
	{
		return "Contract failed";
	}

	[DebuggerNonUserCode]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static void TriggerFailure(ContractFailureKind kind, string displayMessage, string userMessage, string conditionText, Exception innerException)
	{
	}
}
