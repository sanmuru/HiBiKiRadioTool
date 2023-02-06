// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class CastItem : InfoItem<Info.CastInfo, Json.cast>, ICastTaskItem
{
    public int ID => this.info.ID;
    public string Name => this.info.Name;
    public Uri? PCImageUri => this.info.PCImageUri;
    public Uri? SPImageUri => this.info.SPImageUri;
    public DateTime? PublishStartTimeUtc => this.info.PublishStartTimeUtc;
    public DateTime? PublishEndTimeUtc => this.info.PublishEndTimeUtc;
    public DateTime? UpdatedTimeUtc => this.info.UpdatedTimeUtc;

    public CastItem(Info.CastInfo info) : base(info) { }

    protected override string ItemSpec { get => this.Name; [DoesNotReturn] set => TaskItemExtensions.ThrowEditReadOnlyException(); }

    protected override List<string> MetadataNames { get; } = new()
    {
        nameof(ID),
        nameof(Name),
        nameof(PCImageUri),
        nameof(SPImageUri),
        nameof(PublishStartTimeUtc),
        nameof(PublishEndTimeUtc),
        nameof(UpdatedTimeUtc),
    };

    protected override string? GetMetadata(string metadataName) => metadataName switch
    {
        nameof(ID) => this.ID.ToString(),
        nameof(Name) => this.Name,
        nameof(PCImageUri) => this.PCImageUri?.AbsoluteUri,
        nameof(SPImageUri) => this.SPImageUri?.AbsoluteUri,
        nameof(PublishStartTimeUtc) => this.PublishStartTimeUtc.HasValue ? FormatDateTime(this.PublishStartTimeUtc.Value) : default,
        nameof(PublishEndTimeUtc) => this.PublishEndTimeUtc.HasValue ? FormatDateTime(this.PublishEndTimeUtc.Value) : default,
        nameof(UpdatedTimeUtc) => this.UpdatedTimeUtc.HasValue ? FormatDateTime(this.UpdatedTimeUtc.Value) : default,
        _ => base.GetMetadata(metadataName)
    };
}
