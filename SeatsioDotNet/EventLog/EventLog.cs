using System.Collections.Generic;
using RestSharp;
using SeatsioDotNet.Util;

namespace SeatsioDotNet.EventLog;

public class EventLog
{
    private readonly RestClient _restClient;

    public EventLog(RestClient restClient)
    {
        _restClient = restClient;
    }

    public IEnumerable<EventLogItem> ListAll()
    {
        return ParametrizedList().All();
    }

    public Page<EventLogItem> ListFirstPage(int? pageSize = null)
    {
        return ParametrizedList().FirstPage(pageSize);
    }

    public Page<EventLogItem> ListPageAfter(long id, int? pageSize = null)
    {
        return ParametrizedList().PageAfter(id, pageSize);
    }

    public Page<EventLogItem> ListPageBefore(long id, int? pageSize = null)
    {
        return ParametrizedList().PageBefore(id, pageSize);
    }

    private EventLogItemLister ParametrizedList()
    {
        return new EventLogItemLister(new PageFetcher<EventLogItem>(_restClient, "/event-log"));
    }
}