// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.M3U8;

public abstract class Insection
{
    public InsectionType Type { get; }

    protected Insection(InsectionType type) => this.Type = type;

    public static Insection CreateComment(string text)
    {
        if (text is null) throw new ArgumentNullException(nameof(text));
        return new CommentInsection(text);
    }

    public static Insection CreateTag(M3U8Tag tag, string? value = null)
    {
        if (value is null)
            return new TagInsection(tag);
        else
            return new TagInsection(tag, value);
    }

    public static Insection CreateUri(Uri uri, M3U8Key? key = null)
    {
        if (uri is null) throw new ArgumentNullException(nameof(uri));

        return new UriInsection(uri, key);
    }
}
