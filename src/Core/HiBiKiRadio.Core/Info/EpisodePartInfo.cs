using SamLu.Utility.HiBiKiRadio.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    [DebuggerDisplay("{Description}")]
    public class EpisodePartInfo : JsonObjectInfo<episode_part>
    {
        public int ID => this.jObject.id;
        public int? sort_order => this.jObject.sort_order;
        public string Description => this.jObject.description;
        public Uri PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url);
        public Size? PCImageSize => this.jObject.pc_image_info is null ? default(Size?) : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
        public Uri SPImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url);
        public Size? SPImageSize => this.jObject.sp_image_info is null ? default(Size?) : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);
        public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.jObject.updated_at) ? null : DateTime.TryParseExact(this.jObject.updated_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? this.UpdatedTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;

        public EpisodePartInfo(episode_part jObject) : base(jObject) { }
    }
}
