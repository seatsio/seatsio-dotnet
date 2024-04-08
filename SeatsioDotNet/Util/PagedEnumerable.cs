using System.Collections.Generic;
using System.Threading;

namespace SeatsioDotNet.Util;

public class PagedEnumerable<T> : IAsyncEnumerable<T>
{
    private readonly PageFetcher<T> _pageFetcher;
    private readonly Dictionary<string, object> _listParams;

    public PagedEnumerable(PageFetcher<T> pageFetcher, Dictionary<string, object> listParams)
    {
        _pageFetcher = pageFetcher;
        _listParams = listParams;
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return  new PagedEnumerator<T>(_pageFetcher, _listParams, cancellationToken);
    }
}