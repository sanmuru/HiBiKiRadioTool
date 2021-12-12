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
    public class ChapterInfo : JsonObjectInfo<chapter>
    {
        public int ID => this.jObject.id;
        public TimeSpan StartTime => TimeSpan.FromSeconds(this.jObject.start_time);
        public Uri? PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url);
        public Size? PCImageSize => this.jObject.pc_image_info is null ? null : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
        public Uri? SPImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url);
        public Size? SPImageSize => this.jObject.sp_image_info is null ? null : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);
        public string Name => this.jObject.name;
        public string Description => this.jObject.description;

        public ChapterInfo(chapter jObject) : base(jObject) { }
    }
}
