// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER

using System.Text;

namespace Qtyi.HiBiKiRadio.Tasks;

partial class ApiTaskBase
{
#if !NET35
#pragma warning disable SYSLIB0014
#endif
    protected sealed class WebClient : IDownloadClient
    {
        private readonly System.Net.WebClient _client;

        public WebClient()
        {
            this._client = new();
            this._client.Headers.Add("X-Requested-With", "XMLHttpRequest");
            this._client.Encoding = Encoding.UTF8;
        }

        public WebClient(System.Net.WebClient client) => this._client = client;

        public Task<byte[]> GetDataAsync(Uri requestUri, CancellationToken cancellationToken = default) => Task.Factory.StartNew(() => this._client.DownloadData(requestUri), cancellationToken);

        public Task<string> GetStringAsync(Uri requestUri, CancellationToken cancellationToken = default) => Task.Factory.StartNew(() => this._client.DownloadString(requestUri), cancellationToken);

        public Task GetFileAsync(Uri requestUri, string fileName, CancellationToken cancellationToken = default) => Task.Factory.StartNew(() => this._client.DownloadFile(requestUri, fileName), cancellationToken);

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
