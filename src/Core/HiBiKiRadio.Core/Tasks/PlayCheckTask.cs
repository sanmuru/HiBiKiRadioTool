using SamLu.Utility.HiBiKiRadio.Info;
using SamLu.Utility.HiBiKiRadio.Json;
using System;
using System.Diagnostics;
#if !NET35
using System.Threading;
using System.Threading.Tasks;
#endif

namespace SamLu.Utility.HiBiKiRadio.Tasks
{
    public class PlayCheckTask : ApiTaskBase
    {
        public PlayCheckTask() : base() { }

        #region Check
        protected virtual PlaylistInfo CheckCore(int id) =>
            new(
                this.FetchAs<playlist>(new Uri(ApiBase, $"videos/play_check?video_id={id}"))
            );

#if NET5_0_OR_GREATER
        protected virtual PlaylistInfo CheckCore(int id, CancellationToken cancellationToken) =>
            new(
                this.FetchAs<playlist>(new Uri(ApiBase, $"videos/play_check?video_id={id}"), cancellationToken)
            );
#endif

        public PlaylistInfo Check(int id) => this.CheckCore(id);

        public IAsyncResult BeginCheck(AsyncCallback? callback, object state)
        {
            Debug.Assert(state is FetchAsyncState);
            var fetchAsyncState = (FetchAsyncState)state;

            var arguments = fetchAsyncState.Arguments;
            Debug.Assert(arguments.Length >= 1 && arguments[0] is int);
#if NET35
            Func<int, PlaylistInfo> handler = this.CheckCore;
            IAsyncResult asyncResult = handler.BeginInvoke(
                (int)arguments[0],
                callback,
                state);
            return FetchAsyncResult.Create(handler, asyncResult);
#else
            return Task<PlaylistInfo>.Factory.ContinueWhenAll(
                new[] {
                    this.CheckAsync((int)arguments[0]
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

        public PlaylistInfo EndCheck(IAsyncResult asyncResult)
        {
            if (asyncResult is null) throw new ArgumentNullException(nameof(asyncResult));
#if NET35
            var fetchAsyncResult = (FetchAsyncResult<Func<int, PlaylistInfo>>)asyncResult;
            return fetchAsyncResult.Handler.EndInvoke(fetchAsyncResult.AsyncResult);
#else
            Debug.Assert(asyncResult is Task<PlaylistInfo>);
            var task = (Task<PlaylistInfo>)asyncResult;
            task.Wait();
            return task.Result;
#endif
        }

#if !NET35
        public async Task<PlaylistInfo> CheckAsync(int id) =>
            await Task.Run(() => this.CheckCore(id));

#if NET5_0_OR_GREATER
        public async Task<PlaylistInfo> CheckAsync(int id, CancellationToken cancellationToken) =>
            await Task.Run(() => this.CheckCore(id, cancellationToken));
#endif
#endif
        #endregion
    }
}
