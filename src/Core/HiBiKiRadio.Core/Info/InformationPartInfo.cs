using SamLu.Utility.HiBiKiRadio.Json;
using System;
using System.Drawing;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    public class InformationPartInfo : JsonObjectInfo<information_part>
    {
        public string Description => this.jObject.description;
        public Uri PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url);
        public Size? PCImageSize => this.jObject.pc_image_info is null ? default(Size?) : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
        public Uri SPImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url);
        public Size? SPImageSize => this.jObject.sp_image_info is null ? default(Size?) : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);

        public InformationPartInfo(information_part jObject) : base(jObject) { }
    }
}
