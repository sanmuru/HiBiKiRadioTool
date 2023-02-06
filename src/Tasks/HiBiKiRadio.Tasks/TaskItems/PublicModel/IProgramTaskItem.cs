// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

public interface IProgramTaskItem : ITaskItem
{
    string AccessID { get; }
    int ID { get; }
    string Name { get; }
    string NameKana { get; }
    DayOfWeek DayOfWeek { get; }
    string Description { get; }
    Uri? PCImageUri { get; }
    Uri? SPImageUri { get; }
    string OnAirInformation { get; }
    string Email { get; }
    bool IsNewProgram { get; }
    string Copyright { get; }
    int Priority { get; }
    string[] Hashtags { get; }
    string ShareText { get; }
    Uri? ShareUri { get; }
    DateTime? PublishStartTimeUtc { get; }
    DateTime? PublishEndTimeUtc { get; }
    DateTime? UpdatedTimeUtc { get; }
    bool IsUpdate { get; }
    IEpisodeTaskItem Episode { get; }
    bool IsChapter { get; }
    bool IsAdditionalVideo { get; }
    int ProgramInformationCount { get; }
    int ProductInformationCount { get; }
    bool IsUserFavorite { get; }
    IProgramLinkTaskItem[] ProgramLinks { get; }
    ICastTaskItem[] Casts { get; }
    string[] Rolls { get; }
    ISegmentTaskItem[] Segments { get; }

    public class EqualityComparer<T> : IEqualityComparer<T> where T : IProgramTaskItem
    {
        public static IEqualityComparer<T> Default { get; } = new EqualityComparer<T>();

        public virtual bool Equals(T? x, T? y)
        {
            if (x is null && y is null) return true;
            else if (x is not null && y is not null) return x.ID == y.ID;
            else return false;
        }

        public virtual int GetHashCode(T obj) => obj.ID;
    }
}
