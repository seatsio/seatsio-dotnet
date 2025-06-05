using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.TicketBuyers;

public class TicketBuyers
{
    private readonly RestClient _restClient;

    public TicketBuyers(RestClient restClient)
    {
        _restClient = restClient;
    }

    public async Task<AddTicketBuyerIdsResponse> AddAsync(Guid ticketBuyerId, CancellationToken cancellationToken = default)
    {
        return await AddAsync(new[] { ticketBuyerId }, cancellationToken);
    }

    public async Task<AddTicketBuyerIdsResponse> AddAsync(IEnumerable<Guid> ticketBuyerIds, CancellationToken cancellationToken = default)
    {
        var requestBody = new Dictionary<string, object> { { "ids", ticketBuyerIds.Where(id => id != Guid.Empty) } };

        var restRequest = new RestRequest("/ticket-buyers", Method.Post)
            .AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<AddTicketBuyerIdsResponse>(restRequest, cancellationToken));
    }

    public async Task<RemoveTicketBuyerIdsResponse> RemoveAsync(Guid ticketBuyerId, CancellationToken cancellationToken = default)
    {
        return await RemoveAsync(new[] { ticketBuyerId }, cancellationToken);
    }

    public async Task<RemoveTicketBuyerIdsResponse> RemoveAsync(IEnumerable<Guid> ticketBuyerIds, CancellationToken cancellationToken = default)
    {
        var requestBody = new Dictionary<string, object> { { "ids", ticketBuyerIds.Where(id => id != Guid.Empty) } };

        var restRequest = new RestRequest("/ticket-buyers", Method.Delete)
            .AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<RemoveTicketBuyerIdsResponse>(restRequest, cancellationToken));
    }
    
    public IAsyncEnumerable<Guid> ListAllAsync()
    {
        return List().AllAsync();
    }
    
    private ParametrizedLister<Guid> List()
    {
        return new ParametrizedLister<Guid>(new PageFetcher<Guid>(_restClient, "/ticket-buyers"));
    }
}