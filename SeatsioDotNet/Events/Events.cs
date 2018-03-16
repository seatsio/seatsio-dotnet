using System.Collections.Generic;
using System.Runtime.Serialization;
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

        public void Book(string eventKey, IEnumerable<string> objectLabels, string holdToken = null,
            string orderId = null)
        {
            ChangeObjectStatus(eventKey, objectLabels, ObjectStatus.Booked, holdToken, orderId);
        }  
        
        public void Release(string eventKey, IEnumerable<string> objectLabels, string holdToken = null,
            string orderId = null)
        {
            ChangeObjectStatus(eventKey, objectLabels, ObjectStatus.Free, holdToken, orderId);
        }

        public void Hold(string eventKey, IEnumerable<string> objectLabels, string holdToken, string orderId = null)
        {
            ChangeObjectStatus(eventKey, objectLabels, ObjectStatus.Held, holdToken, orderId);
        }

        private void ChangeObjectStatus(string eventKey, IEnumerable<string> objectLabels, string status,
            string holdToken, string orderId)
        {
            var requestBody = new Dictionary<string, object>()
            {
                {"status", status},
                {"objects", objectLabels},
                {"events", new[] {eventKey}}
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
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }
    }
}