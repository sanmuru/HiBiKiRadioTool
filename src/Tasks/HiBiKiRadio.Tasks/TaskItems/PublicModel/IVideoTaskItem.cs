// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

public interface IVideoTaskItem : ITaskItem
{
    public int ID { get; }
    public TimeSpan Duration { get; }
    public bool IsLive { get; }
    public DateTime? DeliveryStartTimeUtc { get; }
    public DateTime? DeliveryEndTimeUtc { get; }
    public bool IsDelivery { get; }
    public bool IsReplay { get; }
    public int MediaType { get; }

    public class EqualityComparer<T> : IEqualityComparer<T> where T : IVideoTaskItem
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
