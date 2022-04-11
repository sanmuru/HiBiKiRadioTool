using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamLu.Utility.HiBiKiRadio.M3U8
{
    public class CommentInsection : Insection
    {
        public string Text { get; }

        public CommentInsection(string text) : base(InsectionType.Comment) => this.Text = text;
    }
}
