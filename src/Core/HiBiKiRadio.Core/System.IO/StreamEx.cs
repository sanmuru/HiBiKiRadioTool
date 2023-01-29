#if NET35

using System.Runtime.CompilerServices;

namespace System.IO;

public static class StreamEx
{
    public static void CopyTo(this Stream source, Stream destination) => source.CopyTo(destination, 81920);

    public static void CopyTo(this Stream source, Stream destination, int bufferSize)
    {
        byte[] buffer = new byte[bufferSize];
        int count;
        while ((count = source.Read(buffer, 0, buffer.Length)) != 0)
            destination.Write(buffer, 0, count);
    }
}

#endif
