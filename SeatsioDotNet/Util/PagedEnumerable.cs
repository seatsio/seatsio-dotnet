using System.Collections;
using System.Collections.Generic;

namespace SeatsioDotNet.Util;

public class PagedEnumerable<T> : IEnumerable<T>
{
    private readonly PagedEnumerator<T> _pagedEnumerator;

    public PagedEnumerable(PageFetcher<T> pageFetcher, Dictionary<string, object> listParams)
    {
        _pagedEnumerator = new PagedEnumerator<T>(pageFetcher, listParams);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _pagedEnumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}