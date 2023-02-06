// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCOREAPP3_0_OR_GREATER

using System.Diagnostics;

namespace System.Runtime.CompilerServices;

[AttributeUsage(
    AttributeTargets.Parameter,
    AllowMultiple = false, Inherited = false)]
[DebuggerNonUserCode]
public sealed class CallerArgumentExpressionAttribute : Attribute
{
    public string ParameterName { get; }

    public CallerArgumentExpressionAttribute(string parameterName) => this.ParameterName = parameterName;
}

#endif
