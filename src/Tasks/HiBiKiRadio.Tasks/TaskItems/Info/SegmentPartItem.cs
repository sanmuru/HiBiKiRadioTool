// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class SegmentPartItem : InfoItem<Info.SegmentPartInfo, Json.segment_part>, ISegmentPartTaskItem
{
    public int ID => this.info.ID;
    public string Description => this.info.Description;
    public Uri? PCImageUri => this.PCImageUri;
    public Uri? SPImageUri => this.SPImageUri;
    public DateTime? UpdatedTimeUtc => this.UpdatedTimeUtc;

    public SegmentPartItem(Info.SegmentPartInfo info) : base(info) { }

    protected override string ItemSpec { get => this.Description; [DoesNotReturn] set => TaskItemExtensions.ThrowEditReadOnlyException(); }

    protected override List<string> MetadataNames { get; } = new()
    {
        nameof(ID),
        nameof(Description),
        nameof(PCImageUri),
        nameof(SPImageUri),
        nameof(UpdatedTimeUtc)
    };

    protected override string? GetMetadata(string metadataName) => metadataName switch
    {
        nameof(ID) => this.ID.ToString(),
        nameof(Description) => this.Description,
        nameof(PCImageUri) => this.PCImageUri?.AbsoluteUri,
        nameof(SPImageUri) => this.SPImageUri?.AbsoluteUri,
        nameof(UpdatedTimeUtc) => this.UpdatedTimeUtc.HasValue ? FormatDateTime(this.UpdatedTimeUtc.Value) : default,
        _ => base.GetMetadata(metadataName)
    };
}
