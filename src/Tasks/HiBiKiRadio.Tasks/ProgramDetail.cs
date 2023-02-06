// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Qtyi.HiBiKiRadio.Tasks;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

#pragma warning disable CS8618
public class ProgramDetail : Task
{
    [Required]
    public string ID { get; set; }

    [Output]
    public ITaskItem? AdditionalVideo { get; set; }

    [Output]
    public ITaskItem[]? Casts { get; set; }

    [Output]
    public ITaskItem[]? Chapters { get; set; }

    [Output]
    public ITaskItem? Episode { get; set; }

    [Output]
    public ITaskItem[]? EpisodeParts { get; set; }

    [Output]
    public ITaskItem? Program { get; set; }

    [Output]
    public ITaskItem[]? ProgramLinks { get; set; }

    [Output]
    public ITaskItem[]? Segments { get; set; }

    [Output]
    public ITaskItem[]? SegmentParts { get; set; }

    [Output]
    public ITaskItem? Video { get; set; }

    public override bool Execute()
    {
        var program = new ProgramDetailTask().FetchAsync(ID).Result.AsProgramTaskItem();
        ExposeProgram(program,
            out var additionalVideo,
            out var casts,
            out var chapters,
            out var episode,
            out var episodeParts,
            out var programLinks,
            out var segments,
            out var segmentParts,
            out var video);
        this.AdditionalVideo = additionalVideo;
        this.Casts = casts;
        this.Chapters = chapters;
        this.Episode = episode;
        this.EpisodeParts = episodeParts;
        this.Program = program;
        this.ProgramLinks = programLinks;
        this.Segments = segments;
        this.SegmentParts = segmentParts;
        this.Video = video;

        return true;
    }

    public static void ExposeProgram(IProgramTaskItem program,
        out IVideoTaskItem? additionalVideo,
        out ICastTaskItem[] casts,
        out IChapterTaskItem[] chapters,
        out IEpisodeTaskItem episode,
        out IEpisodePartTaskItem[] episodeParts,
        out IProgramLinkTaskItem[] programLinks,
        out ISegmentTaskItem[] segments,
        out ISegmentPartTaskItem[] segmentParts,
        out IVideoTaskItem? video)
    {
        casts = program.Casts;
        episode = program.Episode;
        ExposeEpisode(program.Episode, out additionalVideo, out chapters, out episodeParts, out video);
        programLinks = program.ProgramLinks;
        segments = program.Segments;
        ExposeSegments(program.Segments, out segmentParts);
    }

    public static void ExposeEpisode(IEpisodeTaskItem episode,
        out IVideoTaskItem? additionalVideo,
        out IChapterTaskItem[] chapters,
        out IEpisodePartTaskItem[] episodeParts,
        out IVideoTaskItem? video)
    {
        additionalVideo = episode.AdditionalVideo;
        chapters = episode.Chapters;
        episodeParts = episode.EpisodeParts;
        video = episode.Video;
    }

    public static void ExposeSegments(ISegmentTaskItem[] segments,
        out ISegmentPartTaskItem[] segmentParts)
    {
        segmentParts = segments.SelectMany(item => item.SegmentParts).ToArray();
    }
}
