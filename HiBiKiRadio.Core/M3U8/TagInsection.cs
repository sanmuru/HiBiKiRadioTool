using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamLu.Utility.HiBiKiRadio.M3U8
{
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
}
