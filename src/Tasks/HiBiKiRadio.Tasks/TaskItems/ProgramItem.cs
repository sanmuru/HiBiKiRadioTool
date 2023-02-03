using System.Diagnostics.CodeAnalysis;

namespace SamLu.Utility.HiBiKiRadio.Build.Tasks;

internal sealed class ProgramItem : InfoItem<Info.ProgramInfo, Json.program>
{
    public string AccessID => this.info.AccessID!;
    public int ID => this.info.ID;
    public string Name => this.info.Name!;
    public string NameKana => this.info.NameKana;
    public DayOfWeek DayOfWeek => this.info.DayOfWeek;
    public string Description => this.info.Description;
    public Uri? PCImageUri => this.info.PCImageUri;
    public Uri? SPImageUri => this.info.SPImageUri;
    public string OnAirInformation => this.info.OnAirInformation;
    public string Email => this.info.Email;
    public bool IsNewProgram => this.info.IsNewProgram;
    public string Copyright => this.info.Copyright;
    public int Priority => this.info.Priority;
    public string[] Hashtags => this.info.Hashtags;
    public string ShareText => this.info.ShareText;
    public Uri? ShareUri => this.info.ShareUri;
    public DateTime? PublishStartTimeUtc => this.info.PublishStartTimeUtc;
    public DateTime? PublishEndTimeUtc => this.info.PublishEndTimeUtc;
    public DateTime? UpdatedTimeUtc => this.info.UpdatedTimeUtc;
    public bool IsUpdate => this.info.IsUpdate;
    public EpisodeItem Episode => new(this.info.Episode);
    public bool IsChapter => this.info.IsChapter;
    public bool IsAdditionalVideo => this.info.IsAdditionalVideo;
    public int ProgramInformationCount => this.info.ProgramInformationCount;
    public int ProductInformationCount => this.info.ProductInformationCount;
    public bool IsUserFavorite => this.info.IsUserFavorite;
    public ProgramLinkItem[] ProgramLinks => this.info.ProgramLinks.Select(info => new ProgramLinkItem(info)).ToArray();
    public CastItem[] Casts => this.info.Casts.Select(info => new CastItem(info)).ToArray();
    public string[] Rolls => this.info.Casts.Select(info => info.RollName).ToArray();
    public SegmentItem[] Segments => this.info.Segments.Select(s => new SegmentItem(s)).ToArray();

    public ProgramItem(Info.ProgramInfo info) : base(info) { }

    protected override string ItemSpec { get => this.Name; [DoesNotReturn] set => ThrowEditReadOnlyException(); }

    protected override List<string> MetadataNames { get; } = new()
    {
        nameof(AccessID),
        nameof(ID),
        nameof(Name),
        nameof(NameKana),
        nameof(DayOfWeek),
        nameof(Description),
        nameof(PCImageUri),
        nameof(SPImageUri),
        nameof(OnAirInformation),
        nameof(Email),
        nameof(IsNewProgram),
        nameof(Copyright),
        nameof(Priority),
        nameof(Hashtags),
        nameof(ShareText),
        nameof(ShareUri),
        nameof(PublishStartTimeUtc),
        nameof(PublishEndTimeUtc),
        nameof(UpdatedTimeUtc),
        nameof(IsUpdate),
        nameof(Episode),
        nameof(IsChapter),
        nameof(IsAdditionalVideo),
        nameof(ProgramInformationCount),
        nameof(ProductInformationCount),
        nameof(IsUserFavorite),
        nameof(ProgramLinks),
        nameof(Casts),
        nameof(Rolls),
        nameof(Segments),
    };

    protected override string? GetMetadata(string metadataName) => metadataName switch
    {
        nameof(AccessID) => this.AccessID,
        nameof(ID) => this.ID.ToString(),
        nameof(Name) => this.Name,
        nameof(NameKana) => this.NameKana,
        nameof(DayOfWeek) => this.DayOfWeek.ToString(),
        nameof(Description) => this.Description,
        nameof(PCImageUri) => this.PCImageUri?.AbsoluteUri,
        nameof(SPImageUri) => this.SPImageUri?.AbsoluteUri,
        nameof(OnAirInformation) => this.OnAirInformation,
        nameof(Email) => this.Email,
        nameof(IsNewProgram) => this.IsNewProgram.ToString().ToLowerInvariant(),
        nameof(Copyright) => this.Copyright,
        nameof(Priority) => this.Priority.ToString(),
        nameof(Hashtags) => string.Join(" ", this.Hashtags),
        nameof(ShareText) => this.ShareText,
        nameof(ShareUri) => this.ShareUri?.AbsoluteUri,
        nameof(PublishStartTimeUtc) => this.PublishStartTimeUtc.HasValue ? FormatDateTime(this.PublishStartTimeUtc.Value) : null,
        nameof(PublishEndTimeUtc) => this.PublishEndTimeUtc.HasValue ? FormatDateTime(this.PublishEndTimeUtc.Value) : null,
        nameof(UpdatedTimeUtc) => this.UpdatedTimeUtc.HasValue ? FormatDateTime(this.UpdatedTimeUtc.Value) : null,
        nameof(IsUpdate) => this.IsUpdate.ToString().ToLowerInvariant(),
        nameof(Episode) => this.Episode.ID.ToString(),
        nameof(IsChapter) => this.IsChapter.ToString().ToLowerInvariant(),
        nameof(IsAdditionalVideo) => this.IsAdditionalVideo.ToString().ToLowerInvariant(),
        nameof(ProgramInformationCount) => this.ProgramInformationCount.ToString(),
        nameof(ProductInformationCount) => this.ProductInformationCount.ToString(),
        nameof(IsUserFavorite) => this.IsUserFavorite.ToString().ToLowerInvariant(),
        nameof(ProgramLinks) => string.Join(";", this.ProgramLinks.Select(item => item.ID.ToString()).ToArray()),
        nameof(Casts) => string.Join(";", this.Casts.Select(item => item.ID.ToString()).ToArray()),
        nameof(Rolls) => string.Join(";", this.Rolls),
        nameof(Segments) => string.Join(";", this.Segments.Select(item => item.ID.ToString()).ToArray()),
        _ => base.GetMetadata(metadataName)
    };

    public sealed class EqualityComparer : IEqualityComparer<ProgramItem>
    {
        public static IEqualityComparer<ProgramItem> Default { get; } = new EqualityComparer();

        public bool Equals(ProgramItem? x, ProgramItem? y)
        {
            if (x is null && y is null) return true;
            else if (x is not null && y is not null) return x.ID == y.ID;
            else return false;
        }

        public int GetHashCode(ProgramItem obj) => obj.ID;
    }
}
