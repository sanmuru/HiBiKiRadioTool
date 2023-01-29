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