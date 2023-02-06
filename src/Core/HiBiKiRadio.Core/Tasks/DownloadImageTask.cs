// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace Qtyi.HiBiKiRadio.Tasks;

public class DownloadImageTask : ApiTaskBase
{
    public DownloadImageTask() : base() { }

    protected virtual void DownloadCore(Uri imageUri, string fileName, CancellationToken cancellationToken)
    {
        Debug.Assert(imageUri is not null);

        this.Client.GetFileAsync(imageUri, fileName, cancellationToken).Wait(cancellationToken);
    }

    public Task DownloadAsync(Uri imageUri, string fileName, CancellationToken cancellationToken = default) =>
        Task.Factory.StartNew(() => DownloadCore(imageUri, fileName, cancellationToken), cancellationToken);
}
