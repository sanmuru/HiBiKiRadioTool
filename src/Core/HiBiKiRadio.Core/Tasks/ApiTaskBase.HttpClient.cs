// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NET35

using System.Net.Http;

namespace Qtyi.HiBiKiRadio.Tasks;

partial class ApiTaskBase
{
    protected sealed class HttpClient : IDownloadClient
    {
        private readonly System.Net.Http.HttpClient _client;

        public HttpClient()
        {
            this._client = new(new HttpClientHandler() { UseCookies = true });
            this._client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }

        public HttpClient(System.Net.Http.HttpClient client) => this._client = client;

        public Task<byte[]> GetDataAsync(Uri requestUri, CancellationToken cancellationToken = default) =>
#if !NET5_0_OR_GREATER
            this._client.GetByteArrayAsync(requestUri);
#else
            this._client.GetByteArrayAsync(requestUri, cancellationToken);
#endif

        public Task<string> GetStringAsync(Uri requestUri, CancellationToken cancellationToken = default) =>
#if !NET5_0_OR_GREATER
            this._client.GetStringAsync(requestUri);
#else
            this._client.GetStringAsync(requestUri, cancellationToken);
#endif

        public async Task GetFileAsync(Uri requestUri, string fileName, CancellationToken cancellationToken = default)
        {
            using var response =
#if !NET5_0_OR_GREATER
                await this._client.GetStreamAsync(requestUri).ConfigureAwait(false);
#else
                await this._client.GetStreamAsync(requestUri, cancellationToken).ConfigureAwait(false);
#endif
            using var file = File.Create(fileName);

            const int bufferSize = 81920;
            await response.CopyToAsync(file, bufferSize, cancellationToken).ConfigureAwait(false);
        }

        #region IDisposable
        private bool disposedValue;
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._client.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

#endif
