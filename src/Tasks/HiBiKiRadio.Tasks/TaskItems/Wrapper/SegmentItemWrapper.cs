// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;

namespace Qtyi.HiBiKiRadio.Build.Tasks;

internal sealed class SegmentItemWrapper : ItemWrapper, ISegmentTaskItem
{
    public int ID => this.GetInt32Metadata();

    public string Name => this.GetMetadata();

    public ISegmentPartTaskItem[] SegmentParts { get; init; }

    public string HtmlDescritption => this.GetMetadata();

    public DateTime? PublishStartTimeUtc => this.GetDateTimeMetadata();

    public DateTime? PublishEndTimeUtc => this.GetDateTimeMetadata();

    public DateTime? UpdatedTimeUtc => this.GetDateTimeMetadata();

    public SegmentItemWrapper(ITaskItem item,
        IEnumerable<ITaskItem> segmentParts) : base(item)
    {
        this.SegmentParts = this.GetListMetadata(propertyName: nameof(SegmentParts)).Select(id =>
            segmentParts.Single(_item => _item.GetMetadata(nameof(ISegmentPartTaskItem.ID)) == id).AsSegmentPartTaskItem()
        ).ToArray();
    }
}
