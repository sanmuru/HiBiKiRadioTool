#if !NET35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class AsyncEnumerable
    {
        public static IEnumerable<T> Synchronize<T>(this IAsyncEnumerable<T> enumerable, CancellationToken cancellationToken = default)
        {
            if (enumerable is null) throw new ArgumentNullException(nameof(enumerable));

            var enumerator = enumerable.GetAsyncEnumerator(cancellationToken);
            while (enumerator.MoveNextAsync().AsTask().Result)
                yield return enumerator.Current;
        }
    }
}
#endif