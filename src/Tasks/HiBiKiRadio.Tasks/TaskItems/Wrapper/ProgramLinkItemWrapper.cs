﻿// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class ProgramLinkItemWrapper : ItemWrapper, IProgramLinkTaskItem
{
    public int ID => this.GetInt32Metadata();

    public string Name => this.GetMetadata();

    public Uri? PCImageUri => this.GetUriMetadata();

    public Uri? SPImageUri => this.GetUriMetadata();

    public Uri? LinkUri => this.GetUriMetadata();

    public ProgramLinkItemWrapper(ITaskItem item) : base(item) { }
}
