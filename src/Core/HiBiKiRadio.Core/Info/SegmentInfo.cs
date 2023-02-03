using SamLu.Utility.HiBiKiRadio.Json;
using System.Diagnostics;
using System.Globalization;

namespace SamLu.Utility.HiBiKiRadio.Info;

[DebuggerDisplay("{Name}")]
public class SegmentInfo : JsonObjectInfo<segment>
{
    public int ID => this.jObject.id;
    public string Name => this.jObject.name!;
    public SegmentPartInfo[] SegmentParts => this.jObject.segment_parts.OrderBy(sp => sp.sort_order).Select(sp => new SegmentPartInfo(sp)).ToArray();
    public string HtmlDescritption => this.jObject.html_description;
    public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.jObject.publish_start_at) ? null : this.TryParseDateTimeUtc(this.jObject.publish_start_at, out DateTime dt) ? dt : default;
    public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? UtcToLocal(this.PublishStartTimeUtc.Value) : null;
    public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.jObject.publish_end_at) ? null : this.TryParseDateTimeUtc(this.jObject.publish_end_at, out DateTime dt) ? dt : default;
    public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? UtcToLocal(this.PublishEndTimeUtc.Value) : null;
    public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.jObject.updated_at) ? null : this.TryParseDateTimeUtc(this.jObject.updated_at, out DateTime dt) ? dt : default;
    public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? UtcToLocal(this.UpdatedTimeUtc.Value) : null;

    public SegmentInfo(segment jObject) : base(jObject) { }
}
