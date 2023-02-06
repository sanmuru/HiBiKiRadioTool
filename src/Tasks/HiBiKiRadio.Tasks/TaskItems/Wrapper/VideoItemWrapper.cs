// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using Microsoft.Build.Framework;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class VideoItemWrapper : ItemWrapper, IVideoTaskItem
{
    public int ID => this.GetInt32Metadata();

    public TimeSpan Duration => this.GetTimeSpanMetadata();

    public bool IsLive => this.GetBooleanMetadata();

    public DateTime? DeliveryStartTimeUtc => this.GetDateTimeMetadata();

    public DateTime? DeliveryEndTimeUtc => this.GetDateTimeMetadata();

    public bool IsDelivery => this.GetBooleanMetadata();

    public bool IsReplay => this.GetBooleanMetadata();

    public int MediaType => this.GetInt32Metadata();

    public VideoItemWrapper(ITaskItem item) : base(item) { }
}
