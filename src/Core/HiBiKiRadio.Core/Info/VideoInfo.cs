using SamLu.Utility.HiBiKiRadio.Json;
using System.Diagnostics;
using System.Globalization;

namespace SamLu.Utility.HiBiKiRadio.Info;

[DebuggerDisplay("{Duration}")]
public class VideoInfo : JsonObjectInfo<video>
{
    public int ID => this.jObject.id;
    public TimeSpan Duration => TimeSpan.FromSeconds(this.jObject.duration);
    public bool IsLive => this.jObject.live_flg;
    public DateTime? DeliveryStartTimeUtc => string.IsNullOrEmpty(this.jObject.delivery_start_at) ? null : this.TryParseDateTimeUtc(this.jObject.delivery_start_at, out DateTime dt) ? dt : default;
    public DateTime? DeliveryStartTime => this.DeliveryStartTimeUtc.HasValue ? UtcToLocal(this.DeliveryStartTimeUtc.Value) : null;
    public DateTime? DeliveryEndTimeUtc => string.IsNullOrEmpty(this.jObject.delivery_end_at) ? null : this.TryParseDateTimeUtc(this.jObject.delivery_end_at, out DateTime dt) ? dt : default;
    public DateTime? DeliveryEndTime => this.DeliveryEndTimeUtc.HasValue ? UtcToLocal(this.DeliveryEndTimeUtc.Value) : null;
    public bool IsDelivery => this.jObject.dvr_flg;
    public bool IsReplay => this.jObject.replay_flg;
    public int MediaType => this.jObject.media_type;

    public VideoInfo(video jObject) : base(jObject) { }
}
