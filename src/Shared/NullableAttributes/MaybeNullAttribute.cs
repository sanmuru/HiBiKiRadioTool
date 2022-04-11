namespace System.Diagnostics.CodeAnalysis
{
#if DEBUG
    /// <summary>
    ///     Specifies that an output may be <see langword="null"/> even if the
    ///     corresponding type disallows it.
    /// </summary>
#endif
    [AttributeUsage(
        AttributeTargets.Field | AttributeTargets.Parameter |
        AttributeTargets.Property | AttributeTargets.ReturnValue,
        Inherited = false
    )]
    [DebuggerNonUserCode]
    internal sealed class MaybeNullAttribute : Attribute
    {
#if DEBUG
        /// <summary>
        ///     Initializes a new instance of the <see cref="MaybeNullAttribute"/> class.
        /// </summary>
#endif
        public MaybeNullAttribute() { }
    }
}