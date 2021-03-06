using SamLu.Utility.HiBiKiRadio.Json;
using System;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    public class PlaylistInfo : JsonObjectInfo<playlist>
    {
        public Uri PlaylistUri => string.IsNullOrEmpty(this.jObject.playlist_url) ? default : new Uri(this.jObject.playlist_url, UriKind.Absolute);

        public string Token => this.jObject.token;

        public PlaylistInfo(playlist jObject) : base(jObject) { }
    }
}
