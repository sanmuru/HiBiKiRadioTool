// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json;
using System.Drawing;

namespace Qtyi.HiBiKiRadio.Info;

public class SegmentPartInfo : JsonObjectInfo
{
    public int ID => this.JsonObject.id;
    public string Description => this.JsonObject.description!;
    public Uri? PCImageUri => string.IsNullOrEmpty(this.JsonObject.pc_image_url) ? default : new Uri(this.JsonObject.pc_image_url);
    public Size? PCImageSize => this.JsonObject.pc_image_info is null ? default : new Size(this.JsonObject.pc_image_info.width, this.JsonObject.pc_image_info.height);
    public Uri? SPImageUri => string.IsNullOrEmpty(this.JsonObject.sp_image_url) ? default : new Uri(this.JsonObject.sp_image_url);
    public Size? SPImageSize => this.JsonObject.sp_image_info is null ? default : new Size(this.JsonObject.sp_image_info.width, this.JsonObject.sp_image_info.height);
    public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.JsonObject.updated_at) ? null : this.TryParseDateTimeUtc(this.JsonObject.updated_at, out var dt) ? dt : default;
    public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? UtcToLocal(this.UpdatedTimeUtc.Value) : null;

    public SegmentPartInfo(segment_part jObject) : base(jObject) { }
}
