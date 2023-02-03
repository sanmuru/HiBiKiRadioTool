using System.Diagnostics.CodeAnalysis;

namespace SamLu.Utility.HiBiKiRadio.Build.Tasks;

internal sealed class SegmentPartItem : InfoItem<Info.SegmentPartInfo, Json.segment_part>
{
    public int ID => this.info.ID;
    public string Description => this.info.Description;
    public Uri? PCImageUri => this.PCImageUri;
    public Uri? SPImageUri => this.SPImageUri;
    public DateTime? UpdatedTimeUtc => this.UpdatedTimeUtc;

    public SegmentPartItem(Info.SegmentPartInfo info) : base(info) { }

    protected override string ItemSpec { get => this.Description; [DoesNotReturn] set => ThrowEditReadOnlyException(); }

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

    public sealed class EqualityComparer : IEqualityComparer<SegmentPartItem>
    {
        public static IEqualityComparer<SegmentPartItem> Default { get; } = new EqualityComparer();

        public bool Equals(SegmentPartItem? x, SegmentPartItem? y)
        {
            if (x is null && y is null) return true;
            else if (x is not null && y is not null) return x.ID == y.ID;
            else return false;
        }

        public int GetHashCode(SegmentPartItem obj) => obj.ID;
    }
}
