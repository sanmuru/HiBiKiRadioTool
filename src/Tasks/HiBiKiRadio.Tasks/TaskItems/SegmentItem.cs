﻿using System.Diagnostics.CodeAnalysis;

namespace SamLu.Utility.HiBiKiRadio.Build.Tasks;

internal sealed class SegmentItem : InfoItem<Info.SegmentInfo, Json.segment>
{
    private readonly Lazy<SegmentPartItem[]> _segmentParts;

    public int ID => this.info.ID;
    public string Name => this.info.Name;
    public SegmentPartItem[] SegmentParts => this._segmentParts.Value;
    public string HtmlDescritption => this.info.HtmlDescritption;
    public DateTime? PublishStartTimeUtc => this.info.PublishStartTimeUtc;
    public DateTime? PublishEndTimeUtc => this.info.PublishEndTimeUtc;
    public DateTime? UpdatedTimeUtc => this.info.UpdatedTimeUtc;

    public SegmentItem(Info.SegmentInfo info) : base(info)
    {
        this._segmentParts = new(() => this.info.SegmentParts.Select(sp => new SegmentPartItem(sp)).ToArray());
    }

    protected override string ItemSpec { get => this.Name; [DoesNotReturn] set => ThrowEditReadOnlyException(); }

    protected override List<string> MetadataNames { get; } = new()
    {
        nameof(ID),
        nameof(Name),
        nameof(SegmentParts),
        nameof(HtmlDescritption),
        nameof(PublishStartTimeUtc),
        nameof(PublishEndTimeUtc),
        nameof(UpdatedTimeUtc)
    };

    protected override string? GetMetadata(string metadataName) => metadataName switch
    {
        nameof(ID) => this.ID.ToString(),
        nameof(Name) => this.Name,
        nameof(SegmentParts) => string.Join(";", this.SegmentParts.Select(item => item.ID.ToString()).ToArray()),
        nameof(HtmlDescritption) => this.HtmlDescritption,
        nameof(PublishStartTimeUtc) => this.PublishStartTimeUtc.HasValue ? FormatDateTime(this.PublishStartTimeUtc.Value) : default,
        nameof(PublishEndTimeUtc) => this.PublishEndTimeUtc.HasValue ? FormatDateTime(this.PublishEndTimeUtc.Value) : default,
        nameof(UpdatedTimeUtc) => this.UpdatedTimeUtc.HasValue ? FormatDateTime(this.UpdatedTimeUtc.Value) : default,
        _ => base.GetMetadata(metadataName)
    };

    public sealed class EqualityComparer : IEqualityComparer<SegmentItem>
    {
        public static IEqualityComparer<SegmentItem> Default { get; } = new EqualityComparer();

        public bool Equals(SegmentItem? x, SegmentItem? y)
        {
            if (x is null && y is null) return true;
            else if (x is not null && y is not null) return x.ID == y.ID;
            else return false;
        }

        public int GetHashCode(SegmentItem obj) => obj.ID;
    }
}