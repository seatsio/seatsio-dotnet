using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SeatsioDotNet.Util;

public class Lister<T>
{
    private readonly PageFetcher<T> _pageFetcher;

    public Lister(PageFetcher<T> pageFetcher)
    {
        _pageFetcher = pageFetcher;
    }

    public IAsyncEnumerable<T> AllAsync(Dictionary<string, object> listParams = null)
    {
        return new PagedEnumerable<T>(_pageFetcher, listParams);
    }

    public async Task<Page<T>> FirstPageAsync(int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchFirstPageAsync(pageSize: pageSize, cancellationToken: cancellationToken);
    }

    public async Task<Page<T>> PageAfterAsync(long id, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchAfterAsync(id, pageSize: pageSize, cancellationToken: cancellationToken);
    }

    public async Task<Page<T>> PageBeforeAsync(long id, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchBeforeAsync(id, pageSize: pageSize, cancellationToken: cancellationToken);
    }
}