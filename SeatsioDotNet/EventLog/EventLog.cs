using System.Collections.Generic;
using System.Threading;
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
        return ParametrizedList().AllAsync();
    }

    public async Task<Page<EventLogItem>> ListFirstPageAsync(int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await ParametrizedList().FirstPageAsync(pageSize, cancellationToken);
    }

    public async Task<Page<EventLogItem>> ListPageAfterAsync(long id, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await ParametrizedList().PageAfterAsync(id, pageSize, cancellationToken);
    }

    public async Task<Page<EventLogItem>> ListPageBeforeAsync(long id, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await ParametrizedList().PageBeforeAsync(id, pageSize, cancellationToken);
    }

    private EventLogItemLister ParametrizedList()
    {
        return new EventLogItemLister(new PageFetcher<EventLogItem>(_restClient, "/event-log"));
    }
}