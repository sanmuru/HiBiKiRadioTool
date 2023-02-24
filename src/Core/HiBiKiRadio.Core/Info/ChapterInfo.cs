// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json;
using System.Diagnostics;
using System.Drawing;

namespace Qtyi.HiBiKiRadio.Info;

[DebuggerDisplay("{Name}")]
public class ChapterInfo : JsonObjectInfo
{
    internal new chapter JsonObject => (chapter)base.JsonObject;

    public int ID => this.JsonObject.id;
    public string Name => this.JsonObject.name;
    public string Description => this.JsonObject.description;
    public TimeSpan StartTime => TimeSpan.FromSeconds(this.JsonObject.start_time);
    public Uri? PCImageUri => UriConverter.ConvertFrom(this.JsonObject.pc_image_url);
    public Size? PCImageSize => SizeConverter.ConvertFrom(this.JsonObject.pc_image_info);
    public Uri? SPImageUri => UriConverter.ConvertFrom(this.JsonObject.sp_image_url);
    public Size? SPImageSize => SizeConverter.ConvertFrom(this.JsonObject.sp_image_info);

    internal ChapterInfo(chapter jObject) : base(jObject) { }
}
