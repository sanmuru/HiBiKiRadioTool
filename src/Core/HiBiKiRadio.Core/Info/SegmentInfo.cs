using SamLu.Utility.HiBiKiRadio.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    [DebuggerDisplay("{Name}")]
    public class SegmentInfo : JsonObjectInfo<segment>
    {
        public int ID => this.jObject.id;
        public string Name => this.jObject.name;
        public SegmentPartInfo[] SegmentParts => this.jObject.segment_parts.Select(sp => new SegmentPartInfo(sp)).ToArray();
        public string HtmlDescritption => this.jObject.html_description;
        public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.jObject.publish_start_at) ? null : DateTime.TryParseExact(this.jObject.publish_start_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? this.PublishStartTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
        public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.jObject.publish_end_at) ? null : DateTime.TryParseExact(this.jObject.publish_end_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? this.PublishEndTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
        public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.jObject.updated_at) ? null : DateTime.TryParseExact(this.jObject.updated_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? this.UpdatedTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;

        public SegmentInfo(segment jObject) : base(jObject) { }
    }
}
