using System.Collections.Generic;
using System.Threading.Tasks;
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

    public IAsyncEnumerable<EventLogItem> ListAllAsync()
    {
        return ParametrizedList().All();
    }

    public async Task<Page<EventLogItem>> ListFirstPage(int? pageSize = null)
    {
        return await ParametrizedList().FirstPage(pageSize);
    }

    public async Task<Page<EventLogItem>> ListPageAfter(long id, int? pageSize = null)
    {
        return await ParametrizedList().PageAfter(id, pageSize);
    }

    public async Task<Page<EventLogItem>> ListPageBefore(long id, int? pageSize = null)
    {
        return await ParametrizedList().PageBefore(id, pageSize);
    }

    private EventLogItemLister ParametrizedList()
    {
        return new EventLogItemLister(new PageFetcher<EventLogItem>(_restClient, "/event-log"));
    }
}