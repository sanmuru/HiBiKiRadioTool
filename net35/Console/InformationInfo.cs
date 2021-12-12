using HiBikiRadioTool.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool
{
    public class InformationInfo
    {
        private readonly information jObject;

        public int ID => this.jObject.id;
        public DateTime Day => DateTime.TryParseExact(this.jObject.day, "yyyy.MM.dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt : default;
        public string Name => this.jObject.name;
        public InformationKind Kind => (InformationKind)this.jObject.kind;
        public string KindName => this.jObject.kind_name;
        public int Priority => this.jObject.priority;
        public Uri LinkUri => string.IsNullOrEmpty(this.jObject.link_url) ? default : new Uri(this.jObject.link_url);
        public Uri PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url, UriKind.Absolute);
        public Size PCImageSize => this.jObject.pc_image_info is null ? Size.Empty : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
        public Uri SampleImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url, UriKind.Absolute);
        public Size SampleImageSize => this.jObject.sp_image_info is null ? Size.Empty : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);
        public InformationPartInfo[] InformationParts => this.jObject.information_parts.Select(ip => new InformationPartInfo(ip)).ToArray();
        public string HtmlDescription => this.jObject.html_description;
        public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.jObject.publish_start_at) ? default : DateTime.TryParseExact(this.jObject.publish_start_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? this.PublishStartTimeUtc + (DateTime.Now - DateTime.UtcNow) : default;
        public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.jObject.publish_end_at) ? default : DateTime.TryParseExact(this.jObject.publish_end_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? this.PublishEndTimeUtc + (DateTime.Now - DateTime.UtcNow) : default;
        public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.jObject.updated_at) ? default : DateTime.TryParseExact(this.jObject.updated_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? this.UpdatedTimeUtc + (DateTime.Now - DateTime.UtcNow) : default;

        public InformationInfo(information jObject) => this.jObject = jObject ?? throw new ArgumentNullException(nameof(jObject));
    }
}
