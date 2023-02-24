// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Build.Framework;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal abstract class ItemWrapper : ITaskItem
{
    protected readonly ITaskItem item;

    protected ItemWrapper(ITaskItem item) => this.item = item;

    protected string GetMetadata([CallerMemberName] string? propertyName = null)
    {
        Debug.Assert(string.IsNullOrEmpty(propertyName?.Trim()));
        return this.item.GetMetadata(propertyName);
    }

    protected string[] GetListMetadata(char separator = ';', [CallerMemberName] string? propertyName = null) => this.GetMetadata(propertyName).Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);

    protected int GetInt32Metadata([CallerMemberName] string? propertyName = null) => int.Parse(this.GetMetadata(propertyName));

    protected bool GetBooleanMetadata([CallerMemberName] string? propertyName = null) => bool.Parse(this.GetMetadata(propertyName));

    protected Uri? GetUriMetadata([CallerMemberName] string? propertyName = null) => this.TryGetNoneWhitespaceMetadata(out var value, propertyName) ? new Uri(value, UriKind.RelativeOrAbsolute) : default;

    protected DateTime? GetDateTimeMetadata([CallerMemberName] string? propertyName = null) => this.TryGetNoneWhitespaceMetadata(out var value, propertyName) ? DateTime.Parse(value) : default;

    protected TimeSpan GetTimeSpanMetadata([CallerMemberName] string? propertyName = null) => TimeSpan.FromSeconds(double.Parse(this.GetMetadata(propertyName)));

    protected bool TryGetNoneEmptyMetadata(out string metadataValue, [CallerMemberName] string? propertyName = null)
    {
        metadataValue = this.GetMetadata(propertyName);
        return !string.IsNullOrEmpty(metadataValue);
    }

    protected bool TryGetNoneWhitespaceMetadata(out string metadataValue, [CallerMemberName] string? propertyName = null)
    {
        metadataValue = this.GetMetadata(propertyName);
#if NETFRAMEWORK && !NET40_OR_GREATER
        return !string.IsNullOrEmpty(propertyName?.Trim());
#else
        return !string.IsNullOrWhiteSpace(propertyName);
#endif
    }

    #region ITaskItem
    string ITaskItem.ItemSpec { get => this.item.ItemSpec; [DoesNotReturn] set => TaskItemExtensions.ThrowEditReadOnlyException(); }

    ICollection ITaskItem.MetadataNames => this.item.MetadataNames;

    int ITaskItem.MetadataCount => this.item.MetadataCount;

    IDictionary ITaskItem.CloneCustomMetadata() => this.item.CloneCustomMetadata();

    void ITaskItem.CopyMetadataTo(ITaskItem destinationItem) => this.item.CopyMetadataTo(destinationItem);

    string ITaskItem.GetMetadata(string metadataName) => this.item.GetMetadata(metadataName);

    void ITaskItem.RemoveMetadata(string metadataName) => this.item.RemoveMetadata(metadataName);

    void ITaskItem.SetMetadata(string metadataName, string metadataValue) => this.item.SetMetadata(metadataName, metadataValue);
    #endregion
}
