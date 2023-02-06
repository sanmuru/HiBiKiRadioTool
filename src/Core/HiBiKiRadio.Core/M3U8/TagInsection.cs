// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.M3U8;

public class TagInsection : Insection
{
    public M3U8Tag Tag { get; }
    public string Value { get; }


    public TagInsection(M3U8Tag tag) : base(InsectionType.Tag) => this.Tag = tag;
    public TagInsection(M3U8Tag tag, string value) : this(tag)
    {
        this.Value = value ?? throw new ArgumentNullException(nameof(value));
    }
}
