using HiBikiRadioTool.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool
{
    [DebuggerDisplay("{Name}")]
    public class CastInfo
    {
        private readonly cast jObject;

        public int ID => this.jObject.id;
        public string Name => this.jObject.name;
        public string RollName => this.jObject.roll_name;
        public Uri PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url);
        public Size PCImageSize => this.jObject.pc_image_info is null ? Size.Empty : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
        public Uri SampleImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url);
        public Size SampleImageSize => this.jObject.sp_image_info is null ? Size.Empty : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);
        public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.jObject.publish_start_at) ? default : DateTime.TryParseExact(this.jObject.publish_start_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? this.PublishStartTimeUtc + (DateTime.Now - DateTime.UtcNow) : default;
        public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.jObject.publish_end_at) ? default : DateTime.TryParseExact(this.jObject.publish_end_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? this.PublishEndTimeUtc + (DateTime.Now - DateTime.UtcNow) : default;
        public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.jObject.updated_at) ? default : DateTime.TryParseExact(this.jObject.updated_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? this.UpdatedTimeUtc + (DateTime.Now - DateTime.UtcNow) : default;

        public CastInfo(cast jObject) => this.jObject = jObject ?? throw new ArgumentNullException(nameof(jObject));
    }
}
