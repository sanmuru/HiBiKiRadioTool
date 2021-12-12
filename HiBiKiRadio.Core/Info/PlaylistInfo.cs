using SamLu.Utility.HiBiKiRadio.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    public class PlaylistInfo : JsonObjectInfo<playlist>
    {
        public Uri? PlaylistUri => string.IsNullOrEmpty(this.jObject.playlist_url) ? default : new Uri(this.jObject.playlist_url, UriKind.Absolute);

        public string Token => this.jObject.token;

        public PlaylistInfo(playlist jObject) : base(jObject) { }
    }
}
