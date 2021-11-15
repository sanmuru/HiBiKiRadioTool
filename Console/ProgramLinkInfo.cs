using HiBikiRadioTool.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool
{
    [DebuggerDisplay("{Name}")]
    public class ProgramLinkInfo
    {
        private readonly program_link jObject;

        public int ID => this.jObject.id;
        public string Name => this.jObject.name;
        public Uri PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url);
        public Size PCImageSize => this.jObject.pc_image_info is null ? Size.Empty : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
        public Uri SampleImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url);
        public Size SampleImageSize => this.jObject.sp_image_info is null ? Size.Empty : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);
        public Uri LinkUri => string.IsNullOrEmpty(this.jObject.link_url) ? default : new Uri(this.jObject.link_url);

        public ProgramLinkInfo(program_link jObject) => this.jObject = jObject ?? throw new ArgumentNullException(nameof(jObject));
    }
}
