// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;
using Qtyi.HiBiKiRadio.Info;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal abstract class InfoItem<TInfo, TJsonObject> : ITaskItem
    where TInfo : notnull, JsonObjectInfo<TJsonObject>
    where TJsonObject : notnull
{
    protected TInfo info;

    protected InfoItem(TInfo info) => this.info = info;

    protected abstract string ItemSpec { get; set; }
    protected abstract List<string> MetadataNames { get; }
    protected virtual int MetadataCount => this.MetadataNames.Count;

    protected virtual Dictionary<string, string?> CloneCustomMetadata() => this.MetadataNames.ToDictionary(static name => name, ((ITaskItem)this).GetMetadata);
    protected virtual void CopyMetadataTo(ITaskItem destinationItem) => this.MetadataNames.ForEach(name =>
    {
        var value = this.GetMetadata(name);
        if (value is not null)
            destinationItem.SetMetadata(name, value);
    });

    protected virtual string? GetMetadata(string metadataName)
    {
#if DEBUG
        if (this.MetadataNames.Contains(metadataName))
            Debug.Fail($"未处理的已知元数据名称：{metadataName}");
#endif
        return null;
    }
    [DoesNotReturn]
    protected virtual void SetMetadata(string metadataName, string metadataValue) => ThrowEditReadOnlyException();
    [DoesNotReturn]
    protected virtual void RemoveMetadata(string metadataName) => ThrowEditReadOnlyException();

    protected string FormatDateTime(DateTime dateTime) => JsonObjectInfo<TJsonObject>.FormatDateTime(this.info, dateTime);
#if !NET35
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
    [DoesNotReturn]
    protected static void ThrowEditReadOnlyException() => throw new InvalidOperationException("此任务项是只读的。");

    #region ITaskItem
    string ITaskItem.ItemSpec { get => this.ItemSpec; set => this.ItemSpec = value; }
    ICollection ITaskItem.MetadataNames => this.MetadataNames;
    int ITaskItem.MetadataCount => this.MetadataCount;
    IDictionary ITaskItem.CloneCustomMetadata() => this.CloneCustomMetadata();
    void ITaskItem.CopyMetadataTo(ITaskItem destinationItem) => this.CopyMetadataTo(destinationItem);
    string ITaskItem.GetMetadata(string metadataName) => this.GetMetadata(metadataName) ?? string.Empty;
    void ITaskItem.RemoveMetadata(string metadataName) => this.RemoveMetadata(metadataName);
    void ITaskItem.SetMetadata(string metadataName, string metadataValue) => this.SetMetadata(metadataName, metadataValue ?? throw new ArgumentNullException(metadataValue));
    #endregion
}
