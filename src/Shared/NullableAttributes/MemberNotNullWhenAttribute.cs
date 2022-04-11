namespace System.Diagnostics.CodeAnalysis
{
#if DEBUG
    /// <summary>
    ///     Specifies that the method or property will ensure that the listed field and property members have
    ///     non-<see langword="null"/> values when returning with the specified return value condition.
    /// </summary>
#endif
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    [DebuggerNonUserCode]
    internal sealed class MemberNotNullWhenAttribute : Attribute
    {
#if DEBUG
        /// <summary>
        ///     Gets the return value condition.
        /// </summary>
#endif
        public bool ReturnValue { get; }

#if DEBUG
        /// <summary>
        ///     Gets field or property member names.
        /// </summary>
#endif
        public string[] Members { get; }

#if DEBUG
        /// <summary>
        ///     Initializes the attribute with the specified return value condition and a field or property member.
        /// </summary>
        /// <param name="returnValue">
        ///     The return value condition. If the method returns this value,
        ///     the associated parameter will not be <see langword="null"/>.
        /// </param>
        /// <param name="member">
        ///     The field or property member that is promised to be not-<see langword="null"/>.
        /// </param>
#endif
        public MemberNotNullWhenAttribute(bool returnValue, string member)
        {
            ReturnValue = returnValue;
            Members = new[] { member };
        }

#if DEBUG
        /// <summary>
        ///     Initializes the attribute with the specified return value condition and list
        ///     of field and property members.
        /// </summary>
        /// <param name="returnValue">
        ///     The return value condition. If the method returns this value,
        ///     the associated parameter will not be <see langword="null"/>.
        /// </param>
        /// <param name="members">
        ///     The list of field and property members that are promised to be not-null.
        /// </param>
#endif
        public MemberNotNullWhenAttribute(bool returnValue, params string[] members)
        {
            ReturnValue = returnValue;
            Members = members;
        }
    }
}