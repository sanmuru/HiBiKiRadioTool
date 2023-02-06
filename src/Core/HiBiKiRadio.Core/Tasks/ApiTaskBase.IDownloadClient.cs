// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.Tasks;

partial class ApiTaskBase
{
    public interface IDownloadClient : IDisposable
    {
        Task<byte[]> GetDataAsync(Uri requestUri, CancellationToken cancellationToken = default);
        Task<string> GetStringAsync(Uri requestUri, CancellationToken cancellationToken = default);
        Task GetFileAsync(Uri requestUri, string fileName, CancellationToken cancellationToken = default);
    }
}
