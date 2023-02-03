using System.Diagnostics.CodeAnalysis;

namespace SamLu.Utility.HiBiKiRadio.Build.Tasks;

internal sealed class EpisodeItem : InfoItem<Info.EpisodeInfo, Json.episode>
{
    private readonly Lazy<VideoItem?> _video;
    private readonly Lazy<VideoItem?> _additionalVideo;
    private readonly Lazy<EpisodePartItem[]> _episodeParts;
    private readonly Lazy<ChapterItem[]> _chapters;

    public int ID => this.info.ID;
    public string Name => this.info.Name;
    public int? MediaType => this.info.MediaType;
    public VideoItem? Video => this._video.Value;
    public VideoItem? AdditionalVideo => this._additionalVideo.Value;
    public string? HtmlDescription => this.info.HtmlDescription;
    public Uri? LinkUri => this.info.LinkUri;
    public DateTime? UpdatedTimeUtc => this.info.UpdatedTimeUtc;
    public EpisodePartItem[] EpisodeParts => this._episodeParts.Value;
    public ChapterItem[] Chapters => this._chapters.Value;

    public EpisodeItem(Info.EpisodeInfo info) : base(info)
    {
        this._video = new(() => this.info.Video is null ? null : new VideoItem(this.info.Video));
        this._additionalVideo = new(() => this.info.AdditionalVideo is null ? null : new VideoItem(this.info.AdditionalVideo));
        this._episodeParts = new(() => this.info.EpisodeParts.Select(ep => new EpisodePartItem(ep)).ToArray());
        this._chapters = new(() => this.info.Chapters.Select(ep => new ChapterItem(ep)).ToArray());
    }

    protected override string ItemSpec { get => this.Name; [DoesNotReturn] set => ThrowEditReadOnlyException(); }

    protected override List<string> MetadataNames { get; } = new()
    {
        nameof(ID),
        nameof(Name),
        nameof(MediaType),
        nameof(Video),
        nameof(AdditionalVideo),
        nameof(HtmlDescription),
        nameof(LinkUri),
        nameof(UpdatedTimeUtc),
        nameof(EpisodeParts),
        nameof(Chapters)
    };

    protected override string? GetMetadata(string metadataName) => metadataName switch
    {
        nameof(ID) => this.ID.ToString(),
        nameof(Name) => this.Name,
        nameof(MediaType) => this.MediaType?.ToString(),
        nameof(Video) => this.Video?.ID.ToString(),
        nameof(AdditionalVideo) => this.AdditionalVideo?.ID.ToString(),
        nameof(HtmlDescription) => this.HtmlDescription,
        nameof(LinkUri) => this.LinkUri?.AbsoluteUri,
        nameof(UpdatedTimeUtc) => this.UpdatedTimeUtc.HasValue ? FormatDateTime(this.UpdatedTimeUtc.Value) : default,
        nameof(EpisodeParts) => string.Join(";", this.EpisodeParts.Select(item => item.ID.ToString()).ToArray()),
        nameof(Chapters) => string.Join(";", this.Chapters.Select(item => item.ID.ToString()).ToArray()),
        _ => base.GetMetadata(metadataName)
    };

    public sealed class EqualityComparer : IEqualityComparer<EpisodeItem>
    {
        public static IEqualityComparer<EpisodeItem> Default { get; } = new EqualityComparer();

        public bool Equals(EpisodeItem? x, EpisodeItem? y)
        {
            if (x is null && y is null) return true;
            else if (x is not null && y is not null) return x.ID == y.ID;
            else return false;
        }

        public int GetHashCode(EpisodeItem obj) => obj.ID;
    }
}
