// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.M3U8;

public class CommentInsection : Insection
{
    public string Text { get; }

    public CommentInsection(string text) : base(InsectionType.Comment) => this.Text = text;
}
