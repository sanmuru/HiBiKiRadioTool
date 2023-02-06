// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER

namespace System.Diagnostics.CodeAnalysis;

#if DEBUG
/// <summary>
/// Specifies that a method that will never return under any circumstance.
/// </summary>
#endif
[AttributeUsage(
    AttributeTargets.Method,
    Inherited = false)]
[DebuggerNonUserCode]
internal sealed class DoesNotReturnAttribute : Attribute
{
#if DEBUG
    /// <summary>
    /// Initializes a new instance of the <see cref="DoesNotReturnAttribute"/> class.
    /// </summary>
    ///
#endif
    public DoesNotReturnAttribute() { }
}

#endif