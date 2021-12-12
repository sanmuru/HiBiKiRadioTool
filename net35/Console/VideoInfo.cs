using HiBikiRadioTool.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool
{
    [DebuggerDisplay("{Duration}")]
    public class VideoInfo
    {
        private readonly video jObject;

        public int ID => this.jObject.id;
        public TimeSpan Duration => TimeSpan.FromSeconds(this.jObject.duration);
        public bool IsLive => this.jObject.live_flg;
        public DateTime? DeliveryStartTimeUtc => string.IsNullOrEmpty(this.jObject.delivery_start_at) ? default : DateTime.TryParseExact(this.jObject.delivery_start_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? DeliveryStartTime => this.DeliveryStartTimeUtc.HasValue ? this.DeliveryStartTimeUtc + (DateTime.Now - DateTime.UtcNow) : default;
        public DateTime? DeliveryEndTimeUtc => string.IsNullOrEmpty(this.jObject.delivery_end_at) ? default : DateTime.TryParseExact(this.jObject.delivery_end_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? DeliveryEndTime => this.DeliveryEndTimeUtc.HasValue ? this.DeliveryEndTimeUtc + (DateTime.Now - DateTime.UtcNow) : default;
        public bool IsDelivery => this.jObject.dvr_flg;
        public bool IsReplay => this.jObject.replay_flg;
        public int MediaType => this.jObject.media_type;

        public VideoInfo(video jObject) => this.jObject = jObject ?? throw new ArgumentNullException(nameof(jObject));
    }
}
