namespace System.Diagnostics.CodeAnalysis
{
#if DEBUG
    /// <summary>
    ///     Specifies that the method or property will ensure that the listed field and property members have
    ///     not-<see langword="null"/> values.
    /// </summary>
#endif
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    [DebuggerNonUserCode]
    internal sealed class MemberNotNullAttribute : Attribute
    {
#if DEBUG
        /// <summary>
        ///     Gets field or property member names.
        /// </summary>
#endif
        public string[] Members { get; }

#if DEBUG
        /// <summary>
        ///     Initializes the attribute with a field or property member.
        /// </summary>
        /// <param name="member">
        ///     The field or property member that is promised to be not-null.
        /// </param>
#endif
        public MemberNotNullAttribute(string member)
        {
            Members = new[] { member };
        }

#if DEBUG
        /// <summary>
        ///     Initializes the attribute with the list of field and property members.
        /// </summary>
        /// <param name="members">
        ///     The list of field and property members that are promised to be not-null.
        /// </param>
#endif
        public MemberNotNullAttribute(params string[] members)
        {
            Members = members;
        }
    }
}