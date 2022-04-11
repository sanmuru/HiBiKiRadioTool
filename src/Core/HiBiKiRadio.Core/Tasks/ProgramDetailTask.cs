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
    public class ProgramDetailTask : ApiTaskBase
    {
        public ProgramDetailTask() : base() { }

        #region Fetch
        protected virtual ProgramInfo FetchCore(string id)
        {
            Debug.Assert(id is not null);

            return new ProgramInfo(
                this.FetchAs<program>(new Uri(ApiBase, $"programs/{id}"))
            );
        }

#if NET5_0_OR_GREATER
        protected virtual ProgramInfo FetchCore(string id, CancellationToken cancellationToken)
        {
            Debug.Assert(id is not null);

            return new ProgramInfo(
                this.FetchAs<program>(new Uri(ApiBase, $"programs/{id}"), cancellationToken)
            );
        }
#endif

        public ProgramInfo Fetch(string id)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));

            return this.FetchCore(id);
        }

        public IAsyncResult BeginFetch(AsyncCallback? callback, object state)
        {
            Debug.Assert(state is FetchAsyncState);
            var fetchAsyncState = (FetchAsyncState)state;

            var arguments = fetchAsyncState.Arguments;
            Debug.Assert(arguments.Length >= 1 && arguments[0] is string);
#if NET35
            Func<string, ProgramInfo> handler = this.FetchCore;
            IAsyncResult asyncResult = handler.BeginInvoke(
                (string)arguments[0],
                callback,
                state);
            return FetchAsyncResult.Create(handler, asyncResult);
#else
            return Task<ProgramInfo>.Factory.ContinueWhenAll(
                new[]
                {
                    this.FetchAsync((string)arguments[0]
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

        public ProgramInfo EndFetch(IAsyncResult asyncResult)
        {
            if (asyncResult is null) throw new ArgumentNullException(nameof(asyncResult));
#if NET35
            var fetchAsyncResult = (FetchAsyncResult<Func<string, ProgramInfo>>)asyncResult;
            return fetchAsyncResult.Handler.EndInvoke(fetchAsyncResult.AsyncResult);
#else
            Debug.Assert(asyncResult is Task<ProgramInfo>);
            var task = (Task<ProgramInfo>)asyncResult;
            task.Wait();
            return task.Result;
#endif
        }

#if !NET35
        public async Task<ProgramInfo> FetchAsync(string id)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));

            return await Task.Run(() => this.FetchCore(id));
        }

#if NET5_0_OR_GREATER
        public async Task<ProgramInfo> FetchAsync(string id, CancellationToken cancellationToken)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));

            return await Task.Run(() => this.FetchCore(id, cancellationToken));
        }
#endif
#endif
        #endregion
    }
}
