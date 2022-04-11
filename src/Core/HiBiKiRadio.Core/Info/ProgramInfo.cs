using SamLu.Utility.HiBiKiRadio.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    [DebuggerDisplay("{AccessID}")]
    public class ProgramInfo : JsonObjectInfo<program>
    {
        public string AccessID => this.jObject.access_id;
        public int ID => this.jObject.id;
        public string Name => this.jObject.name;
        public string NameKana => this.jObject.name_kana;
        public DayOfWeek DayOfWeek => (DayOfWeek)(this.jObject.day_of_week % 7);
        public string Description => this.jObject.description;
        public Uri PCImageUri => string.IsNullOrEmpty(this.jObject.pc_image_url) ? default : new Uri(this.jObject.pc_image_url, UriKind.Absolute);
        public Size? PCImageSize => this.jObject.pc_image_info is null ? default(Size?) : new Size(this.jObject.pc_image_info.width, this.jObject.pc_image_info.height);
        public Uri SPImageUri => string.IsNullOrEmpty(this.jObject.sp_image_url) ? default : new Uri(this.jObject.sp_image_url, UriKind.Absolute);
        public Size? SPImageSize => this.jObject.sp_image_info is null ? default(Size?) : new Size(this.jObject.sp_image_info.width, this.jObject.sp_image_info.height);
        public string OnAirInformation => this.jObject.onair_information;
        public string Email => this.jObject.email;
        public bool IsNewProgram => this.jObject.new_program_flg;
        public string Copyright => this.jObject.copyright;
        public int Priority => this.jObject.priority;
        public string[] Hashtags => (
                                     from Match match in Regex.Matches(this.jObject.hash_tag ?? string.Empty, "[#＃][^#＃]+")
                                     let hastag = match.Value.TrimEnd()
                                     where hastag.Length > 1
                                     select $"#{hastag.Substring(1)}"
                                    ).ToArray();
        public string ShareText => this.jObject.share_text;
        public Uri ShareUri => new Uri(this.jObject.share_url);
        //public string[] Casts => Regex.Split(this.jObject.cast, @"\s*,\s*").Where(str => !string.IsNullOrEmpty(str)).ToArray();
        public DateTime? PublishStartTimeUtc => string.IsNullOrEmpty(this.jObject.publish_start_at) ? null : DateTime.TryParseExact(this.jObject.publish_start_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? PublishStartTime => this.PublishStartTimeUtc.HasValue ? this.PublishStartTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
        public DateTime? PublishEndTimeUtc => string.IsNullOrEmpty(this.jObject.publish_end_at) ? null : DateTime.TryParseExact(this.jObject.publish_end_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? PublishEndTime => this.PublishEndTimeUtc.HasValue ? this.PublishEndTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
        public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.jObject.updated_at) ? null : DateTime.TryParseExact(this.jObject.updated_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? this.UpdatedTimeUtc + (DateTime.Now - DateTime.UtcNow) : null;
        public bool IsUpdate => this.jObject.update_flg;
        public EpisodeInfo Episode => new EpisodeInfo(this.jObject.episode);
        public bool IsChapter => this.jObject.chapter_flg;
        public bool IsAdditionalVideo => this.jObject.additional_video_flg;
        public int SegmentCount => this.jObject.segment_count;
        public int ProgramInformationCount => this.jObject.program_information_count;
        public int ProductInformationCount => this.jObject.product_information_count;
        public bool IsUserFavorite => this.jObject.user_favorite_flag;
        public ProgramLinkInfo[] ProgramLinks => this.jObject.program_links.Select(pl => new ProgramLinkInfo(pl)).ToArray();
        public CastInfo[] Casts => this.jObject.casts.Select(c => new CastInfo(c)).ToArray();
        public SegmentInfo[] Segments => this.jObject.segments.Select(s => new SegmentInfo(s)).ToArray();

        public ProgramInfo(program jObject) : base(jObject) { }


    }
}
