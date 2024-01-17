using System.Collections.Generic;
using SeatsioDotNet.Util;

namespace SeatsioDotNet.EventLog;

public class EventLogItemLister
{
    private readonly PageFetcher<EventLogItem> _pageFetcher;

    public EventLogItemLister(PageFetcher<EventLogItem> pageFetcher)
    {
        _pageFetcher = pageFetcher;
    }

    public IEnumerable<EventLogItem> All()
    {
        return new PagedEnumerable<EventLogItem>(_pageFetcher, null);
    }

    public Page<EventLogItem> FirstPage(int? pageSize = null)
    {
        return _pageFetcher.FetchFirstPage(null, pageSize);
    }

    public Page<EventLogItem> PageAfter(long id, int? pageSize = null)
    {
        return _pageFetcher.FetchAfter(id, null, pageSize);
    }

    public Page<EventLogItem> PageBefore(long id, int? pageSize = null)
    {
        return _pageFetcher.FetchBefore(id, null, pageSize);
    }
}