using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using SeatsioDotNet.Events;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Seasons;

public class Seasons
{
    private readonly RestClient _restClient;
    private readonly SeatsioClient _seatsioClient;

    public Seasons(RestClient restClient, SeatsioClient seatsioClient)
    {
        _restClient = restClient;
        _seatsioClient = seatsioClient;
    }

    public async Task<Event> CreateAsync(string chartKey, string key = null, int? numberOfEvents = null, IEnumerable<string> eventKeys = null, TableBookingConfig tableBookingConfig = null, IEnumerable<Channel> channels = null, ForSaleConfig forSaleConfig = null)
    {
        Dictionary<string, object> requestBody = new Dictionary<string, object>();
        requestBody.Add("chartKey", chartKey);
            
        if (key != null)
        {
            requestBody.Add("key", key);
        }

        if (numberOfEvents != null)
        {
            requestBody.Add("numberOfEvents", numberOfEvents);
        }
            
        if (eventKeys != null)
        {
            requestBody.Add("eventKeys", eventKeys);
        }
            
        if (tableBookingConfig != null)
        {
            requestBody.Add("tableBookingConfig", tableBookingConfig.AsJsonObject());
        }    
            
        if (channels != null)
        {
            requestBody.Add("channels", channels);
        }
            
        if (forSaleConfig != null)
        {
            requestBody.Add("forSaleConfig", forSaleConfig.AsJsonObject());
        }
            
        var restRequest = new RestRequest("/seasons", Method.Post).AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<Event>(restRequest));
    }

    public async Task<Event> CreatePartialSeasonAsync(string topLevelSeasonKey, string partialSeasonKey = null, IEnumerable<string> eventKeys = null)
    {
        Dictionary<string, object> requestBody = new Dictionary<string, object>();
            
        if (partialSeasonKey != null)
        {
            requestBody.Add("key", partialSeasonKey);
        }
            
        if (eventKeys != null)
        {
            requestBody.Add("eventKeys", eventKeys);
        }

        var restRequest = new RestRequest("/seasons/{topLevelSeasonKey}/partial-seasons", Method.Post)
            .AddUrlSegment("topLevelSeasonKey", topLevelSeasonKey)
            .AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<Event>(restRequest));
    }

    public async Task<Event> RetrieveAsync(string key)
    {
        return await _seatsioClient.Events.RetrieveAsync(key);
    }

    public async Task<Event> AddEventsToPartialSeasonAsync(string topLevelSeasonKey, string partialSeasonKey, string[] eventKeys)
    {
        Dictionary<string, object> requestBody = new Dictionary<string, object>();
        requestBody.Add("eventKeys", eventKeys);
        var restRequest = new RestRequest("/seasons/{topLevelSeasonKey}/partial-seasons/{partialSeasonKey}/actions/add-events", Method.Post)
            .AddUrlSegment("topLevelSeasonKey", topLevelSeasonKey)
            .AddUrlSegment("partialSeasonKey", partialSeasonKey)
            .AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<Event>(restRequest));
    }

    public async Task<Event> RemoveEventFromPartialSeasonAsync(string topLevelSeasonKey, string partialSeasonKey, string eventKey)
    {
        var restRequest = new RestRequest("/seasons/{topLevelSeasonKey}/partial-seasons/{partialSeasonKey}/events/{eventKey}", Method.Delete)
            .AddUrlSegment("topLevelSeasonKey", topLevelSeasonKey)
            .AddUrlSegment("partialSeasonKey", partialSeasonKey)
            .AddUrlSegment("eventKey", eventKey);
        return AssertOk(await _restClient.ExecuteAsync<Event>(restRequest));
    }

    public async Task<Event[]> CreateEventsAsync(string key, string[] eventKeys = null, int? numberOfEvents = null)
    {
        Dictionary<string, object> requestBody = new Dictionary<string, object>();

        if (numberOfEvents != null)
        {
            requestBody.Add("numberOfEvents", numberOfEvents);
        }
            
        if (eventKeys != null)
        {
            requestBody.Add("eventKeys", eventKeys);
        }
            
        var restRequest = new RestRequest("/seasons/{key}/actions/create-events", Method.Post)
            .AddUrlSegment("key", key)
            .AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<MultipleEvents>(restRequest)).events.ToArray();
    }
}