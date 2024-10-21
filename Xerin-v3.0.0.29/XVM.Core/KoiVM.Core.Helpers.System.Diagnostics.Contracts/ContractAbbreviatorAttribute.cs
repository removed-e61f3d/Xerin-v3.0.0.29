using System;
using System.Diagnostics;

namespace KoiVM.Core.Helpers.System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractAbbreviatorAttribute : Attribute
{
}
