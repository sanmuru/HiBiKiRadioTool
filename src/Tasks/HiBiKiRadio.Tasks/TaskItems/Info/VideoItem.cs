// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class VideoItem : InfoItem<Info.VideoInfo, Json.video>, IVideoTaskItem
{
    public int ID => this.info.ID;
    public TimeSpan Duration => this.info.Duration;
    public bool IsLive => this.info.IsLive;
    public DateTime? DeliveryStartTimeUtc => this.info.DeliveryStartTimeUtc;
    public DateTime? DeliveryEndTimeUtc => this.info.DeliveryEndTimeUtc;
    public bool IsDelivery => this.info.IsDelivery;
    public bool IsReplay => this.info.IsReplay;
    public int MediaType => this.info.MediaType;

    public VideoItem(Info.VideoInfo info) : base(info) { }

    protected override string ItemSpec { get => this.ID.ToString(); [DoesNotReturn] set => TaskItemExtensions.ThrowEditReadOnlyException(); }

    protected override List<string> MetadataNames { get; } = new()
    {
        nameof(ID),
        nameof(Duration),
        nameof(IsLive),
        nameof(DeliveryStartTimeUtc),
        nameof(DeliveryEndTimeUtc),
        nameof(IsDelivery),
        nameof(IsReplay),
        nameof(MediaType)
    };

    protected override string? GetMetadata(string metadataName) => metadataName switch
    {
        nameof(ID) => this.ID.ToString(),
        nameof(Duration) => this.Duration.TotalSeconds.ToString(),
        nameof(IsLive) => this.IsLive.ToString().ToLowerInvariant(),
        nameof(DeliveryStartTimeUtc) => this.DeliveryStartTimeUtc.HasValue ? FormatDateTime(this.DeliveryStartTimeUtc.Value) : default,
        nameof(DeliveryEndTimeUtc) => this.DeliveryEndTimeUtc.HasValue ? FormatDateTime(this.DeliveryEndTimeUtc.Value) : default,
        nameof(IsDelivery) => this.IsDelivery.ToString().ToLowerInvariant(),
        nameof(IsReplay) => this.IsReplay.ToString().ToLowerInvariant(),
        nameof(MediaType) => this.MediaType.ToString(),
        _ => base.GetMetadata(metadataName)
    };
}
