using System.Collections.Generic;
using RestSharp;
using SeatsioDotNet.Test.Events;
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
            var restRequest = new RestRequest("/events", Method.POST)
                .AddJsonBody(new {chartKey});
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public ObjectStatus RetrieveObjectStatus(string eventKey, string objectLabel)
        {
            var restRequest = new RestRequest("/events/{key}/objects/{object}", Method.GET)
                .AddUrlSegment("key", eventKey)
                .AddUrlSegment("object", objectLabel);
            return AssertOk(_restClient.Execute<ObjectStatus>(restRequest));
        }

        public void Book(string eventKey, IEnumerable<string> objectLabels)
        {
            var restRequest = new RestRequest("/seasons/actions/change-object-status", Method.POST)
                .AddJsonBody(new
                {
                    status = ObjectStatus.Booked,
                    objects = objectLabels,
                    events = new [] {eventKey}
                });
            AssertOk(_restClient.Execute<object>(restRequest));
        }
    }
}