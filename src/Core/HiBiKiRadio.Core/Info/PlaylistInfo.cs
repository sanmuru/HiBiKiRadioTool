// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json;

namespace Qtyi.HiBiKiRadio.Info;

public class PlaylistInfo : JsonObjectInfo
{
    public Uri PlaylistUri => new(this.JsonObject.playlist_url!, UriKind.Absolute);

    public string Token => this.JsonObject.token!;

    public PlaylistInfo(playlist jObject) : base(jObject) { }
}
