// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json;
using System.Diagnostics;

namespace Qtyi.HiBiKiRadio.Info;

[DebuggerDisplay("{Name}")]
public class SegmentInfo : JsonObjectInfo
{
    public int ID => this.JsonObject.id;
    public string Name => this.JsonObject.name!;
    public SegmentPartInfo[] SegmentParts => this.JsonObject.segment_parts.OrderBy(sp => sp.sort_order).Select(sp => new SegmentPartInfo(sp)).ToArray();
    public string HtmlDescritption => this.JsonObject.html_description;
    public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.JsonObject.publish_start_at) ? null : this.TryParseDateTimeUtc(this.JsonObject.publish_start_at, out var dt) ? dt : default;
    public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? UtcToLocal(this.PublishStartTimeUtc.Value) : null;
    public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.JsonObject.publish_end_at) ? null : this.TryParseDateTimeUtc(this.JsonObject.publish_end_at, out var dt) ? dt : default;
    public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? UtcToLocal(this.PublishEndTimeUtc.Value) : null;
    public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.JsonObject.updated_at) ? null : this.TryParseDateTimeUtc(this.JsonObject.updated_at, out var dt) ? dt : default;
    public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? UtcToLocal(this.UpdatedTimeUtc.Value) : null;

    public SegmentInfo(segment jObject) : base(jObject) { }
}
