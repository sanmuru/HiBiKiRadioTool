// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class EpisodeItemWrapper : ItemWrapper, IEpisodeTaskItem
{
    public int ID => this.GetInt32Metadata();

    public string Name => this.GetMetadata();

    public int? MediaType => this.GetInt32Metadata();

    public IVideoTaskItem? Video { get; init; }

    public IVideoTaskItem? AdditionalVideo { get; init; }

    public string? HtmlDescription => this.GetMetadata();

    public Uri? LinkUri => this.GetUriMetadata();

    public DateTime? UpdatedTimeUtc => this.GetDateTimeMetadata();

    public IEpisodePartTaskItem[] EpisodeParts { get; init; }

    public IChapterTaskItem[] Chapters { get; init; }

    public EpisodeItemWrapper(ITaskItem item,
        IEnumerable<ITaskItem> additionalVideos,
        IEnumerable<ITaskItem> chapters,
        IEnumerable<ITaskItem> episodeParts,
        IEnumerable<ITaskItem> videos) : base(item)
    {
        this.AdditionalVideo = additionalVideos.Single(_item => _item.GetMetadata(nameof(IVideoTaskItem.ID)) == this.GetMetadata(nameof(AdditionalVideo))).AsVideoTaskItem();
        this.Chapters = this.GetListMetadata(propertyName: nameof(Chapters)).Select(id =>
            chapters.Single(_item => _item.GetMetadata(nameof(IChapterTaskItem.ID)) == id).AsChapterTaskItem()
        ).ToArray();
        this.EpisodeParts = this.GetListMetadata(propertyName: nameof(EpisodeParts)).Select(id =>
            episodeParts.Single(_item => _item.GetMetadata(nameof(IEpisodePartTaskItem.ID)) == id).AsEpisodePartTaskItem()
        ).ToArray();
        this.Video = videos.Single(_item => _item.GetMetadata(nameof(IVideoTaskItem.ID)) == this.GetMetadata(nameof(Video))).AsVideoTaskItem();
    }
}
