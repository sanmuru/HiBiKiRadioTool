using SamLu.Utility.HiBiKiRadio.Json;
using System.Diagnostics;
using System.Drawing;

namespace SamLu.Utility.HiBiKiRadio.Info;

[DebuggerDisplay("{Name}")]
public class ChapterInfo : JsonObjectInfo<chapter>
{
    public int ID => this.jObject.id;
    public string Name => this.jObject.name!;
    public string Description => this.jObject.description!;
    public TimeSpan StartTime => TimeSpan.FromSeconds(this.jObject.start_time);
    public Uri? PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url);
    public Size? PCImageSize => this.jObject.pc_image_info is null ? default(Size?) : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
    public Uri? SPImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url);
    public Size? SPImageSize => this.jObject.sp_image_info is null ? default(Size?) : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);

    public ChapterInfo(chapter jObject) : base(jObject) { }
}
