// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET35

using System.Runtime.CompilerServices;

namespace System.IO;

public static class StreamEx
{
    public static void CopyTo(this Stream source, Stream destination) => source.CopyTo(destination, 81920);

    public static void CopyTo(this Stream source, Stream destination, int bufferSize)
    {
        var buffer = new byte[bufferSize];
        int count;
        while ((count = source.Read(buffer, 0, buffer.Length)) != 0)
            destination.Write(buffer, 0, count);
    }
}

#endif
