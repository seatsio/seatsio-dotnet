using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Events
{
    public class Events
    {
        private readonly RestClient _restClient;

        public Events(RestClient restClient)
        {
            _restClient = restClient;
        }

        public Event Create(string chartKey)
        {
            return Create(chartKey, null, null, null);
        }

        public Event Create(string chartKey, string eventKey)
        {
            return Create(chartKey, eventKey, null, null);
        }

        public Event Create(string chartKey, string eventKey, bool? bookWholeTables)
        {
            return Create(chartKey, eventKey, bookWholeTables, null);
        }

        public Event Create(string chartKey, string eventKey, Dictionary<string, string> tableBookingModes)
        {
            return Create(chartKey, eventKey, null, tableBookingModes);
        }

        private Event Create(string chartKey, string eventKey, bool? bookWholeTables, Dictionary<string, string> tableBookingModes)
        {
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
            requestBody.Add("chartKey", chartKey);

            if (eventKey != null)
            {
                requestBody.Add("eventKey", eventKey);
            }

            if (bookWholeTables != null)
            {
                requestBody.Add("bookWholeTables", bookWholeTables);
            }

            if (tableBookingModes != null)
            {
                requestBody.Add("tableBookingModes", tableBookingModes);
            }

            var restRequest = new RestRequest("/events", Method.POST).AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public Event[] Create(string chartKey, EventCreationParams[] eventCreationParams)
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
                if (param.BookWholeTables != null)
                {
                    e.Add("bookWholeTables", param.BookWholeTables);
                }
                if (param.TableBookingModes != null)
                {
                    e.Add("tableBookingModes", param.TableBookingModes);
                }
                events.Add(e);
            }
            requestBody.Add("events", events.ToArray());
            var restRequest = new RestRequest("/events/actions/create-multiple", Method.POST).AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<MultipleEvents>(restRequest)).events.ToArray();
        }

        public void Update(string eventKey, string chartKey, string newEventKey)
        {
            Update(eventKey, chartKey, newEventKey, null, null);
        }

        public void Update(string eventKey, string chartKey, string newEventKey, bool? bookWholeTables)
        {
            Update(eventKey, chartKey, newEventKey, bookWholeTables, null);
        }

        public void Update(string eventKey, string chartKey, string newEventKey, Dictionary<string, string> tableBookingModes)
        {
            Update(eventKey, chartKey, newEventKey, null, tableBookingModes);
        }

        private void Update(string eventKey, string chartKey, string newEventKey, bool? bookWholeTables, Dictionary<string, string> tableBookingModes)
        {
            Dictionary<string, object> requestBody = new Dictionary<string, object>();

            if (chartKey != null)
            {
                requestBody.Add("chartKey", chartKey);
            }

            if (newEventKey != null)
            {
                requestBody.Add("eventKey", newEventKey);
            }

            if (bookWholeTables != null)
            {
                requestBody.Add("bookWholeTables", bookWholeTables);
            }

            if (tableBookingModes != null)
            {
                requestBody.Add("tableBookingModes", tableBookingModes);
            }

            var restRequest = new RestRequest("/events/{key}", Method.POST)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void Delete(string eventKey)
        {
            var restRequest = new RestRequest("/events/{key}", Method.DELETE)
                .AddUrlSegment("key", eventKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public Event Retrieve(string eventKey)
        {
            var restRequest = new RestRequest("/events/{key}", Method.GET)
                .AddUrlSegment("key", eventKey);
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public ObjectStatus RetrieveObjectStatus(string eventKey, string objectLabel)
        {
            var restRequest = new RestRequest("/events/{key}/objects/{object}", Method.GET)
                .AddUrlSegment("key", eventKey)
                .AddUrlSegment("object", objectLabel);
            return AssertOk(_restClient.Execute<ObjectStatus>(restRequest));
        }

        public ChangeObjectStatusResult Book(string eventKey, IEnumerable<string> objects, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Booked, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult Book(string[] eventKeys, IEnumerable<string> objects, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Booked, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult Book(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Booked, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult Book(string[] eventKeys, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Booked, holdToken, orderId, keepExtraData);
        }

        public BestAvailableResult Book(string eventKey, BestAvailable bestAvailable, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKey, bestAvailable, ObjectStatus.Booked, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult Release(string eventKey, IEnumerable<string> objects, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Free, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult Release(string[] eventKeys, IEnumerable<string> objects, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Free, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult Release(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Free, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult Release(string[] eventKeys, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Free, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult Hold(string eventKey, IEnumerable<string> objects, string holdToken, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Held, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult Hold(string[] eventKeys, IEnumerable<string> objects, string holdToken, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Held, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult Hold(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Held, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult Hold(string[] eventKeys, IEnumerable<ObjectProperties> objects, string holdToken, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Held, holdToken, orderId, keepExtraData);
        }

        public BestAvailableResult Hold(string eventKey, BestAvailable bestAvailable, string holdToken, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(eventKey, bestAvailable, ObjectStatus.Held, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(string eventKey, IEnumerable<string> objects, string status, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(new[] {eventKey}, objects.Select(o => new ObjectProperties(o)), status, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(string eventKey, IEnumerable<ObjectProperties> objects, string status, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(new[] {eventKey}, objects, status, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(IEnumerable<string> events, IEnumerable<string> objects, string status, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            return ChangeObjectStatus(events, objects.Select(o => new ObjectProperties(o)), status, holdToken, orderId, keepExtraData);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(IEnumerable<string> events, IEnumerable<ObjectProperties> objects, string status, string holdToken = null, string orderId = null, bool? keepExtraData = null)
        {
            var requestBody = ChangeObjectStatusRequest(events, objects, status, holdToken, orderId, keepExtraData);
            var restRequest = new RestRequest("/seasons/actions/change-object-status", Method.POST)
                .AddQueryParameter("expand", "objects")
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<ChangeObjectStatusResult>(restRequest));
        }

        public List<ChangeObjectStatusResult> ChangeObjectStatus(StatusChangeRequest[] requests)
        {
            var serializedRequests = requests.Select(r => ChangeObjectStatusRequest(r.EventKey, r.Objects, r.Status, r.HoldToken, r.OrderId, r.KeepExtraData));
            var restRequest = new RestRequest("/events/actions/change-object-status", Method.POST)
                .AddQueryParameter("expand", "objects")
                .AddJsonBody(new Dictionary<string, object> {{ "statusChanges", serializedRequests}});
            return AssertOk(_restClient.Execute<ChangeObjectStatusInBatchResult>(restRequest)).Results;
        }

        private Dictionary<string, object> ChangeObjectStatusRequest(string evnt, IEnumerable<ObjectProperties> objects, string status, string holdToken, string orderId, bool? keepExtraData)
        {
            var request = ChangeObjectStatusRequest(objects, status, holdToken, orderId, keepExtraData);
            request.Add("event", evnt);
            return request;
        }

        private Dictionary<string, object> ChangeObjectStatusRequest(IEnumerable<string> events, IEnumerable<ObjectProperties> objects, string status, string holdToken, string orderId, bool? keepExtraData)
        {
            var request = ChangeObjectStatusRequest(objects, status, holdToken, orderId, keepExtraData);
            request.Add("events", events);
            return request;
        }

        private Dictionary<string, object> ChangeObjectStatusRequest(IEnumerable<ObjectProperties> objects, string status, string holdToken, string orderId, bool? keepExtraData)
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

            return requestBody;
        }

        public BestAvailableResult ChangeObjectStatus(string eventKey, BestAvailable bestAvailable, string status, string holdToken = null, string orderId = null, bool? keepExtraData = null)
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

            var restRequest = new RestRequest("/events/{key}/actions/change-object-status", Method.POST)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<BestAvailableResult>(restRequest));
        }

        public void UpdateExtraData(string eventKey, string objectLabel, Dictionary<string, object> extraData)
        {
            var restRequest = new RestRequest("/events/{key}/objects/{object}/actions/update-extra-data", Method.POST)
                .AddUrlSegment("key", eventKey)
                .AddUrlSegment("object", objectLabel)
                .AddJsonBody(new {extraData});
            AssertOk(_restClient.Execute<BestAvailableResult>(restRequest));
        }

        public void UpdateExtraDatas(string eventKey, Dictionary<string, Dictionary<string, object>> extraData)
        {
            var restRequest = new RestRequest("/events/{key}/actions/update-extra-data", Method.POST)
                .AddUrlSegment("key", eventKey)
                .AddParameter("application/json", JsonConvert.SerializeObject(new {extraData}), ParameterType.RequestBody); // default serializer doesn't convert extraData to JSON properly
            AssertOk(_restClient.Execute<BestAvailableResult>(restRequest));
        }

        public void MarkAsForSale(string eventKey, IEnumerable<string> objects, IEnumerable<string> categories)
        {
            var requestBody = ForSaleRequest(objects, categories);
            var restRequest = new RestRequest("/events/{key}/actions/mark-as-for-sale", Method.POST)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void MarkAsNotForSale(string eventKey, IEnumerable<string> objects, IEnumerable<string> categories)
        {
            var requestBody = ForSaleRequest(objects, categories);
            var restRequest = new RestRequest("/events/{key}/actions/mark-as-not-for-sale", Method.POST)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void MarkEverythingAsForSale(string eventKey)
        {
            var restRequest = new RestRequest("/events/{key}/actions/mark-everything-as-for-sale", Method.POST)
                .AddUrlSegment("key", eventKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        private Dictionary<string, object> ForSaleRequest(IEnumerable<string> objects, IEnumerable<string> categories)
        {
            var request = new Dictionary<string, object>();

            if (objects != null)
            {
                request.Add("objects", objects);
            }

            if (categories != null)
            {
                request.Add("categories", categories);
            }

            return request;
        }

        public void UpdateChannels(string eventKey, object channels)
        {
            var requestBody = UpdateChannelsRequest(channels);
            var restRequest = new RestRequest("/events/{key}/channels/update", Method.POST)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        private Dictionary<string, object> UpdateChannelsRequest(object channels)
        {
            var request = new Dictionary<string, object>();
            request.Add("channels", channels);
            return request;
        }

        public void AssignObjectsToChannel(string eventKey, object channelsConfig)
        {
            var requestBody = AssignObjectsToChannelsRequest(channelsConfig);
            var restRequest = new RestRequest("/events/{key}/channels/assign-objects", Method.POST)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        private Dictionary<string, object> AssignObjectsToChannelsRequest(object channelsConfig)
        {
            var request = new Dictionary<string, object>();
            request.Add("channelConfig", channelsConfig);
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

        public Lister<StatusChange> StatusChanges(string eventKey, string filter = null, string sortField = null, string sortDirection = null)
        {
            return new Lister<StatusChange>(new PageFetcher<StatusChange>(
                _restClient,
                "/events/{key}/status-changes",
                request => (RestRequest) request.AddUrlSegment("key", eventKey),
                new Dictionary<string, string> {{"filter", filter}, {"sort", ToSort(sortField, sortDirection)}}
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
                request => (RestRequest) request.AddUrlSegment("key", eventKey).AddUrlSegment("objectId", objectLabel)
            ));
        }

        private class ChangeObjectStatusInBatchResult
        {
            public List<ChangeObjectStatusResult> Results { get; set; }
        }
    }
}
