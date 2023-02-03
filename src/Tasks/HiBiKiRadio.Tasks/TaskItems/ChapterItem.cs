using System.Diagnostics.CodeAnalysis;

namespace SamLu.Utility.HiBiKiRadio.Build.Tasks;

internal sealed class ChapterItem : InfoItem<Info.ChapterInfo, Json.chapter>
{
    public int ID => this.info.ID;
    public string Name => this.info.Name;
    public string Description => this.info.Description;
    public TimeSpan StartTime => this.info.StartTime;
    public Uri PCImageUri => this.PCImageUri;
    public Uri SPImageUri => this.SPImageUri;

    public ChapterItem(Info.ChapterInfo info) : base(info) { }

    protected override string ItemSpec { get => this.Name; [DoesNotReturn] set => ThrowEditReadOnlyException(); }

    protected override List<string> MetadataNames { get; } = new()
    {
        nameof(ID),
        nameof(Name),
        nameof(Description),
        nameof(StartTime),
        nameof(PCImageUri),
        nameof(SPImageUri)
    };

    protected override string? GetMetadata(string metadataName) => metadataName switch
    {
        nameof(ID) => this.ID.ToString(),
        nameof(Name) => this.Name,
        nameof(Description) => this.Description,
        nameof(StartTime) => this.StartTime.TotalSeconds.ToString(),
        nameof(PCImageUri) => this.PCImageUri?.AbsoluteUri,
        nameof(SPImageUri) => this.SPImageUri?.AbsoluteUri,
        _ => base.GetMetadata(metadataName)
    };

    public sealed class EqualityComparer : IEqualityComparer<ChapterItem>
    {
        public static IEqualityComparer<ChapterItem> Default { get; } = new EqualityComparer();

        public bool Equals(ChapterItem? x, ChapterItem? y)
        {
            if (x is null && y is null) return true;
            else if (x is not null && y is not null) return x.ID == y.ID;
            else return false;
        }

        public int GetHashCode(ChapterItem obj) => obj.ID;
    }
}
