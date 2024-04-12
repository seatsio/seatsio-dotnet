using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SeatsioDotNet.Util;

public class ParametrizedLister<T>
{
    private readonly PageFetcher<T> _pageFetcher;

    public ParametrizedLister(PageFetcher<T> pageFetcher)
    {
        _pageFetcher = pageFetcher;
    }

    public IAsyncEnumerable<T> AllAsync(Dictionary<string, object> listParams = null)
    {
        return new PagedEnumerable<T>(_pageFetcher, listParams);
    }

    public async Task<Page<T>> FirstPageAsync(Dictionary<string, object> listParams = null, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchFirstPageAsync(listParams, pageSize, cancellationToken: cancellationToken);
    }

    public async Task<Page<T>> PageAfterAsync(long id, Dictionary<string, object> listParams = null, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchAfterAsync(id, listParams, pageSize, cancellationToken);
    }

    public async Task<Page<T>> PageBeforeAsync(long id, Dictionary<string, object> listParams = null, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchBeforeAsync(id, listParams, pageSize, cancellationToken);
    }
}