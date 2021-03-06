using SamLu.Utility.HiBiKiRadio.Json;
using System;
using System.Diagnostics;
using System.Drawing;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    [DebuggerDisplay("{Name}")]
    public class ProgramLinkInfo : JsonObjectInfo<program_link>
    {
        public int ID => this.jObject.id;
        public string Name => this.jObject.name;
        public Uri PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url);
        public Size? PCImageSize => this.jObject.pc_image_info is null ? default(Size?) : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
        public Uri SPImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url);
        public Size? SPImageSize => this.jObject.sp_image_info is null ? default(Size?) : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);
        public Uri LinkUri => string.IsNullOrEmpty(this.jObject.link_url) ? default : new Uri(this.jObject.link_url);

        public ProgramLinkInfo(program_link jObject) : base(jObject) { }
    }
}
