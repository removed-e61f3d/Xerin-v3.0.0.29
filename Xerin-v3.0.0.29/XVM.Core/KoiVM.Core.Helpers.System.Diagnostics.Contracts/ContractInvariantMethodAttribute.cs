using System;
using System.Diagnostics;

namespace KoiVM.Core.Helpers.System.Diagnostics.Contracts;

[Conditional("CONTRACTS_FULL")]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class ContractInvariantMethodAttribute : Attribute
{
}
