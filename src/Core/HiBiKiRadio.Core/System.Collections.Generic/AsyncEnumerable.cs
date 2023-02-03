#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER

namespace System.Collections.Generic;

public static class AsyncEnumerable
{
    public static IEnumerable<T> Synchronize<T>(this IAsyncEnumerable<T> enumerable, CancellationToken cancellationToken = default)
    {
        if (enumerable is null) throw new ArgumentNullException(nameof(enumerable));

        var enumerator = enumerable.GetAsyncEnumerator(cancellationToken);
        while (enumerator.MoveNextAsync().AsTask().Result)
            yield return enumerator.Current;
    }

    public static async IAsyncEnumerable<T> Asynchronize<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable is null) throw new ArgumentNullException(nameof(enumerable));

        var enumerator = enumerable.GetEnumerator();
        while (enumerator.MoveNext())
            yield return await Task.FromResult(enumerator.Current).ConfigureAwait(false);
    }
}

#endif