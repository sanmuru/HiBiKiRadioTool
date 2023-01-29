using System.Runtime.Serialization;

namespace SamLu.Utility.HiBiKiRadio.M3U8;

public class M3U8FormatException : FormatException
{
    public M3U8FormatException() : this("M3U8格式错误") { }

    public M3U8FormatException(string message) : base(message) { }

    public M3U8FormatException(string message, Exception innerException) : base(message, innerException) { }

    protected M3U8FormatException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
