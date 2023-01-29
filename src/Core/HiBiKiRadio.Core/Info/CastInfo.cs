using SamLu.Utility.HiBiKiRadio.Json;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace SamLu.Utility.HiBiKiRadio.Info;

/// <summary>参演者信息。</summary>
[DebuggerDisplay("{Name}")]
public class CastInfo : JsonObjectInfo<cast>
{
    /// <summary>参演者的ID。</summary>
    public int ID => this.jObject.id;
    /// <summary>参演者的姓名。</summary>
    public string? Name => this.jObject.name;
    /// <summary>参演者的饰演角色的姓名。</summary>
    public string? RollName => this.jObject.roll_name;
    /// <summary>参演者的照片链接（PC端）。</summary>
    public Uri? PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url);
    /// <summary>参演者的照片尺寸（PC端）</summary>
    public Size? PCImageSize => this.jObject.pc_image_info is null ? default(Size?) : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
    /// <summary>参演者的照片链接（移动端）。</summary>
    public Uri? SPImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url);
    /// <summary>参演者的照片尺寸（移动端）。</summary>
    public Size? SPImageSize => this.jObject.sp_image_info is null ? default(Size?) : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);
    public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.jObject.publish_start_at) ? null : DateTime.TryParseExact(this.jObject.publish_start_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
    public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? this.PublishStartTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
    public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.jObject.publish_end_at) ? null : DateTime.TryParseExact(this.jObject.publish_end_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
    public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? this.PublishEndTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
    public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.jObject.updated_at) ? null : DateTime.TryParseExact(this.jObject.updated_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
    public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? this.UpdatedTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;

    /// <summary>
    /// 初始化<see cref="CastInfo"/>的新实例。
    /// </summary>
    /// <param name="jObject">包装的JSON对象。</param>
    public CastInfo(cast jObject) : base(jObject) { }
}
