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
    public class ProgramListTask : ApiTaskBase
    {
        public ProgramListTask() : base() { }

        #region Fetch
        protected virtual ProgramInfo[] FetchCore()
        {
            var programs = this.FetchAs<program[]>(new Uri(ApiBase, "programs"));
            var length = programs.Length;
            var infos = new ProgramInfo[length];
            for (int i = 0; i < length; i++)
                infos[i] = new(programs[i]);

            return infos;
        }

#if NET5_0_OR_GREATER
        protected virtual ProgramInfo[] FetchCore(CancellationToken cancellationToken)
        {
            var programs = this.FetchAs<program[]>(new Uri(ApiBase, "programs"), cancellationToken);
            var length = programs.Length;
            var infos = new ProgramInfo[length];
            for (int i = 0; i < length; i++)
                infos[i] = new(programs[i]);

            return infos;
        }
#endif

        public ProgramInfo[] Fetch() => this.FetchCore();

        public IAsyncResult BeginFetch(AsyncCallback? callback, object? state)
        {
#if NET35
            Func<ProgramInfo[]> handler = this.FetchCore;
            IAsyncResult asyncResult = handler.BeginInvoke(
                callback,
                state);
            return FetchAsyncResult.Create(handler, asyncResult);
#else
            return Task<ProgramInfo[]>.Factory.ContinueWhenAll(
                new[]
                {
                    this.FetchAsync(
#if NET5_0_OR_GREATER
                        state is FetchAsyncState fetchAsyncState ? fetchAsyncState.CancellationToken : CancellationToken.None
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

        public ProgramInfo[] EndFetch(IAsyncResult asyncResult)
        {
            if (asyncResult is null) throw new ArgumentNullException(nameof(asyncResult));
#if NET35
            var fetchAsyncResult = (FetchAsyncResult<Func<ProgramInfo[]>>)asyncResult;
            return fetchAsyncResult.Handler.EndInvoke(fetchAsyncResult.AsyncResult);
#else
            Debug.Assert(asyncResult is Task<ProgramInfo[]>);
            var task = (Task<ProgramInfo[]>)asyncResult;
            task.Wait();
            return task.Result;
#endif
        }

#if !NET35
        public async Task<ProgramInfo[]> FetchAsync() =>
            await Task.Run(() => this.FetchCore());

#if NET5_0_OR_GREATER
        public async Task<ProgramInfo[]> FetchAsync(CancellationToken cancellationToken) =>
            await Task.Run(() => this.FetchCore(cancellationToken));
#endif
#endif
        #endregion
    }
}
