// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER

namespace System.Diagnostics.CodeAnalysis;

#if DEBUG
/// <summary>
/// Specifies that the output will be non-<see langword="null"/> if the named parameter is non-<see langword="null"/>.
/// </summary>
#endif
[AttributeUsage(
    AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue,
    AllowMultiple = true,
    Inherited = false)]
[DebuggerNonUserCode]
internal sealed class NotNullIfNotNullAttribute : Attribute
{
#if DEBUG
    /// <summary>
    /// Gets the associated parameter name.
    /// The output will be non-<see langword="null"/> if the argument to the parameter specified is non-<see langword="null"/>.
    /// </summary>
#endif
    public string ParameterName { get; }

#if DEBUG
    /// <summary>
    /// Initializes the attribute with the associated parameter name.
    /// </summary>
    /// <param name="parameterName">
    /// The associated parameter name.
    /// The output will be non-<see langword="null"/> if the argument to the parameter specified is non-<see langword="null"/>.
    /// </param>
#endif
    public NotNullIfNotNullAttribute(string parameterName) => this.ParameterName = parameterName;
}

#endif
