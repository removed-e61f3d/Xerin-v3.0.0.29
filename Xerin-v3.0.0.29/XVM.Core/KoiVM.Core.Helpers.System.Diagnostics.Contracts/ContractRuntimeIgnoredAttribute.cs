using System;
using System.Diagnostics;

namespace KoiVM.Core.Helpers.System.Diagnostics.Contracts;

[Conditional("CONTRACTS_FULL")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class ContractRuntimeIgnoredAttribute : Attribute
{
}
