// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER

namespace System.Diagnostics.CodeAnalysis;

#if DEBUG
/// <summary>
/// Specifies that <see langword="null"/> is disallowed as an input even if the corresponding type allows it.
/// </summary>
#endif
[AttributeUsage(
    AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property,
    Inherited = false)]
[DebuggerNonUserCode]
internal sealed class DisallowNullAttribute : Attribute
{
#if DEBUG
    /// <summary>
    /// Initializes a new instance of the <see cref="DisallowNullAttribute"/> class.
    /// </summary>
#endif
    public DisallowNullAttribute() { }
}

#endif
