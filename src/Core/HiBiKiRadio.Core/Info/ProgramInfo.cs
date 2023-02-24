// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Qtyi.HiBiKiRadio.Info;

[DebuggerDisplay("{AccessID}")]
public class ProgramInfo : JsonObjectInfo
{
    private readonly Lazy<string[]> _hashtags;
    private readonly Lazy<ProgramLinkInfo[]> _programLinks;
    private readonly Lazy<CastInfo[]> _casts;
    private readonly Lazy<SegmentInfo[]> _segments;

    public string AccessID => this.JsonObject.access_id!;
    public int ID => this.JsonObject.id;
    public string Name => this.JsonObject.name!;
    public string NameKana => this.JsonObject.name_kana ?? string.Empty;
    public DayOfWeek DayOfWeek => (DayOfWeek)(this.JsonObject.day_of_week % 7);
    public string Description => this.JsonObject.description ?? string.Empty;
    public Uri? PCImageUri => string.IsNullOrEmpty(this.JsonObject.pc_image_url) ? default : new(this.JsonObject.pc_image_url, UriKind.Absolute);
    public Size? PCImageSize => this.JsonObject.pc_image_info is null ? default : new Size(this.JsonObject.pc_image_info.width, this.JsonObject.pc_image_info.height);
    public Uri? SPImageUri => string.IsNullOrEmpty(this.JsonObject.sp_image_url) ? default : new(this.JsonObject.sp_image_url, UriKind.Absolute);
    public Size? SPImageSize => this.JsonObject.sp_image_info is null ? default : new Size(this.JsonObject.sp_image_info.width, this.JsonObject.sp_image_info.height);
    public string OnAirInformation => this.JsonObject.onair_information ?? string.Empty;
    public string Email => this.JsonObject.email ?? string.Empty;
    public bool IsNewProgram => this.JsonObject.new_program_flg;
    public string Copyright => this.JsonObject.copyright ?? string.Empty;
    public int Priority => this.JsonObject.priority;
    public string[] Hashtags => this._hashtags.Value;
    public string ShareText => this.JsonObject.share_text ?? string.Empty;
    public Uri? ShareUri => string.IsNullOrEmpty(this.JsonObject.share_url) ? default : new(this.JsonObject.share_url);
    //public string[] Casts => Regex.Split(this.JsonObject.cast, @"\s*,\s*").Where(str => !string.IsNullOrEmpty(str)).ToArray();
    public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.JsonObject.publish_start_at) ? null : this.TryParseDateTimeUtc(this.JsonObject.publish_start_at, out var dt) ? dt : default;
    public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? UtcToLocal(this.PublishStartTimeUtc.Value) : null;
    public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.JsonObject.publish_end_at) ? null : this.TryParseDateTimeUtc(this.JsonObject.publish_end_at, out var dt) ? dt : default;
    public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? UtcToLocal(this.PublishEndTimeUtc.Value) : null;
    public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.JsonObject.updated_at) ? null : this.TryParseDateTimeUtc(this.JsonObject.updated_at, out var dt) ? dt : default;
    public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? UtcToLocal(this.UpdatedTimeUtc.Value) : null;
    public bool IsUpdate => this.JsonObject.update_flg;
    public EpisodeInfo Episode => new(this.JsonObject.episode!);
    public bool IsChapter => this.JsonObject.chapter_flg;
    public bool IsAdditionalVideo => this.JsonObject.additional_video_flg;
    public int SegmentCount => this.JsonObject.segment_count;
    public int ProgramInformationCount => this.JsonObject.program_information_count;
    public int ProductInformationCount => this.JsonObject.product_information_count;
    public bool IsUserFavorite => this.JsonObject.user_favorite_flag;
    public ProgramLinkInfo[] ProgramLinks => this._programLinks.Value;
    public CastInfo[] Casts => this._casts.Value;
    public SegmentInfo[] Segments => this._segments.Value;

    public ProgramInfo(program jObject) : base(jObject)
    {
        this._hashtags = new(() =>
            (from Match match in Regex.Matches(this.JsonObject.hash_tag ?? string.Empty, "[#＃][^#＃]+")
             let hastag = match.Value.TrimEnd()
             where hastag.Length > 1
             select $"#{hastag.Substring(1)}"
            ).ToArray(),
            isThreadSafe: true);
        this._programLinks = new(() =>
            this.JsonObject.program_links!.Select(pl => new ProgramLinkInfo(pl)).ToArray(),
            isThreadSafe: true);
        this._casts = new(() =>
            this.JsonObject.casts!.Select(c => new CastInfo(c)).ToArray(),
            isThreadSafe: true);
        this._segments = new(() =>
            this.JsonObject.segments!.Select(s => new SegmentInfo(s)).ToArray(),
            isThreadSafe: true);
    }


}
