﻿// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER

namespace System.Diagnostics.CodeAnalysis;

#if DEBUG
/// <summary>
/// Specifies that an output may be <see langword="null"/> even if the corresponding type disallows it.
/// </summary>
#endif
[AttributeUsage(
    AttributeTargets.Field | AttributeTargets.Parameter |
    AttributeTargets.Property | AttributeTargets.ReturnValue,
    Inherited = false)]
[DebuggerNonUserCode]
internal sealed class MaybeNullAttribute : Attribute
{
#if DEBUG
    /// <summary>
    /// Initializes a new instance of the <see cref="MaybeNullAttribute"/> class.
    /// </summary>
#endif
    public MaybeNullAttribute() { }
}

#endif
