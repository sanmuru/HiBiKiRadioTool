using System.Diagnostics.CodeAnalysis;

namespace SamLu.Utility.HiBiKiRadio.Build.Tasks;

internal sealed class CastItem : InfoItem<Info.CastInfo, Json.cast>
{
    public int ID => this.info.ID;
    public string Name => this.info.Name;
    public Uri? PCImageUri => this.info.PCImageUri;
    public Uri? SPImageUri => this.info.SPImageUri;
    public DateTime? PublishStartTimeUtc => this.info.PublishStartTimeUtc;
    public DateTime? PublishEndTimeUtc => this.info.PublishEndTimeUtc;
    public DateTime? UpdatedTimeUtc => this.info.UpdatedTimeUtc;

    public CastItem(Info.CastInfo info) : base(info) { }

    protected override string ItemSpec { get => this.Name; [DoesNotReturn] set => ThrowEditReadOnlyException(); }

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

    public sealed class EqualityComparer : IEqualityComparer<CastItem>
    {
        public static IEqualityComparer<CastItem> Default { get; } = new EqualityComparer();

        public bool Equals(CastItem? x, CastItem? y)
        {
            if (x is null && y is null) return true;
            else if (x is not null && y is not null) return x.ID == y.ID;
            else return false;
        }

        public int GetHashCode(CastItem obj) => obj.ID;
    }
}
