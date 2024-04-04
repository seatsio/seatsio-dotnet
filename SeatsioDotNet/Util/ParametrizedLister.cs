using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeatsioDotNet.Util;

public class ParametrizedLister<T>
{
    private readonly PageFetcher<T> _pageFetcher;

    public ParametrizedLister(PageFetcher<T> pageFetcher)
    {
        _pageFetcher = pageFetcher;
    }

    public IAsyncEnumerable<T> All(Dictionary<string, object> listParams = null)
    {
        return new PagedEnumerable<T>(_pageFetcher, listParams);
    }

    public async Task<Page<T>> FirstPage(Dictionary<string, object> listParams = null, int? pageSize = null)
    {
        return await _pageFetcher.FetchFirstPageAsync(listParams, pageSize);
    }

    public async Task<Page<T>> PageAfter(long id, Dictionary<string, object> listParams = null, int? pageSize = null)
    {
        return await _pageFetcher.FetchAfterAsync(id, listParams, pageSize);
    }

    public async Task<Page<T>> PageBefore(long id, Dictionary<string, object> listParams = null, int? pageSize = null)
    {
        return await _pageFetcher.FetchBeforeAsync(id, listParams, pageSize);
    }
}