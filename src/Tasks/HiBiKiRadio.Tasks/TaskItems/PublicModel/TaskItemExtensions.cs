// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Build.Framework;
using Qtyi.HiBiKiRadio.Info;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal static class TaskItemExtensions
{
    #region 类型包装及转换
    public static ICastTaskItem AsCastTaskItem(this CastInfo info) => new CastItem(info);
    public static ICastTaskItem AsCastTaskItem(this ITaskItem item) => item is ICastTaskItem castTaskItem ? castTaskItem : new CastItemWrapper(item);

    public static IChapterTaskItem AsChapterTaskItem(this ChapterInfo info) => new ChapterItem(info);
    public static IChapterTaskItem AsChapterTaskItem(this ITaskItem item) => item is IChapterTaskItem castTaskItem ? castTaskItem : new ChapterItemWrapper(item);

    public static IEpisodeTaskItem AsEpisodeTaskItem(this EpisodeInfo info) => new EpisodeItem(info);
    public static IEpisodeTaskItem AsEpisodeTaskItem(this ITaskItem item,
        IEnumerable<ITaskItem> additionalVideos,
        IEnumerable<ITaskItem> chapters,
        IEnumerable<ITaskItem> episodeParts,
        IEnumerable<ITaskItem> videos) => item is IEpisodeTaskItem castTaskItem ? castTaskItem : new EpisodeItemWrapper(item,
            additionalVideos: additionalVideos,
            chapters: chapters,
            episodeParts: episodeParts,
            videos: videos);

    public static IEpisodePartTaskItem AsEpisodePartTaskItem(this EpisodePartInfo info) => new EpisodePartItem(info);
    public static IEpisodePartTaskItem AsEpisodePartTaskItem(this ITaskItem item) => item is IEpisodePartTaskItem castTaskItem ? castTaskItem : new EpisodePartItemWrapper(item);

    public static IProgramTaskItem AsProgramTaskItem(this ProgramInfo info) => new ProgramItem(info);
    public static IProgramTaskItem AsProgramTaskItem(this ITaskItem item,
        IEnumerable<ITaskItem> additionalVideos,
        IEnumerable<ITaskItem> casts,
        IEnumerable<ITaskItem> chapters,
        IEnumerable<ITaskItem> episodes,
        IEnumerable<ITaskItem> episodeParts,
        IEnumerable<ITaskItem> programLinks,
        IEnumerable<ITaskItem> segments,
        IEnumerable<ITaskItem> segmentsParts,
        IEnumerable<ITaskItem> videos) => item is IProgramTaskItem castTaskItem ? castTaskItem : new ProgramItemWrapper(item,
            additionalVideos: additionalVideos,
            casts: casts,
            chapters: chapters,
            episodes: episodes,
            episodeParts: episodeParts,
            programLinks: programLinks,
            segments: segments,
            segmentParts: segmentsParts,
            videos: videos);

    public static IProgramLinkTaskItem AsProgramLinkTaskItem(this ProgramLinkInfo info) => new ProgramLinkItem(info);
    public static IProgramLinkTaskItem AsProgramLinkTaskItem(this ITaskItem item) => item is IProgramLinkTaskItem castTaskItem ? castTaskItem : new ProgramLinkItemWrapper(item);

    public static ISegmentTaskItem AsSegmentTaskItem(this SegmentInfo info) => new SegmentItem(info);
    public static ISegmentTaskItem AsSegmentTaskItem(this ITaskItem item,
        IEnumerable<ITaskItem> segmentParts) => item is ISegmentTaskItem castTaskItem ? castTaskItem : new SegmentItemWrapper(item,
            segmentParts: segmentParts);

    public static ISegmentPartTaskItem AsSegmentPartTaskItem(this SegmentPartInfo info) => new SegmentPartItem(info);
    public static ISegmentPartTaskItem AsSegmentPartTaskItem(this ITaskItem item) => item is ISegmentPartTaskItem castTaskItem ? castTaskItem : new SegmentPartItemWrapper(item);

    public static IVideoTaskItem AsVideoTaskItem(this VideoInfo info) => new VideoItem(info);
    public static IVideoTaskItem AsVideoTaskItem(this ITaskItem item) => item is IVideoTaskItem castTaskItem ? castTaskItem : new VideoItemWrapper(item);
    #endregion

#if !NET35
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
    [DoesNotReturn]
    public static void ThrowEditReadOnlyException() => throw new InvalidOperationException("此任务项是只读的。");
}
