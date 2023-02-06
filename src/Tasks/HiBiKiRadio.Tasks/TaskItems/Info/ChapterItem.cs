// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class ChapterItem : InfoItem<Info.ChapterInfo, Json.chapter>, IChapterTaskItem
{
    public int ID => this.info.ID;
    public string Name => this.info.Name;
    public string Description => this.info.Description;
    public TimeSpan StartTime => this.info.StartTime;
    public Uri? PCImageUri => this.info.PCImageUri;
    public Uri? SPImageUri => this.info.SPImageUri;

    public ChapterItem(Info.ChapterInfo info) : base(info) { }

    protected override string ItemSpec { get => this.Name; [DoesNotReturn] set => TaskItemExtensions.ThrowEditReadOnlyException(); }

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
}
