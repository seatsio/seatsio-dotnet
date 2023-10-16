using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using RestSharp;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Events
{
    public class Events
    {
        private readonly RestClient _restClient;
        public Channels Channels { get; }

        public Events(RestClient restClient)
        {
            _restClient = restClient;
            Channels = new Channels(restClient);
        }

        public Event Create(string chartKey)
        {
            return Create(chartKey, new CreateEventParams());
        }

        public Event Create(string chartKey, CreateEventParams p)
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
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public Event[] Create(string chartKey, CreateEventParams[] eventCreationParams)
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
            return AssertOk(_restClient.Execute<MultipleEvents>(restRequest)).events.ToArray();
        }

        public void Update(string eventKey, UpdateEventParams p)
        {
            Dictionary<string, object> requestBody = new Dictionary<string, object>();

            if (p.ChartKey != null)
            {
                requestBody.Add("chartKey", p.ChartKey);
            }

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
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void Delete(string eventKey)
        {
            var restRequest = new RestRequest("/events/{key}", Method.Delete)
                .AddUrlSegment("key", eventKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public Event Retrieve(string eventKey)
        {
            var restRequest = new RestRequest("/events/{key}")
                .AddUrlSegment("key", eventKey);
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public EventObjectInfo RetrieveObjectInfo(string eventKey, string objectLabel)
        {
            var result = RetrieveObjectInfos(eventKey, new[] {objectLabel});
            return result[objectLabel];
        }

        public Dictionary<string, EventObjectInfo> RetrieveObjectInfos(string eventKey, string[] objectLabels)
        {
            var restRequest = new RestRequest("/events/{key}/objects")
                .AddUrlSegment("key", eventKey);

            foreach (var objectLabel in objectLabels)
            {
                restRequest.AddQueryParameter("label", objectLabel);
            }

            return AssertOk(_restClient.Execute<Dictionary<string, EventObjectInfo>>(restRequest));
        }

        public ChangeObjectStatusResult Book(string eventKey, IEnumerable<string> objects, string holdToken = null,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Book(string[] eventKeys, IEnumerable<string> objects, string holdToken = null,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Book(string eventKey, IEnumerable<ObjectProperties> objects,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Book(string[] eventKeys, IEnumerable<ObjectProperties> objects,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Booked, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public BestAvailableResult Book(string eventKey, BestAvailable bestAvailable, string holdToken = null,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, bestAvailable, EventObjectInfo.Booked, holdToken, orderId,
                keepExtraData, ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Release(string eventKey, IEnumerable<string> objects, string holdToken = null,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Free, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Release(string[] eventKeys, IEnumerable<string> objects,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Free, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Release(string eventKey, IEnumerable<ObjectProperties> objects,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Free, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Release(string[] eventKeys, IEnumerable<ObjectProperties> objects,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Free, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Hold(string eventKey, IEnumerable<string> objects, string holdToken,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Hold(string[] eventKeys, IEnumerable<string> objects, string holdToken,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Hold(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult Hold(string[] eventKeys, IEnumerable<ObjectProperties> objects,
            string holdToken, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKeys, objects, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public BestAvailableResult Hold(string eventKey, BestAvailable bestAvailable, string holdToken,
            string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(eventKey, bestAvailable, EventObjectInfo.Held, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(string eventKey, IEnumerable<string> objects, string status,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null, string[] allowedPreviousStatuses = null,
            string[] rejectedPreviousStatuses = null)
        {
            return ChangeObjectStatus(new[] {eventKey}, objects.Select(o => new ObjectProperties(o)), status, holdToken,
                orderId, keepExtraData, ignoreChannels, channelKeys, allowedPreviousStatuses,
                rejectedPreviousStatuses);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(string eventKey, IEnumerable<ObjectProperties> objects,
            string status, string holdToken = null, string orderId = null, bool? keepExtraData = null,
            bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(new[] {eventKey}, objects, status, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(IEnumerable<string> events, IEnumerable<string> objects,
            string status, string holdToken = null, string orderId = null, bool? keepExtraData = null,
            bool? ignoreChannels = null, string[] channelKeys = null)
        {
            return ChangeObjectStatus(events, objects.Select(o => new ObjectProperties(o)), status, holdToken, orderId,
                keepExtraData, ignoreChannels, channelKeys);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(IEnumerable<string> events,
            IEnumerable<ObjectProperties> objects, string status, string holdToken = null, string orderId = null,
            bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null, string[] allowedPreviousStatuses = null,
            string[] rejectedPreviousStatuses = null)
        {
            var requestBody = ChangeObjectStatusRequest(events, objects, status, holdToken, orderId, keepExtraData,
                ignoreChannels, channelKeys, allowedPreviousStatuses, rejectedPreviousStatuses);
            var restRequest = new RestRequest("/events/groups/actions/change-object-status", Method.Post)
                .AddQueryParameter("expand", "objects")
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<ChangeObjectStatusResult>(restRequest));
        }

        public List<ChangeObjectStatusResult> ChangeObjectStatus(StatusChangeRequest[] requests)
        {
            var serializedRequests = requests.Select(r => ChangeObjectStatusRequest(r.EventKey, r.Objects, r.Status,
                r.HoldToken, r.OrderId, r.KeepExtraData, r.IgnoreChannels, r.ChannelKeys,
                r.AllowedPreviousStatuses, r.RejectedPreviousStatuses));
            var restRequest = new RestRequest("/events/actions/change-object-status", Method.Post)
                .AddQueryParameter("expand", "objects")
                .AddJsonBody(new Dictionary<string, object> {{"statusChanges", serializedRequests}});
            return AssertOk(_restClient.Execute<ChangeObjectStatusInBatchResult>(restRequest)).Results;
        }

        private Dictionary<string, object> ChangeObjectStatusRequest(string evnt, IEnumerable<ObjectProperties> objects,
            string status, string holdToken, string orderId, bool? keepExtraData, bool? ignoreChannels = null,
            string[] channelKeys = null, string[] allowedPreviousStatuses = null,
            string[] rejectedPreviousStatuses = null)
        {
            var request = ChangeObjectStatusRequest(objects, status, holdToken, orderId, keepExtraData, ignoreChannels,
                channelKeys, allowedPreviousStatuses, rejectedPreviousStatuses);
            request.Add("event", evnt);
            return request;
        }

        private Dictionary<string, object> ChangeObjectStatusRequest(IEnumerable<string> events,
            IEnumerable<ObjectProperties> objects, string status, string holdToken, string orderId, bool? keepExtraData,
            bool? ignoreChannels = null, string[] channelKeys = null,
            string[] allowedPreviousStatuses = null, string[] rejectedPreviousStatuses = null)
        {
            var request = ChangeObjectStatusRequest(objects, status, holdToken, orderId, keepExtraData, ignoreChannels,
                channelKeys, allowedPreviousStatuses, rejectedPreviousStatuses);
            request.Add("events", events);
            return request;
        }

        private Dictionary<string, object> ChangeObjectStatusRequest(IEnumerable<ObjectProperties> objects,
            string status, string holdToken, string orderId, bool? keepExtraData, bool? ignoreChannels = null,
            string[] channelKeys = null, string[] allowedPreviousStatuses = null,
            string[] rejectedPreviousStatuses = null)
        {
            var requestBody = new Dictionary<string, object>()
            {
                {"status", status},
                {"objects", objects.Select(o => o.AsDictionary())},
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

        public BestAvailableResult ChangeObjectStatus(string eventKey, BestAvailable bestAvailable, string status,
            string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null,
            string[] channelKeys = null)
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
            return AssertOk(_restClient.Execute<BestAvailableResult>(restRequest));
        }

        public void UpdateExtraData(string eventKey, string objectLabel, Dictionary<string, object> extraData)
        {
            var restRequest = new RestRequest("/events/{key}/objects/{object}/actions/update-extra-data", Method.Post)
                .AddUrlSegment("key", eventKey)
                .AddUrlSegment("object", objectLabel)
                .AddJsonBody(new {extraData});
            AssertOk(_restClient.Execute<BestAvailableResult>(restRequest));
        }

        public void UpdateExtraDatas(string eventKey, Dictionary<string, Dictionary<string, object>> extraData)
        {
            var restRequest = new RestRequest("/events/{key}/actions/update-extra-data", Method.Post)
                .AddUrlSegment("key", eventKey)
                .AddParameter("application/json", JsonSerializer.Serialize(new {extraData}),
                    ParameterType.RequestBody); // default serializer doesn't convert extraData to JSON properly
            AssertOk(_restClient.Execute<BestAvailableResult>(restRequest));
        }

        public void MarkAsForSale(string eventKey, IEnumerable<string> objects, Dictionary<string, int> areaPlaces,
            IEnumerable<string> categories)
        {
            var requestBody = ForSaleRequest(objects, areaPlaces, categories);
            var restRequest = new RestRequest("/events/{key}/actions/mark-as-for-sale", Method.Post)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void MarkAsNotForSale(string eventKey, IEnumerable<string> objects, Dictionary<string, int> areaPlaces,
            IEnumerable<string> categories)
        {
            var requestBody = ForSaleRequest(objects, areaPlaces, categories);
            var restRequest = new RestRequest("/events/{key}/actions/mark-as-not-for-sale", Method.Post)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void MarkEverythingAsForSale(string eventKey)
        {
            var restRequest = new RestRequest("/events/{key}/actions/mark-everything-as-for-sale", Method.Post)
                .AddUrlSegment("key", eventKey);
            AssertOk(_restClient.Execute<object>(restRequest));
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

        public IEnumerable<Event> ListAll()
        {
            return List().All();
        }

        public Page<Event> ListFirstPage(int? pageSize = null)
        {
            return List().FirstPage(pageSize: pageSize);
        }

        public Page<Event> ListPageAfter(long id, int? pageSize = null)
        {
            return List().PageAfter(id, pageSize: pageSize);
        }

        public Page<Event> ListPageBefore(long id, int? pageSize = null)
        {
            return List().PageBefore(id, pageSize: pageSize);
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
}