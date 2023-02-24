// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json;
using System.Diagnostics;
using System.Drawing;

namespace Qtyi.HiBiKiRadio.Info;

[DebuggerDisplay("{Name}")]
public class ProgramLinkInfo : JsonObjectInfo
{
    public int ID => this.JsonObject.id;
    public string Name => this.JsonObject.name!;
    public Uri? PCImageUri => string.IsNullOrEmpty(this.JsonObject.pc_image_url) ? default : new Uri(this.JsonObject.pc_image_url);
    public Size? PCImageSize => this.JsonObject.pc_image_info is null ? default : new Size(this.JsonObject.pc_image_info.width, this.JsonObject.pc_image_info.height);
    public Uri? SPImageUri => string.IsNullOrEmpty(this.JsonObject.sp_image_url) ? default : new Uri(this.JsonObject.sp_image_url);
    public Size? SPImageSize => this.JsonObject.sp_image_info is null ? default : new Size(this.JsonObject.sp_image_info.width, this.JsonObject.sp_image_info.height);
    public Uri? LinkUri => string.IsNullOrEmpty(this.JsonObject.link_url) ? default : new Uri(this.JsonObject.link_url);

    public ProgramLinkInfo(program_link jObject) : base(jObject) { }
}
