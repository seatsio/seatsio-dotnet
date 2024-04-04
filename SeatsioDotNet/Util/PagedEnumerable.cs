using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SeatsioDotNet.Util;

public class PagedEnumerable<T> : IAsyncEnumerable<T>
{
    private readonly PagedEnumerator<T> _pagedEnumerator;

    public PagedEnumerable(PageFetcher<T> pageFetcher, Dictionary<string, object> listParams)
    {
        _pagedEnumerator = new PagedEnumerator<T>(pageFetcher, listParams);
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator()
    {
        return _pagedEnumerator;
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
    {
        return GetAsyncEnumerator();
    }
}