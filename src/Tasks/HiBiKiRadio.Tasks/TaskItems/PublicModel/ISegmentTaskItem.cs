﻿// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

public interface ISegmentTaskItem : ITaskItem
{
    public int ID { get; }
    public string Name { get; }
    public ISegmentPartTaskItem[] SegmentParts { get; }
    public string HtmlDescritption { get; }
    public DateTime? PublishStartTimeUtc { get; }
    public DateTime? PublishEndTimeUtc { get; }
    public DateTime? UpdatedTimeUtc { get; }

    public class EqualityComparer<T> : IEqualityComparer<T> where T : ISegmentTaskItem
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
