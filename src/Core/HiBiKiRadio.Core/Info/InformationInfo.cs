// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json;
using System.Drawing;
using System.Globalization;

namespace Qtyi.HiBiKiRadio.Info;

public class InformationInfo : JsonObjectInfo
{
    public int ID => this.JsonObject.id;
    public DateTime Day => DateTime.ParseExact(this.JsonObject.day, "yyyy.MM.dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
    public string Name => this.JsonObject.name;
    public InformationKind Kind => (InformationKind)this.JsonObject.kind;
    public string KindName => this.JsonObject.kind_name;
    public int Priority => this.JsonObject.priority;
    public Uri LinkUri => string.IsNullOrEmpty(this.JsonObject.link_url) ? default : new Uri(this.JsonObject.link_url);
    public Uri PCImageUri => string.IsNullOrEmpty(this.JsonObject.pc_image_url) ? default : new Uri(this.JsonObject.pc_image_url, UriKind.Absolute);
    public Size? PCImageSize => this.JsonObject.pc_image_info is null ? default : new Size(this.JsonObject.pc_image_info.width, this.JsonObject.pc_image_info.height);
    public Uri SPImageUri => string.IsNullOrEmpty(this.JsonObject.sp_image_url) ? default : new Uri(this.JsonObject.sp_image_url, UriKind.Absolute);
    public Size? SPImageSize => this.JsonObject.sp_image_info is null ? default : new Size(this.JsonObject.sp_image_info.width, this.JsonObject.sp_image_info.height);
    public InformationPartInfo[] InformationParts => this.JsonObject.information_parts.Select(ip => new InformationPartInfo(ip)).ToArray();
    public string HtmlDescription => this.JsonObject.html_description;
    public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.JsonObject.publish_start_at) ? null : DateTime.TryParseExact(this.JsonObject.publish_start_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt.AddHours(-9) : default;
    public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? this.PublishStartTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
    public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.JsonObject.publish_end_at) ? null : DateTime.TryParseExact(this.JsonObject.publish_end_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt.AddHours(-9) : default;
    public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? this.PublishEndTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
    public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.JsonObject.updated_at) ? null : DateTime.TryParseExact(this.JsonObject.updated_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt.AddHours(-9) : default;
    public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? this.UpdatedTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;

    public InformationInfo(information jObject) : base(jObject) { }
}
