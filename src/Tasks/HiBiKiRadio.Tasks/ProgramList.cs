using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using SamLu.Utility.HiBiKiRadio.Tasks;

namespace SamLu.Utility.HiBiKiRadio.Build.Tasks;

#pragma warning disable CS8618
public class ProgramList : Task
{
    [Output]
    public ITaskItem[]? AdditionalVideos { get; set; }

    [Output]
    public ITaskItem[]? Casts { get; set; }

    [Output]
    public ITaskItem[]? Chapters { get; set; }

    [Output]
    public ITaskItem[]? Episodes { get; set; }

    [Output]
    public ITaskItem[]? EpisodeParts { get; set; }

    [Output]
    public ITaskItem[]? Programs { get; set; }

    [Output]
    public ITaskItem[]? ProgramLinks { get; set; }

    [Output]
    public ITaskItem[]? Segments { get; set; }

    [Output]
    public ITaskItem[]? SegmentParts { get; set; }

    [Output]
    public ITaskItem[]? Videos { get; set; }

    public override bool Execute()
    {
        var additionalVideos = Enumerable.Empty<VideoItem>();
        var casts = Enumerable.Empty<CastItem>();
        var chapters = Enumerable.Empty<ChapterItem>();
        var episodes = Enumerable.Empty<EpisodeItem>();
        var episodeParts = Enumerable.Empty<EpisodePartItem>();
        var programs = Enumerable.Empty<ProgramItem>();
        var programLinks = Enumerable.Empty<ProgramLinkItem>();
        var segments = Enumerable.Empty<SegmentItem>();
        var segmentParts = Enumerable.Empty<SegmentPartItem>();
        var videos = Enumerable.Empty<VideoItem>();
        foreach (var info in new ProgramListTask().FetchAsync().Result)
        {
            var _program = new ProgramItem(info);
            ProgramDetail.ExposeProgram(_program,
                out var _additionalVideo,
                out var _casts,
                out var _chapters,
                out var _episode,
                out var _episodeParts,
                out var _programLinks,
                out var _segments,
                out var _segmentParts,
                out var _video);
            if (_additionalVideo is not null) additionalVideos = additionalVideos.Concat(new[] { _additionalVideo });
            casts = casts.Concat(_casts);
            chapters = chapters.Concat(_chapters);
            episodes = episodes.Concat(new[] { _episode });
            episodeParts = episodeParts.Concat(_episodeParts);
            programs = programs.Concat(new[] { _program });
            programLinks = programLinks.Concat(_programLinks);
            segments = segments.Concat(_segments);
            segmentParts = segmentParts.Concat(_segmentParts);
            if (_video is not null) videos = videos.Concat(new[] { _video });
        }

        this.AdditionalVideos = additionalVideos.Distinct(VideoItem.EqualityComparer.Default).ToArray();
        this.Casts = casts.Distinct(CastItem.EqualityComparer.Default).ToArray();
        this.Chapters = chapters.Distinct(ChapterItem.EqualityComparer.Default).ToArray();
        this.Episodes = episodes.Distinct(EpisodeItem.EqualityComparer.Default).ToArray();
        this.EpisodeParts = episodeParts.Distinct(EpisodePartItem.EqualityComparer.Default).ToArray();
        this.Programs = programs.Distinct(ProgramItem.EqualityComparer.Default).ToArray();
        this.ProgramLinks = programLinks.Distinct(ProgramLinkItem.EqualityComparer.Default).ToArray();
        this.Segments = segments.Distinct(SegmentItem.EqualityComparer.Default).ToArray();
        this.SegmentParts = segmentParts.Distinct(SegmentPartItem.EqualityComparer.Default).ToArray();
        this.Videos = videos.Distinct(VideoItem.EqualityComparer.Default).ToArray();

        return true;
    }
}
