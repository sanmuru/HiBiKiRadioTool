// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Qtyi.HiBiKiRadio.Tasks;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

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
        var additionalVideos = new HashSet<IVideoTaskItem>(IVideoTaskItem.EqualityComparer<IVideoTaskItem>.Default);
        var casts = new HashSet<ICastTaskItem>(ICastTaskItem.EqualityComparer<ICastTaskItem>.Default);
        var chapters = new HashSet<IChapterTaskItem>(IChapterTaskItem.EqualityComparer<IChapterTaskItem>.Default);
        var episodes = new HashSet<IEpisodeTaskItem>(IEpisodeTaskItem.EqualityComparer<IEpisodeTaskItem>.Default);
        var episodeParts = new HashSet<IEpisodePartTaskItem>(IEpisodePartTaskItem.EqualityComparer<IEpisodePartTaskItem>.Default);
        var programs = new HashSet<IProgramTaskItem>(IProgramTaskItem.EqualityComparer<IProgramTaskItem>.Default);
        var programLinks = new HashSet<IProgramLinkTaskItem>(IProgramLinkTaskItem.EqualityComparer<IProgramLinkTaskItem>.Default);
        var segments = new HashSet<ISegmentTaskItem>(ISegmentTaskItem.EqualityComparer<ISegmentTaskItem>.Default);
        var segmentParts = new HashSet<ISegmentPartTaskItem>(ISegmentPartTaskItem.EqualityComparer<ISegmentPartTaskItem>.Default);
        var videos = new HashSet<IVideoTaskItem>(IVideoTaskItem.EqualityComparer<IVideoTaskItem>.Default);
        foreach (var info in new ProgramListTask().FetchAsync().Result)
        {
            var _program = info.AsProgramTaskItem();
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
            if (_additionalVideo is not null) additionalVideos.Add(_additionalVideo);
            casts.UnionWith(_casts);
            chapters.UnionWith(_chapters);
            episodes.Add(_episode);
            episodeParts.UnionWith(_episodeParts);
            programs.Add(_program);
            programLinks.UnionWith(_programLinks);
            segments.UnionWith(_segments);
            segmentParts.UnionWith(_segmentParts);
            if (_video is not null) videos.Add(_video);
        }

        this.AdditionalVideos = additionalVideos.ToArray();
        this.Casts = casts.ToArray();
        this.Chapters = chapters.ToArray();
        this.Episodes = episodes.ToArray();
        this.EpisodeParts = episodeParts.ToArray();
        this.Programs = programs.ToArray();
        this.ProgramLinks = programLinks.ToArray();
        this.Segments = segments.ToArray();
        this.SegmentParts = segmentParts.ToArray();
        this.Videos = videos.ToArray();

        return true;
    }
}
