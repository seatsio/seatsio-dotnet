using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SeatsioDotNet.Util;

namespace SeatsioDotNet.EventLog;

public class EventLogItemLister
{
    private readonly PageFetcher<EventLogItem> _pageFetcher;

    public EventLogItemLister(PageFetcher<EventLogItem> pageFetcher)
    {
        _pageFetcher = pageFetcher;
    }

    public IAsyncEnumerable<EventLogItem> AllAsync()
    {
        return new PagedEnumerable<EventLogItem>(_pageFetcher, null);
    }

    public async Task<Page<EventLogItem>> FirstPageAsync(int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchFirstPageAsync(null, pageSize, cancellationToken);
    }

    public async Task<Page<EventLogItem>> PageAfterAsync(long id, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchAfterAsync(id, null, pageSize, cancellationToken);
    }

    public async Task<Page<EventLogItem>> PageBeforeAsync(long id, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchBeforeAsync(id, null, pageSize, cancellationToken);
    }
}