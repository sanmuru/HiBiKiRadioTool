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
    public class InformationListTask : ApiTaskBase
    {
        public InformationListTask() : base() { }

        #region Fetch
        protected virtual InformationInfo[] FetchCore()
        {
            var informations = this.FetchAs<information[]>(new Uri(ApiBase, "informations"));
            var length = informations.Length;
            var infos = new InformationInfo[length];
            for (int i = 0; i < length; i++)
                infos[i] = new(informations[i]);

            return infos;
        }

#if NET5_0_OR_GREATER
        protected virtual InformationInfo[] FetchCore(CancellationToken cancellationToken)
        {
            var informations = this.FetchAs<information[]>(new Uri(ApiBase, "informations"), cancellationToken);
            var length = informations.Length;
            var infos = new InformationInfo[informations.Length];
            for (int i = 0; i < length; i++)
                infos[i] = new(informations[i]);

            return infos;
        }
#endif

        public InformationInfo[] Fetch() => this.FetchCore();

        public IAsyncResult BeginFetch(AsyncCallback? callback, object? state)
        {
#if NET35
            Func<InformationInfo[]> handler = this.FetchCore;
            IAsyncResult asyncResult = handler.BeginInvoke(
                callback,
                state);
            return FetchAsyncResult.Create(handler, asyncResult);
#else
            return Task<InformationInfo[]>.Factory.ContinueWhenAll(
                new[] {
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

        public InformationInfo[] EndFetch(IAsyncResult asyncResult)
        {
            if (asyncResult is null) throw new ArgumentNullException(nameof(asyncResult));
#if NET35
            var fetchAsyncResult = (FetchAsyncResult<Func<InformationInfo[]>>)asyncResult;
            return fetchAsyncResult.Handler.EndInvoke(fetchAsyncResult.AsyncResult);
#else
            Debug.Assert(asyncResult is Task<InformationInfo[]>);
            var task = (Task<InformationInfo[]>)asyncResult;
            task.Wait();
            return task.Result;
#endif
        }

#if !NET35
        public async Task<InformationInfo[]> FetchAsync() =>
            await Task.Run(() => this.FetchCore());

#if NET5_0_OR_GREATER
        public async Task<InformationInfo[]> FetchAsync(CancellationToken cancellationToken) =>
            await Task.Run(() => this.FetchCore(cancellationToken));
#endif
#endif
        #endregion
    }
}
