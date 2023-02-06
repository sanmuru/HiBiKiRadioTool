// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETFRAMEWORK && !NET45_OR_GREATER

using System.Diagnostics;

namespace System.Runtime.CompilerServices;

[AttributeUsage(
    AttributeTargets.Parameter,
    Inherited = false)]
[DebuggerNonUserCode]
public sealed class CallerLineNumberAttribute : Attribute
{
    public CallerLineNumberAttribute() { }
}

#endif
