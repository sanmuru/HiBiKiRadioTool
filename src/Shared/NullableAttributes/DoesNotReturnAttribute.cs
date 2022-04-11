namespace System.Diagnostics.CodeAnalysis
{
#if DEBUG
    /// <summary>
    ///     Specifies that a method that will never return under any circumstance.
    /// </summary>
#endif
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    [DebuggerNonUserCode]
    internal sealed class DoesNotReturnAttribute : Attribute
    {
#if DEBUG
        /// <summary>
        ///     Initializes a new instance of the <see cref="DoesNotReturnAttribute"/> class.
        /// </summary>
        ///
#endif
        public DoesNotReturnAttribute() { }
    }
}