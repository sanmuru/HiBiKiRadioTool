#if !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER

namespace System.Diagnostics.CodeAnalysis;

#if DEBUG
/// <summary>
/// Specifies that when a method returns <see cref="ReturnValue"/>, the parameter will not be <see langword="null"/> even if the corresponding type allows it.
/// </summary>
#endif
[AttributeUsage(AttributeTargets.Parameter,
    Inherited = false)]
[DebuggerNonUserCode]
internal sealed class NotNullWhenAttribute : Attribute
{
#if DEBUG
    /// <summary>
    /// Gets the return value condition.
    /// If the method returns this value, the associated parameter will not be <see langword="null"/>.
    /// </summary>
#endif
    public bool ReturnValue { get; }

#if DEBUG
    /// <summary>
    /// Initializes the attribute with the specified return value condition.
    /// </summary>
    /// <param name="returnValue">
    /// The return value condition.
    /// If the method returns this value, the associated parameter will not be <see langword="null"/>.
    /// </param>
#endif
    public NotNullWhenAttribute(bool returnValue) => this.ReturnValue = returnValue;
}

#endif