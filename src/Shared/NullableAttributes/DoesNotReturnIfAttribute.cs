namespace System.Diagnostics.CodeAnalysis
{
#if DEBUG
    /// <summary>
    ///     Specifies that the method will not return if the associated <see cref="Boolean"/>
    ///     parameter is passed the specified value.
    /// </summary>
#endif
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    [DebuggerNonUserCode]
    internal sealed class DoesNotReturnIfAttribute : Attribute
    {
#if DEBUG
        /// <summary>
        ///     Gets the condition parameter value.
        ///     Code after the method is considered unreachable by diagnostics if the argument
        ///     to the associated parameter matches this value.
        /// </summary>
#endif
        public bool ParameterValue { get; }

#if DEBUG
        /// <summary>
        ///     Initializes a new instance of the <see cref="DoesNotReturnIfAttribute"/>
        ///     class with the specified parameter value.
        /// </summary>
        /// <param name="parameterValue">
        ///     The condition parameter value.
        ///     Code after the method is considered unreachable by diagnostics if the argument
        ///     to the associated parameter matches this value.
        /// </param>
#endif
        public DoesNotReturnIfAttribute(bool parameterValue)
        {
            ParameterValue = parameterValue;
        }
    }
}