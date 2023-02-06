// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace Qtyi.HiBiKiRadio.Info;

/// <summary>参演者信息。</summary>
[DebuggerDisplay("{Name}")]
public class CastInfo : JsonObjectInfo<cast>
{
    /// <summary>参演者的ID。</summary>
    public int ID => this.jObject.id;
    /// <summary>参演者的姓名。</summary>
    public string Name => this.jObject.name!;
    /// <summary>参演者的饰演角色的姓名。</summary>
    public string RollName => this.jObject.roll_name!;
    /// <summary>参演者的照片链接（PC端）。</summary>
    public Uri? PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url);
    /// <summary>参演者的照片尺寸（PC端）</summary>
    public Size? PCImageSize => this.jObject.pc_image_info is null ? default(Size?) : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
    /// <summary>参演者的照片链接（移动端）。</summary>
    public Uri? SPImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url);
    /// <summary>参演者的照片尺寸（移动端）。</summary>
    public Size? SPImageSize => this.jObject.sp_image_info is null ? default(Size?) : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);
    public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.jObject.publish_start_at) ? null : this.TryParseDateTimeUtc(this.jObject.publish_start_at, out DateTime dt) ? dt : default;
    public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? UtcToLocal(this.PublishStartTimeUtc.Value) : null;
    public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.jObject.publish_end_at) ? null : this.TryParseDateTimeUtc(this.jObject.publish_end_at, out DateTime dt) ? dt : default;
    public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? UtcToLocal(this.PublishEndTimeUtc.Value) : null;
    public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.jObject.updated_at) ? null : this.TryParseDateTimeUtc(this.jObject.updated_at, out DateTime dt) ? dt : default;
    public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? UtcToLocal(this.UpdatedTimeUtc.Value) : null;

    /// <summary>
    /// 初始化<see cref="CastInfo"/>的新实例。
    /// </summary>
    /// <param name="jObject">包装的JSON对象。</param>
    public CastInfo(cast jObject) : base(jObject) { }
}
