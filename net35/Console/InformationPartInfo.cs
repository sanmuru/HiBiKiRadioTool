using HiBikiRadioTool.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace HiBikiRadioTool
{
    public class InformationPartInfo
    {
        private readonly information_part jObject;

        public string Description => this.jObject.description;
        public Uri PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url);
        public Size PCImageSize => this.jObject.pc_image_info is null ? Size.Empty : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
        public Uri SampleImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url);
        public Size SampleImageSize => this.jObject.sp_image_info is null ? Size.Empty : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);

        public InformationPartInfo(information_part jObject) => this.jObject = jObject ?? throw new ArgumentNullException(nameof(jObject));
    }
}
