using SamLu.Utility.HiBiKiRadio.Json;
using System;
using System.Diagnostics;
using System.Globalization;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    [DebuggerDisplay("{Duration}")]
    public class VideoInfo : JsonObjectInfo<video>
    {
        public int ID => this.jObject.id;
        public TimeSpan Duration => TimeSpan.FromSeconds(this.jObject.duration);
        public bool IsLive => this.jObject.live_flg;
        public DateTime? DeliveryStartTimeUtc => string.IsNullOrEmpty(this.jObject.delivery_start_at) ? null : DateTime.TryParseExact(this.jObject.delivery_start_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? DeliveryStartTime => this.DeliveryStartTimeUtc.HasValue ? this.DeliveryStartTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
        public DateTime? DeliveryEndTimeUtc => string.IsNullOrEmpty(this.jObject.delivery_end_at) ? null : DateTime.TryParseExact(this.jObject.delivery_end_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? DeliveryEndTime => this.DeliveryEndTimeUtc.HasValue ? this.DeliveryEndTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
        public bool IsDelivery => this.jObject.dvr_flg;
        public bool IsReplay => this.jObject.replay_flg;
        public int MediaType => this.jObject.media_type;

        public VideoInfo(video jObject) : base(jObject) { }
    }
}
