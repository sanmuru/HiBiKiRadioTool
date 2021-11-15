using HiBikiRadioTool.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool
{
    public class PlaylistInfo
    {
        private readonly playlist jObject;

        public Uri PlaylistUri => string.IsNullOrEmpty(this.jObject.playlist_url) ? default : new Uri(this.jObject.playlist_url, UriKind.Absolute);

        public string Token => this.jObject.token;

        public PlaylistInfo(playlist jObject) => this.jObject = jObject ?? throw new ArgumentNullException(nameof(jObject));

    }
}
