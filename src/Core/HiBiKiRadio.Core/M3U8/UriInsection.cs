// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.M3U8;

public class UriInsection : Insection
{
    public Uri Uri { get; }
    public M3U8Key? Key { get; }

    public UriInsection(Uri uri, M3U8Key? key = null) : base(InsectionType.Uri)
    {
        this.Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        this.Key = key;
    }
}
