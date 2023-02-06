// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class EpisodeItem : InfoItem<Info.EpisodeInfo, Json.episode>, IEpisodeTaskItem
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

    protected override string ItemSpec { get => this.Name; [DoesNotReturn] set => TaskItemExtensions.ThrowEditReadOnlyException(); }

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

    #region IEpisodeTaskItem
    IVideoTaskItem? IEpisodeTaskItem.Video => this.Video;

    IVideoTaskItem? IEpisodeTaskItem.AdditionalVideo => this.AdditionalVideo;

    IEpisodePartTaskItem[] IEpisodeTaskItem.EpisodeParts => this.EpisodeParts;

    IChapterTaskItem[] IEpisodeTaskItem.Chapters => this.Chapters;
    #endregion
}
