// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class SegmentItem : InfoItem<Info.SegmentInfo, Json.segment>, ISegmentTaskItem
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

    protected override string ItemSpec { get => this.Name; [DoesNotReturn] set => TaskItemExtensions.ThrowEditReadOnlyException(); }

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

    #region ISegmentPartTaskItem
    ISegmentPartTaskItem[] ISegmentTaskItem.SegmentParts => this.SegmentParts;
    #endregion
}
