using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Events;

public class Events
{
    private readonly RestClient _restClient;
    public Channels Channels { get; }

    public Events(RestClient restClient)
    {
        _restClient = restClient;
        Channels = new Channels(restClient);
    }

    public async Task<Event> CreateAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        return await CreateAsync(chartKey, new CreateEventParams(), cancellationToken);
    }

    public async Task<Event> CreateAsync(string chartKey, CreateEventParams p, CancellationToken cancellationToken = default)
    {
        var requestBody = new Dictionary<string, object>();
        requestBody.Add("chartKey", chartKey);

        if (p.Key != null)
        {
            requestBody.Add("eventKey", p.Key);
        }

        if (p.Name != null)
        {
            requestBody.Add("name", p.Name);
        }

        if (p.Date != null)
        {
            requestBody.Add("date", p.Date);
        }

        if (p.TableBookingConfig != null)
        {
            requestBody.Add("tableBookingConfig", p.TableBookingConfig.AsJsonObject());
        }

        if (p.ObjectCategories != null)
        {
            requestBody.Add("objectCategories", p.ObjectCategories);
        }

        if (p.Categories != null)
        {
            requestBody.Add("categories", p.Categories);
        }

        if (p.Channels != null)
        {
            requestBody.Add("channels", p.Channels);
        }

        if (p.ForSaleConfig != null)
        {
            requestBody.Add("forSaleConfig", p.ForSaleConfig.AsJsonObject());
        }

        var restRequest = new RestRequest("/events", Method.Post).AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<Event>(restRequest, cancellationToken));
    }

    public async Task<Event[]> CreateAsync(string chartKey, CreateEventParams[] eventCreationParams, CancellationToken cancellationToken = default)
    {
        Dictionary<string, object> requestBody = new Dictionary<string, object>();
        requestBody.Add("chartKey", chartKey);
        var events = new List<Dictionary<string, object>>();
        foreach (var param in eventCreationParams)
        {
            var e = new Dictionary<string, object>();
            if (param.Key != null)
            {
                e.Add("eventKey", param.Key);
            }

            if (param.Name != null)
            {
                e.Add("name", param.Name);
            }

            if (param.Date != null)
            {
                e.Add("date", param.Date);
            }

            if (param.TableBookingConfig != null)
            {
                e.Add("tableBookingConfig", param.TableBookingConfig.AsJsonObject());
            }

            if (param.ObjectCategories != null)
            {
                e.Add("objectCategories", param.ObjectCategories);
            }

            if (param.Categories != null)
            {
                e.Add("categories", param.Categories);
            }

            if (param.Channels != null)
            {
                e.Add("channels", param.Channels);
            }

            if (param.ForSaleConfig != null)
            {
                e.Add("forSaleConfig", param.ForSaleConfig.AsJsonObject());
            }

            events.Add(e);
        }

        requestBody.Add("events", events.ToArray());
        var restRequest = new RestRequest("/events/actions/create-multiple", Method.Post).AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<MultipleEvents>(restRequest, cancellationToken)).events.ToArray();
    }

    public async Task UpdateAsync(string eventKey, UpdateEventParams p, CancellationToken cancellationToken = default)
    {
        Dictionary<string, object> requestBody = new Dictionary<string, object>();

        if (p.Key != null)
        {
            requestBody.Add("eventKey", p.Key);
        }

        if (p.Name != null)
        {
            requestBody.Add("name", p.Name);
        }

        if (p.Date != null)
        {
            requestBody.Add("date", p.Date);
        }

        if (p.TableBookingConfig != null)
        {
            requestBody.Add("tableBookingConfig", p.TableBookingConfig.AsJsonObject());
        }

        if (p.ObjectCategories != null)
        {
            requestBody.Add("objectCategories", p.ObjectCategories);
        }

        if (p.Categories != null)
        {
            requestBody.Add("categories", p.Categories);
        }

        if (p.IsInThePast != null)
        {
            requestBody.Add("isInThePast", p.IsInThePast);
        }

        var restRequest = new RestRequest("/events/{key}", Method.Post)
            .AddUrlSegment("key", eventKey)
            .AddJsonBody(requestBody);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task DeleteAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/events/{key}", Method.Delete)
            .AddUrlSegment("key", eventKey);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task<Event> RetrieveAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/events/{key}")
            .AddUrlSegment("key", eventKey);
        return AssertOk(await _restClient.ExecuteAsync<Event>(restRequest, cancellationToken));
    }

    public async Task<EventObjectInfo> RetrieveObjectInfoAsync(string eventKey, string objectLabel, CancellationToken cancellationToken = default)
    {
        var result = await RetrieveObjectInfosAsync(eventKey, new[] {objectLabel}, cancellationToken);
        return result[objectLabel];
    }

    public async Task<Dictionary<string, EventObjectInfo>> RetrieveObjectInfosAsync(string eventKey, string[] objectLabels, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/events/{key}/objects")
            .AddUrlSegment("key", eventKey);

        foreach (var objectLabel in objectLabels)
        {
            restRequest.AddQueryParameter("label", objectLabel);
        }

        return AssertOk(await _restClient.ExecuteAsync<Dictionary<string, EventObjectInfo>>(restRequest, cancellationToken));
    }

    public async Task<ChangeObjectStatusResult> BookAsync(string eventKey, IEnumerable<string> objects, string holdToken = null,
        string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKey, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken: cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> BookAsync(string[] eventKeys, IEnumerable<string> objects, string holdToken = null,
        string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKeys, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> BookAsync(string eventKey, IEnumerable<ObjectProperties> objects,
        string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
        string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKey, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> BookAsync(string[] eventKeys, IEnumerable<ObjectProperties> objects,
        string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
        string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKeys, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken: cancellationToken);
    }

    public async Task<BestAvailableResult> BookAsync(string eventKey, BestAvailable bestAvailable, string holdToken = null,
        string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKey, bestAvailable, EventObjectInfo.Booked, holdToken, orderId,
            keepExtraData, ignoreChannels, channelKeys, cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> PutUpForResaleAsync(string eventKey, IEnumerable<string> objects)
    {
        return await ChangeObjectStatusAsync(eventKey, objects, EventObjectInfo.Resale);
    }
    
    public async Task<ChangeObjectStatusResult> PutUpForResaleAsync(string[] eventKeys, IEnumerable<string> objects)
    {
        return await ChangeObjectStatusAsync(eventKeys, objects, EventObjectInfo.Resale);
    }

    public async Task<ChangeObjectStatusResult> ReleaseAsync(string eventKey, IEnumerable<string> objects, string holdToken = null,
        string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        var requestBody = ChangeObjectStatusRequest(StatusChangeRequest.RELEASE, new[] {eventKey}, objects.Select(o => new ObjectProperties(o)), null, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys);
        return await DoChangeObjectStatusAsync(requestBody, cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> ReleaseAsync(string[] eventKeys, IEnumerable<string> objects,
        string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
        string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKeys, objects, EventObjectInfo.Free, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> ReleaseAsync(string eventKey, IEnumerable<ObjectProperties> objects,
        string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
        string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKey, objects, EventObjectInfo.Free, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> ReleaseAsync(string[] eventKeys, IEnumerable<ObjectProperties> objects,
        string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
        string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKeys, objects, EventObjectInfo.Free, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken: cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> HoldAsync(string eventKey, IEnumerable<string> objects, string holdToken,
        string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKey, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken: cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> HoldAsync(string[] eventKeys, IEnumerable<string> objects, string holdToken,
        string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKeys, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> HoldAsync(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken,
        string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKey, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> HoldAsync(string[] eventKeys, IEnumerable<ObjectProperties> objects,
        string holdToken, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
        string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKeys, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken: cancellationToken);
    }

    public async Task<BestAvailableResult> HoldAsync(string eventKey, BestAvailable bestAvailable, string holdToken,
        string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(eventKey, bestAvailable, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> ChangeObjectStatusAsync(string eventKey, IEnumerable<string> objects, string status,
        string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
        string[] channelKeys = null, string[] allowedPreviousStatuses = null,
        string[] rejectedPreviousStatuses = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(new[] {eventKey}, objects.Select(o => new ObjectProperties(o)), status, holdToken,
            orderId, keepExtraData, ignoreChannels, channelKeys, allowedPreviousStatuses,
            rejectedPreviousStatuses, cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> ChangeObjectStatusAsync(string eventKey, IEnumerable<ObjectProperties> objects,
        string status, string holdToken = null, string orderId = null, bool? keepExtraData = null,
        bool? ignoreChannels = null, string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(new[] {eventKey}, objects, status, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, cancellationToken: cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> ChangeObjectStatusAsync(IEnumerable<string> events, IEnumerable<string> objects,
        string status, string holdToken = null, string orderId = null, bool? keepExtraData = null,
        bool? ignoreChannels = null, string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        return await ChangeObjectStatusAsync(events, objects.Select(o => new ObjectProperties(o)), status, holdToken, orderId,
            keepExtraData, ignoreChannels, channelKeys, cancellationToken: cancellationToken);
    }

    public async Task<ChangeObjectStatusResult> ChangeObjectStatusAsync(IEnumerable<string> events,
        IEnumerable<ObjectProperties> objects, string status, string holdToken = null, string orderId = null,
        bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null,
        string[] allowedPreviousStatuses = null,
        string[] rejectedPreviousStatuses = null, CancellationToken cancellationToken = default)
    {
        var requestBody = ChangeObjectStatusRequest(StatusChangeRequest.CHANGE_STATUS_TO, events, objects, status, holdToken, orderId, keepExtraData,
            ignoreChannels, channelKeys, allowedPreviousStatuses, rejectedPreviousStatuses);
        return await DoChangeObjectStatusAsync(requestBody, cancellationToken);
    }

    private async Task<ChangeObjectStatusResult> DoChangeObjectStatusAsync(Dictionary<string, object> requestBody, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("/events/groups/actions/change-object-status", Method.Post)
            .AddQueryParameter("expand", "objects")
            .AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<ChangeObjectStatusResult>(restRequest, cancellationToken));
    }

    public async Task<List<ChangeObjectStatusResult>> ChangeObjectStatusAsync(StatusChangeRequest[] requests, CancellationToken cancellationToken = default)
    {
        var serializedRequests = requests.Select(r => ChangeObjectStatusRequest(r.Type, r.EventKey, r.Objects, r.Status,
            r.HoldToken, r.OrderId, r.KeepExtraData, r.IgnoreChannels, r.ChannelKeys,
            r.AllowedPreviousStatuses, r.RejectedPreviousStatuses));
        var restRequest = new RestRequest("/events/actions/change-object-status", Method.Post)
            .AddQueryParameter("expand", "objects")
            .AddJsonBody(new Dictionary<string, object> {{"statusChanges", serializedRequests}});
        return AssertOk(await _restClient.ExecuteAsync<ChangeObjectStatusInBatchResult>(restRequest, cancellationToken)).Results;
    }

    private Dictionary<string, object> ChangeObjectStatusRequest(string type, string evnt, IEnumerable<ObjectProperties> objects,
        string status, string holdToken, string orderId, bool? keepExtraData, bool? ignoreChannels = null,
        string[] channelKeys = null, string[] allowedPreviousStatuses = null, string[] rejectedPreviousStatuses = null)
    {
        var request = ChangeObjectStatusRequest(type, objects, status, holdToken, orderId, keepExtraData, ignoreChannels,
            channelKeys, allowedPreviousStatuses, rejectedPreviousStatuses);
        request.Add("event", evnt);
        return request;
    }
    
    private Dictionary<string, object> ChangeObjectStatusRequest(string type, IEnumerable<string> events,
        IEnumerable<ObjectProperties> objects, string status, string holdToken, string orderId, bool? keepExtraData,
        bool? ignoreChannels = null, string[] channelKeys = null,
        string[] allowedPreviousStatuses = null, string[] rejectedPreviousStatuses = null)
    {
        var request = ChangeObjectStatusRequest(type, objects, status, holdToken, orderId, keepExtraData, ignoreChannels,
            channelKeys, allowedPreviousStatuses, rejectedPreviousStatuses);
        request.Add("events", events);
        return request;
    }

    private Dictionary<string, object> ChangeObjectStatusRequest(string type, IEnumerable<ObjectProperties> objects,
        string status, string holdToken, string orderId, bool? keepExtraData, bool? ignoreChannels = null,
        string[] channelKeys = null, string[] allowedPreviousStatuses = null,
        string[] rejectedPreviousStatuses = null)
    {
        var requestBody = new Dictionary<string, object>()
        {
            {"type", type},
            {"objects", objects.Select(o => o.AsDictionary())},
        };

        if (type != StatusChangeRequest.RELEASE)
        {
            requestBody.Add("status", status);
        }

        if (holdToken != null)
        {
            requestBody.Add("holdToken", holdToken);
        }

        if (orderId != null)
        {
            requestBody.Add("orderId", orderId);
        }

        if (keepExtraData != null)
        {
            requestBody.Add("keepExtraData", keepExtraData);
        }

        if (ignoreChannels != null)
        {
            requestBody.Add("ignoreChannels", ignoreChannels);
        }

        if (channelKeys != null)
        {
            requestBody.Add("channelKeys", channelKeys);
        }

        if (allowedPreviousStatuses != null)
        {
            requestBody.Add("allowedPreviousStatuses", allowedPreviousStatuses);
        }

        if (rejectedPreviousStatuses != null)
        {
            requestBody.Add("rejectedPreviousStatuses", rejectedPreviousStatuses);
        }

        return requestBody;
    }
    
    public async Task<BestAvailableResult> ChangeObjectStatusAsync(string eventKey, BestAvailable bestAvailable, string status,
        string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
        string[] channelKeys = null, CancellationToken cancellationToken = default)
    {
        var requestBody = new Dictionary<string, object>()
        {
            {"status", status},
            {"bestAvailable", bestAvailable.AsDictionary()}
        };

        if (holdToken != null)
        {
            requestBody.Add("holdToken", holdToken);
        }

        if (orderId != null)
        {
            requestBody.Add("orderId", orderId);
        }

        if (keepExtraData != null)
        {
            requestBody.Add("keepExtraData", keepExtraData);
        }

        if (ignoreChannels != null)
        {
            requestBody.Add("ignoreChannels", ignoreChannels);
        }

        if (channelKeys != null)
        {
            requestBody.Add("channelKeys", channelKeys);
        }

        var restRequest = new RestRequest("/events/{key}/actions/change-object-status", Method.Post)
            .AddUrlSegment("key", eventKey)
            .AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<BestAvailableResult>(restRequest, cancellationToken));
    }

    public async Task OverrideSeasonObjectStatusAsync(string eventKey, string[] objects, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/events/{key}/actions/override-season-status", Method.Post)
            .AddUrlSegment("key", eventKey)
            .AddJsonBody(new {objects});
        AssertOk(await _restClient.ExecuteAsync<BestAvailableResult>(restRequest, cancellationToken));
    }

    public async Task UseSeasonObjectStatusAsync(string eventKey, string[] objects, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/events/{key}/actions/use-season-status", Method.Post)
            .AddUrlSegment("key", eventKey)
            .AddJsonBody(new {objects});
        AssertOk(await _restClient.ExecuteAsync<BestAvailableResult>(restRequest, cancellationToken));
    }

    public async Task UpdateExtraDataAsync(string eventKey, string objectLabel, Dictionary<string, object> extraData, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/events/{key}/objects/{object}/actions/update-extra-data", Method.Post)
            .AddUrlSegment("key", eventKey)
            .AddUrlSegment("object", objectLabel)
            .AddJsonBody(new {extraData});
        AssertOk(await _restClient.ExecuteAsync<BestAvailableResult>(restRequest, cancellationToken));
    }

    public async Task UpdateExtraDatasAsync(string eventKey, Dictionary<string, Dictionary<string, object>> extraData, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/events/{key}/actions/update-extra-data", Method.Post)
            .AddUrlSegment("key", eventKey)
            .AddParameter("application/json", JsonSerializer.Serialize(new {extraData}),
                ParameterType.RequestBody); // default serializer doesn't convert extraData to JSON properly
        AssertOk(await _restClient.ExecuteAsync<BestAvailableResult>(restRequest, cancellationToken));
    }

    public async Task MarkAsForSaleAsync(string eventKey, IEnumerable<string> objects, Dictionary<string, int> areaPlaces,
        IEnumerable<string> categories, CancellationToken cancellationToken = default)
    {
        var requestBody = ForSaleRequest(objects, areaPlaces, categories);
        var restRequest = new RestRequest("/events/{key}/actions/mark-as-for-sale", Method.Post)
            .AddUrlSegment("key", eventKey)
            .AddJsonBody(requestBody);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task MarkAsNotForSaleAsync(string eventKey, IEnumerable<string> objects, Dictionary<string, int> areaPlaces,
        IEnumerable<string> categories, CancellationToken cancellationToken = default)
    {
        var requestBody = ForSaleRequest(objects, areaPlaces, categories);
        var restRequest = new RestRequest("/events/{key}/actions/mark-as-not-for-sale", Method.Post)
            .AddUrlSegment("key", eventKey)
            .AddJsonBody(requestBody);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task MarkEverythingAsForSaleAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/events/{key}/actions/mark-everything-as-for-sale", Method.Post)
            .AddUrlSegment("key", eventKey);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    private Dictionary<string, object> ForSaleRequest(IEnumerable<string> objects,
        Dictionary<string, int> areaPlaces, IEnumerable<string> categories)
    {
        var request = new Dictionary<string, object>();

        if (objects != null)
        {
            request.Add("objects", objects);
        }

        if (areaPlaces != null)
        {
            request.Add("areaPlaces", areaPlaces);
        }

        if (categories != null)
        {
            request.Add("categories", categories);
        }

        return request;
    }

    public IAsyncEnumerable<Event> ListAllAsync()
    {
        return List().AllAsync();
    }

    public async Task<Page<Event>> ListFirstPageAsync(int? pageSize = null)
    {
        return await List().FirstPageAsync(pageSize: pageSize);
    }

    public async Task<Page<Event>> ListPageAfterAsync(long id, int? pageSize = null)
    {
        return await List().PageAfterAsync(id, pageSize: pageSize);
    }

    public async Task<Page<Event>> ListPageBeforeAsync(long id, int? pageSize = null)
    {
        return await List().PageBeforeAsync(id, pageSize: pageSize);
    }

    private Lister<Event> List()
    {
        return new Lister<Event>(new PageFetcher<Event>(_restClient, "/events"));
    }

    public Lister<StatusChange> StatusChanges(string eventKey, string filter = null, string sortField = null,
        string sortDirection = null)
    {
        return new Lister<StatusChange>(new PageFetcher<StatusChange>(
            _restClient,
            "/events/{key}/status-changes",
            request => request.AddUrlSegment("key", eventKey),
            new() {{"filter", filter}, {"sort", ToSort(sortField, sortDirection)}}
        ));
    }

    private static string ToSort(string sortField, string sortDirection)
    {
        if (sortField == null)
        {
            return null;
        }

        if (sortDirection == null)
        {
            return sortField;
        }

        return sortField + ":" + sortDirection;
    }

    public Lister<StatusChange> StatusChangesForObject(string eventKey, string objectLabel)
    {
        return new Lister<StatusChange>(new PageFetcher<StatusChange>(
            _restClient,
            "/events/{key}/objects/{objectId}/status-changes",
            request => request.AddUrlSegment("key", eventKey).AddUrlSegment("objectId", objectLabel)
        ));
    }

    private class ChangeObjectStatusInBatchResult
    {
        public List<ChangeObjectStatusResult> Results { get; set; }
    }
}