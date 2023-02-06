// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class ProgramItemWrapper : ItemWrapper, IProgramTaskItem
{
    public string AccessID => this.GetMetadata();

    public int ID => this.GetInt32Metadata();

    public string Name => this.GetMetadata();

    public string NameKana => this.GetMetadata();

    public DayOfWeek DayOfWeek => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), this.GetMetadata());

    public string Description => this.GetMetadata();

    public Uri? PCImageUri => this.GetUriMetadata();

    public Uri? SPImageUri => this.GetUriMetadata();

    public string OnAirInformation => this.GetMetadata();

    public string Email => this.GetMetadata();

    public bool IsNewProgram => this.GetBooleanMetadata();

    public string Copyright => this.GetMetadata();

    public int Priority => this.GetInt32Metadata();

    public string[] Hashtags => this.GetListMetadata(separator: ' ');

    public string ShareText => this.GetMetadata();

    public Uri? ShareUri => this.GetUriMetadata();

    public DateTime? PublishStartTimeUtc => this.GetDateTimeMetadata();

    public DateTime? PublishEndTimeUtc => this.GetDateTimeMetadata();

    public DateTime? UpdatedTimeUtc => this.GetDateTimeMetadata();

    public bool IsUpdate => this.GetBooleanMetadata();

    public IEpisodeTaskItem Episode { get; init; }

    public bool IsChapter => this.GetBooleanMetadata();

    public bool IsAdditionalVideo => this.GetBooleanMetadata();

    public int ProgramInformationCount => this.GetInt32Metadata();

    public int ProductInformationCount => this.GetInt32Metadata();

    public bool IsUserFavorite => this.GetBooleanMetadata();

    public IProgramLinkTaskItem[] ProgramLinks { get; init; }

    public ICastTaskItem[] Casts { get; init; }

    public string[] Rolls => this.GetListMetadata();

    public ISegmentTaskItem[] Segments { get; init; }

    public ProgramItemWrapper(ITaskItem item,
        IEnumerable<ITaskItem> additionalVideos,
        IEnumerable<ITaskItem> casts,
        IEnumerable<ITaskItem> chapters,
        IEnumerable<ITaskItem> episodes,
        IEnumerable<ITaskItem> episodeParts,
        IEnumerable<ITaskItem> programLinks,
        IEnumerable<ITaskItem> segments,
        IEnumerable<ITaskItem> segmentParts,
        IEnumerable<ITaskItem> videos) : base(item)
    {
        this.Casts = this.GetListMetadata().Select(id =>
            casts.Single(_item => _item.GetMetadata(nameof(ICastTaskItem.ID)) == id).AsCastTaskItem()
        ).ToArray();
        this.Episode = episodes.Single(_item => _item.GetMetadata(nameof(IEpisodeTaskItem.ID)) == this.GetMetadata(nameof(Episode))).AsEpisodeTaskItem(additionalVideos, chapters, episodeParts, videos);
        this.ProgramLinks = this.GetListMetadata().Select(id =>
            programLinks.Single(_item => _item.GetMetadata(nameof(IProgramLinkTaskItem.ID)) == id).AsProgramLinkTaskItem()
        ).ToArray();
        this.Segments = this.GetListMetadata().Select(id =>
            segments.Single(_item => _item.GetMetadata(nameof(ISegmentTaskItem.ID)) == id).AsSegmentTaskItem(segmentParts)
        ).ToArray();
    }
}
