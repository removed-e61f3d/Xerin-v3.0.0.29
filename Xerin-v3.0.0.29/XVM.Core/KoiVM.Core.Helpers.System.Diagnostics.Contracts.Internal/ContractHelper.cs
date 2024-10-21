using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using KoiVM.Core.Helpers.System.Runtime.CompilerServices;

namespace KoiVM.Core.Helpers.System.Diagnostics.Contracts.Internal;

[Obsolete("Use the ContractHelper class in the System.Runtime.CompilerServices namespace instead.")]
public static class ContractHelper
{
	[DebuggerNonUserCode]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static string RaiseContractFailedEvent(ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException)
	{
		return KoiVM.Core.Helpers.System.Runtime.CompilerServices.ContractHelper.RaiseContractFailedEvent(failureKind, userMessage, conditionText, innerException);
	}

	[DebuggerNonUserCode]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static void TriggerFailure(ContractFailureKind kind, string displayMessage, string userMessage, string conditionText, Exception innerException)
	{
		KoiVM.Core.Helpers.System.Runtime.CompilerServices.ContractHelper.TriggerFailure(kind, displayMessage, userMessage, conditionText, innerException);
	}
}
