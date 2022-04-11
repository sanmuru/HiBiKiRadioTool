using System;
using System.Threading;

namespace SamLu.Utility.HiBiKiRadio.Tasks
{
    partial class ApiTaskBase
    {
        public sealed class FetchAsyncState
        {
#if NET5_0_OR_GREATER
            public CancellationToken CancellationToken { get; }
#endif
            public object[] Arguments { get; }

            public FetchAsyncState(params object[] arguments)
#if NET5_0_OR_GREATER
                : this(CancellationToken.None, arguments) { }
#else
            {
                this.Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            }
#endif

#if NET5_0_OR_GREATER
            public FetchAsyncState(CancellationToken cancellationToken, params object[] arguments)
            {
                this.CancellationToken = cancellationToken;
                this.Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            }
#endif
        }
    }
}
