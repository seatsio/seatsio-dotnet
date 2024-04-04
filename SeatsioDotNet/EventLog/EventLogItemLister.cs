using System.Collections.Generic;
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

    public IAsyncEnumerable<EventLogItem> All()
    {
        return new PagedEnumerable<EventLogItem>(_pageFetcher, null);
    }

    public async Task<Page<EventLogItem>> FirstPage(int? pageSize = null)
    {
        return await _pageFetcher.FetchFirstPageAsync(null, pageSize);
    }

    public async Task<Page<EventLogItem>> PageAfter(long id, int? pageSize = null)
    {
        return await _pageFetcher.FetchAfterAsync(id, null, pageSize);
    }

    public async Task<Page<EventLogItem>> PageBefore(long id, int? pageSize = null)
    {
        return await _pageFetcher.FetchBeforeAsync(id, null, pageSize);
    }
}