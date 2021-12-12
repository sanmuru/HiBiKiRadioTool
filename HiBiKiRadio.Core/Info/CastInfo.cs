using SamLu.Utility.HiBiKiRadio.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    [DebuggerDisplay("{Name}")]
    public class CastInfo : JsonObjectInfo<cast>
    {
        public int ID => this.jObject.id;
        public string Name => this.jObject.name;
        public string RollName => this.jObject.roll_name;
        public Uri? PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url);
        public Size? PCImageSize => this.jObject.pc_image_info is null ? null : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
        public Uri? SPImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url);
        public Size? SPImageSize => this.jObject.sp_image_info is null ? null : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);
        public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.jObject.publish_start_at) ? null : DateTime.TryParseExact(this.jObject.publish_start_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? this.PublishStartTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
        public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.jObject.publish_end_at) ? null : DateTime.TryParseExact(this.jObject.publish_end_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? this.PublishEndTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
        public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.jObject.updated_at) ? null : DateTime.TryParseExact(this.jObject.updated_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? this.UpdatedTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;

        public CastInfo(cast jObject) : base(jObject) { }
    }
}
