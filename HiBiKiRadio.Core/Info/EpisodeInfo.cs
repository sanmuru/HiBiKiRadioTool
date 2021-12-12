using SamLu.Utility.HiBiKiRadio.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Utility.HiBiKiRadio.Info
{
    [DebuggerDisplay("{Name}")]
    public class EpisodeInfo : JsonObjectInfo<episode>
    {
        public int ID => this.jObject.id;
        public string Name => this.jObject.name;
        public int MediaType => this.jObject.media_type;
        public VideoInfo? Video => this.jObject.video is null ? null : new VideoInfo(this.jObject.video);
        public VideoInfo? AdditionalVideo => this.jObject.additional_video is null ? null : new VideoInfo(this.jObject.additional_video);
        public string HtmlDescription => this.jObject.html_description;
        public Uri LinkUri => string.IsNullOrEmpty(this.jObject.link_url) ? default : new Uri(this.jObject.link_url, UriKind.Absolute);
        public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.jObject.updated_at) ? default : DateTime.TryParseExact(this.jObject.updated_at, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt.AddHours(-9) : default(DateTime?);
        public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? this.UpdatedTimeUtc + (DateTime.Now - DateTime.UtcNow) : default;
        public EpisodePartInfo[] EpisodeParts => this.jObject.episode_parts.OrderBy(ep=>ep.sort_order).Select(ep => new EpisodePartInfo(ep)).ToArray();
        public ChapterInfo[] Chapters => this.jObject.chapters.Select(c => new ChapterInfo(c)).ToArray();

        public EpisodeInfo(episode jObject) : base(jObject) { }
    }
}