using System;
using System.Diagnostics;
#if !NET35
using System.Threading;
using System.Threading.Tasks;
#endif

namespace SamLu.Utility.HiBiKiRadio.Tasks
{
    public class DownloadImageTask : ApiTaskBase
    {
        public DownloadImageTask() : base() { }

        #region Download
        protected virtual byte[] DownloadCore(Uri imageUri)
        {
            Debug.Assert(imageUri is not null);

            return this.FetchData(imageUri);
        }

#if NET5_0_OR_GREATER
        protected virtual byte[] DownloadCore(Uri imageUri, CancellationToken cancellationToken)
        {
            Debug.Assert(imageUri is not null);

            return this.FetchData(imageUri, cancellationToken);
        }
#endif

        public byte[] Download(Uri imageUri)
        {
            if (imageUri is null) throw new ArgumentNullException(nameof(imageUri));
            
            return this.DownloadCore(imageUri);
        }

        public IAsyncResult BeginDownload(AsyncCallback? callback, object state)
        {
            Debug.Assert(state is FetchAsyncState);
            var fetchAsyncState = (FetchAsyncState)state;

            var arguments = fetchAsyncState.Arguments;
            Debug.Assert(arguments.Length >= 1 && arguments[0] is Uri);
#if NET35
            Func<Uri, byte[]> handler = this.DownloadCore;
            IAsyncResult asyncResult = handler.BeginInvoke(
                (Uri)arguments[0],
                callback,
                state);
            return FetchAsyncResult.Create(handler, asyncResult);
#else
            return Task<byte[]>.Factory.ContinueWhenAll(
                new[] {
                    this.DownloadAsync((Uri)arguments[0]
#if NET5_0_OR_GREATER
                        , fetchAsyncState.CancellationToken
#endif
                    )
                },
                tasks =>
                {
                    var task = tasks[0];
                    callback?.Invoke(task);
                    return task.Result;
                }
            );
#endif
        }

        public byte[] EndDownload(IAsyncResult asyncResult)
        {
            if (asyncResult is null) throw new ArgumentNullException(nameof(asyncResult));
#if NET35
            var fetchAsyncResult = (FetchAsyncResult<Func<Uri, byte[]>>)asyncResult;
            return fetchAsyncResult.Handler.EndInvoke(fetchAsyncResult.AsyncResult);
#else
            Debug.Assert(asyncResult is Task<byte[]>);
            var task = (Task<byte[]>)asyncResult;
            task.Wait();
            return task.Result;
#endif
        }

#if !NET35
        public async Task<byte[]> DownloadAsync(Uri imageUri)
        {
            if (imageUri is null) throw new ArgumentNullException(nameof(imageUri));

            return await Task.Run(() => this.DownloadCore(imageUri));
        }

#if NET5_0_OR_GREATER
        public async Task<byte[]> DownloadAsync(Uri imageUri, CancellationToken cancellationToken)
        {
            if (imageUri is null) throw new ArgumentNullException(nameof(imageUri));

            return await Task.Run(() => this.DownloadCore(imageUri, cancellationToken));
        }
#endif
#endif
        #endregion
    }
}
