using System.Collections.Generic;
using System.Linq;
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
            return Create(chartKey, null, null);
        }

        public Event Create(string chartKey, string eventKey, bool? bookWholeTables)
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

            var restRequest = new RestRequest("/events", Method.POST)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public void Update(string eventKey, string chartKey, string newEventKey, bool? bookWholeTables)
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

        public ChangeObjectStatusResult Book(string eventKey, IEnumerable<string> objects, string holdToken = null, string orderId = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Booked, holdToken, orderId);
        }

        public ChangeObjectStatusResult Book(string[] eventKeys, IEnumerable<string> objects, string holdToken = null, string orderId = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Booked, holdToken, orderId);
        }

        public ChangeObjectStatusResult Book(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Booked, holdToken, orderId);
        }

        public ChangeObjectStatusResult Book(string[] eventKeys, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Booked, holdToken, orderId);
        }

        public ChangeObjectStatusResult Release(string eventKey, IEnumerable<string> objects, string holdToken = null, string orderId = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Free, holdToken, orderId);
        }

        public ChangeObjectStatusResult Release(string[] eventKeys, IEnumerable<string> objects, string holdToken = null, string orderId = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Free, holdToken, orderId);
        }

        public ChangeObjectStatusResult Release(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Free, holdToken, orderId);
        }

        public ChangeObjectStatusResult Release(string[] eventKeys, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Free, holdToken, orderId);
        }

        public ChangeObjectStatusResult Hold(string eventKey, IEnumerable<string> objects, string holdToken, string orderId = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Held, holdToken, orderId);
        }

        public ChangeObjectStatusResult Hold(string[] eventKeys, IEnumerable<string> objects, string holdToken, string orderId = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Held, holdToken, orderId);
        }

        public ChangeObjectStatusResult Hold(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken, string orderId = null)
        {
            return ChangeObjectStatus(eventKey, objects, ObjectStatus.Held, holdToken, orderId);
        }

        public ChangeObjectStatusResult Hold(string[] eventKeys, IEnumerable<ObjectProperties> objects, string holdToken, string orderId = null)
        {
            return ChangeObjectStatus(eventKeys, objects, ObjectStatus.Held, holdToken, orderId);
        }

        public BestAvailableResult Hold(string eventKey, BestAvailable bestAvailable, string holdToken, string orderId = null)
        {
            return ChangeObjectStatus(eventKey, bestAvailable, ObjectStatus.Held, holdToken, orderId);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(string eventKey, IEnumerable<string> objects, string status, string holdToken = null, string orderId = null)
        {
            return ChangeObjectStatus(new[] {eventKey}, objects.Select(o => new ObjectProperties(o)), status, holdToken, orderId);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(string eventKey, IEnumerable<ObjectProperties> objects, string status, string holdToken = null, string orderId = null)
        {
            return ChangeObjectStatus(new[] {eventKey}, objects, status, holdToken, orderId);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(IEnumerable<string> events, IEnumerable<string> objects, string status, string holdToken = null, string orderId = null)
        {
            return ChangeObjectStatus(events, objects.Select(o => new ObjectProperties(o)), status, holdToken, orderId);
        }

        public ChangeObjectStatusResult ChangeObjectStatus(IEnumerable<string> events, IEnumerable<ObjectProperties> objects, string status, string holdToken = null, string orderId = null)
        {
            var requestBody = new Dictionary<string, object>()
            {
                {"status", status},
                {"objects", objects.Select(o => o.AsDictionary())},
                {"events", events}
            };
            if (holdToken != null)
            {
                requestBody.Add("holdToken", holdToken);
            }

            if (orderId != null)
            {
                requestBody.Add("orderId", orderId);
            }

            var restRequest = new RestRequest("/seasons/actions/change-object-status", Method.POST)
                .AddQueryParameter("expand", "labels")
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<ChangeObjectStatusResult>(restRequest));
        }

        public BestAvailableResult ChangeObjectStatus(string eventKey, BestAvailable bestAvailable, string status, string holdToken = null, string orderId = null)
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

        public Lister<StatusChange> StatusChanges(string eventKey)
        {
            return new Lister<StatusChange>(new PageFetcher<StatusChange>(
                _restClient,
                "/events/{key}/status-changes",
                request => (RestRequest) request.AddUrlSegment("key", eventKey)
            ));
        }

        public Lister<StatusChange> StatusChanges(string eventKey, string objectLabel)
        {
            return new Lister<StatusChange>(new PageFetcher<StatusChange>(
                _restClient,
                "/events/{key}/objects/{objectId}/status-changes",
                request => (RestRequest) request.AddUrlSegment("key", eventKey).AddUrlSegment("objectId", objectLabel)
            ));
        }
    }
}