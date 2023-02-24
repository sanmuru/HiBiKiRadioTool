// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json;
using System.Diagnostics;
using System.Drawing;

namespace Qtyi.HiBiKiRadio.Info;

/// <summary>参演者信息。</summary>
[DebuggerDisplay("{Name}")]
public class CastInfo : JsonObjectInfo
{
    internal new cast JsonObject => (cast)base.JsonObject;

    /// <summary>参演者的ID。</summary>
    public int ID => this.JsonObject.id;
    /// <summary>参演者的姓名。</summary>
    public string Name => this.JsonObject.name;
    /// <summary>参演者的饰演角色的姓名。</summary>
    public string RollName => this.JsonObject.roll_name;
    /// <summary>参演者的照片链接（PC端）。</summary>
    public Uri? PCImageUri => UriConverter.ConvertFrom(this.JsonObject.pc_image_url);
    /// <summary>参演者的照片尺寸（PC端）</summary>
    public Size? PCImageSize => SizeConverter.ConvertFrom(this.JsonObject.pc_image_info);
    /// <summary>参演者的照片链接（移动端）。</summary>
    public Uri? SPImageUri => UriConverter.ConvertFrom(this.JsonObject.sp_image_url);
    /// <summary>参演者的照片尺寸（移动端）。</summary>
    public Size? SPImageSize => SizeConverter.ConvertFrom(this.JsonObject.sp_image_info);
    public DateTime? PublishStartTimeUtc => DateTimeConverter.ConvertFrom(this.JsonObject.publish_start_at);
    public DateTime? PublishStartTime => this.PublishStartTimeUtc?.ToLocalTime();
    public DateTime? PublishEndTimeUtc => DateTimeConverter.ConvertFrom(this.JsonObject.publish_end_at);
    public DateTime? PublishEndTime => this.PublishEndTimeUtc?.ToLocalTime();
    public DateTime? UpdatedTimeUtc => DateTimeConverter.ConvertFrom(this.JsonObject.updated_at);
    public DateTime? UpdatedTime => this.UpdatedTimeUtc?.ToLocalTime();

    /// <summary>
    /// 初始化<see cref="CastInfo"/>的新实例。
    /// </summary>
    /// <param name="jObject">包装的JSON对象。</param>
    internal CastInfo(cast jObject) : base(jObject) { }
}
