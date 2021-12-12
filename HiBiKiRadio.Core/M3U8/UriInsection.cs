using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamLu.Utility.HiBiKiRadio.M3U8
{
    public class UriInsection : Insection
    {
        public Uri Uri { get; }
        public M3U8Key Key { get; }

        public UriInsection(Uri uri, M3U8Key key = null) : base(InsectionType.Uri)
        {
            this.Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            this.Key = key;
        }
    }
}
