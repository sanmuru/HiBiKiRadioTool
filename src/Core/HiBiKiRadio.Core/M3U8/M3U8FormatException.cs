﻿using System.Runtime.Serialization;

namespace SamLu.Utility.HiBiKiRadio.M3U8;

public class M3U8FormatException : FormatException
{
    public M3U8FormatException() : this("M3U8格式错误") { }

    public M3U8FormatException(string message) : base(message) { }

    public M3U8FormatException(string message, Exception innerException) : base(message, innerException) { }

#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER
    protected M3U8FormatException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}
