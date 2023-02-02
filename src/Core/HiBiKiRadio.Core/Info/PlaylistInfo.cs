using SamLu.Utility.HiBiKiRadio.Json;

namespace SamLu.Utility.HiBiKiRadio.Info;

public class PlaylistInfo : JsonObjectInfo<playlist>
{
    public Uri PlaylistUri => new(this.jObject.playlist_url!, UriKind.Absolute);

    public string Token => this.jObject.token!;

    public PlaylistInfo(playlist jObject) : base(jObject) { }
}
