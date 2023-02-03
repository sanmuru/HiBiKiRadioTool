using SamLu.Utility.HiBiKiRadio.Json;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SamLu.Utility.HiBiKiRadio.Info;

[DebuggerDisplay("{AccessID}")]
public class ProgramInfo : JsonObjectInfo<program>
{
    public string AccessID => this.jObject.access_id!;
    public int ID => this.jObject.id;
    public string Name => this.jObject.name!;
    public string NameKana => this.jObject.name_kana ?? string.Empty;
    public DayOfWeek DayOfWeek => (DayOfWeek)(this.jObject.day_of_week % 7);
    public string Description => this.jObject.description ?? string.Empty;
    public Uri? PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new(this.jObject.pc_image_url, UriKind.Absolute);
    public Size? PCImageSize => this.jObject.pc_image_info is null ? default(Size?) : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
    public Uri? SPImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new(this.jObject.sp_image_url, UriKind.Absolute);
    public Size? SPImageSize => this.jObject.sp_image_info is null ? default(Size?) : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);
    public string OnAirInformation => this.jObject.onair_information ?? string.Empty;
    public string Email => this.jObject.email ?? string.Empty;
    public bool IsNewProgram => this.jObject.new_program_flg;
    public string Copyright => this.jObject.copyright ?? string.Empty;
    public int Priority => this.jObject.priority;
    public string[] Hashtags => (
                                 from Match match in Regex.Matches(this.jObject.hash_tag ?? string.Empty, "[#＃][^#＃]+")
                                 let hastag = match.Value.TrimEnd()
                                 where hastag.Length > 1
                                 select $"#{hastag.Substring(1)}"
                                ).ToArray();
    public string ShareText => this.jObject.share_text ?? string.Empty;
    public Uri? ShareUri => string.IsNullOrEmpty(this.jObject.share_url) ? default : new(this.jObject.share_url);
    //public string[] Casts => Regex.Split(this.jObject.cast, @"\s*,\s*").Where(str => !string.IsNullOrEmpty(str)).ToArray();
    public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.jObject.publish_start_at) ? null : this.TryParseDateTimeUtc(this.jObject.publish_start_at, out DateTime dt) ? dt : default(DateTime?);
    public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? UtcToLocal(this.PublishStartTimeUtc.Value) : null;
    public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.jObject.publish_end_at) ? null : this.TryParseDateTimeUtc(this.jObject.publish_end_at, out DateTime dt) ? dt : default(DateTime?);
    public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? UtcToLocal(this.PublishEndTimeUtc.Value) : null;
    public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.jObject.updated_at) ? null : this.TryParseDateTimeUtc(this.jObject.updated_at, out DateTime dt) ? dt : default(DateTime?);
    public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? UtcToLocal(this.UpdatedTimeUtc.Value) : null;
    public bool IsUpdate => this.jObject.update_flg;
    public EpisodeInfo Episode => new(this.jObject.episode!);
    public bool IsChapter => this.jObject.chapter_flg;
    public bool IsAdditionalVideo => this.jObject.additional_video_flg;
    public int SegmentCount => this.jObject.segment_count;
    public int ProgramInformationCount => this.jObject.program_information_count;
    public int ProductInformationCount => this.jObject.product_information_count;
    public bool IsUserFavorite => this.jObject.user_favorite_flag;
    public ProgramLinkInfo[] ProgramLinks => this.jObject.program_links!.Select(pl => new ProgramLinkInfo(pl)).ToArray();
    public CastInfo[] Casts => this.jObject.casts!.Select(c => new CastInfo(c)).ToArray();
    public SegmentInfo[] Segments => this.jObject.segments!.Select(s => new SegmentInfo(s)).ToArray();

    public ProgramInfo(program jObject) : base(jObject) { }


}
