// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json;
using System.Diagnostics;

namespace Qtyi.HiBiKiRadio.Info;

[DebuggerDisplay("{Duration}")]
public class VideoInfo : JsonObjectInfo
{
    public int ID => this.JsonObject.id;
    public TimeSpan Duration => TimeSpan.FromSeconds(this.JsonObject.duration);
    public bool IsLive => this.JsonObject.live_flg;
    public DateTime? DeliveryStartTimeUtc => string.IsNullOrEmpty(this.JsonObject.delivery_start_at) ? null : this.TryParseDateTimeUtc(this.JsonObject.delivery_start_at, out var dt) ? dt : default;
    public DateTime? DeliveryStartTime => this.DeliveryStartTimeUtc.HasValue ? UtcToLocal(this.DeliveryStartTimeUtc.Value) : null;
    public DateTime? DeliveryEndTimeUtc => string.IsNullOrEmpty(this.JsonObject.delivery_end_at) ? null : this.TryParseDateTimeUtc(this.JsonObject.delivery_end_at, out var dt) ? dt : default;
    public DateTime? DeliveryEndTime => this.DeliveryEndTimeUtc.HasValue ? UtcToLocal(this.DeliveryEndTimeUtc.Value) : null;
    public bool IsDelivery => this.JsonObject.dvr_flg;
    public bool IsReplay => this.JsonObject.replay_flg;
    public int MediaType => this.JsonObject.media_type;

    public VideoInfo(video jObject) : base(jObject) { }
}
