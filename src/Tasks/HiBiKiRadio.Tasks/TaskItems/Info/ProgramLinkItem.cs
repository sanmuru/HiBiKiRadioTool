// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class ProgramLinkItem : InfoItem<Info.ProgramLinkInfo, Json.program_link>, IProgramLinkTaskItem
{
    public int ID => this.info.ID;
    public string Name => this.info.Name;
    public Uri? PCImageUri => this.info.PCImageUri;
    public Uri? SPImageUri => this.SPImageUri;
    public Uri? LinkUri => this.LinkUri;

    public ProgramLinkItem(Info.ProgramLinkInfo info) : base(info) { }

    protected override string ItemSpec { get => this.Name; [DoesNotReturn] set => TaskItemExtensions.ThrowEditReadOnlyException(); }

    protected override List<string> MetadataNames { get; } = new()
    {
        nameof(ID),
        nameof(Name),
        nameof(PCImageUri),
        nameof(SPImageUri),
        nameof(LinkUri)
    };

    protected override string? GetMetadata(string metadataName) => metadataName switch
    {
        nameof(ID) => this.ID.ToString(),
        nameof(Name) => this.Name,
        nameof(PCImageUri) => this.PCImageUri?.AbsoluteUri,
        nameof(SPImageUri) => this.SPImageUri?.AbsoluteUri,
        nameof(LinkUri) => this.LinkUri?.AbsoluteUri,
        _ => base.GetMetadata(metadataName)
    };
}
