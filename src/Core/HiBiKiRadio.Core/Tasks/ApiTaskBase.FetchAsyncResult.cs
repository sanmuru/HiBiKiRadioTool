using System;
using System.Threading;

namespace SamLu.Utility.HiBiKiRadio.Tasks
{
    partial class ApiTaskBase
    {
        public static class FetchAsyncResult
        {
            public static FetchAsyncResult<T> Create<T>(T handler, IAsyncResult asyncResult) where T : MulticastDelegate
                => new(
                    handler ?? throw new ArgumentNullException(nameof(handler)),
                    asyncResult ?? throw new ArgumentNullException(nameof(asyncResult))
                );
        }

        public sealed class FetchAsyncResult<T> : IAsyncResult
            where T : MulticastDelegate
        {
            public T Handler { get; }

            public IAsyncResult AsyncResult { get; }

            public object? AsyncState => AsyncResult.AsyncState;

            public WaitHandle AsyncWaitHandle => AsyncResult.AsyncWaitHandle;

            public bool CompletedSynchronously => AsyncResult.CompletedSynchronously;

            public bool IsCompleted => AsyncResult.IsCompleted;

            public FetchAsyncResult(T handler, IAsyncResult asyncResult)
            {
                this.Handler = handler ?? throw new ArgumentNullException(nameof(handler));
                this.AsyncResult = asyncResult ?? throw new ArgumentNullException(nameof(asyncResult));
            }
        }
    }
}
