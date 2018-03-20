using System.Collections.Generic;
using System.Linq;
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

        public void Book(string eventKey, IEnumerable<string> objects, string holdToken = null, string orderId = null)
        {
            ChangeObjectStatus(eventKey, objects, ObjectStatus.Booked, holdToken, orderId);
        }    
       
        public void Book(string[] eventKeys, IEnumerable<string> objects, string holdToken = null, string orderId = null)
        {
            ChangeObjectStatus(eventKeys, objects, ObjectStatus.Booked, holdToken, orderId);
        }  

        public void Book(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null)
        {
            ChangeObjectStatus(eventKey, objects, ObjectStatus.Booked, holdToken, orderId);
        }  
       
        public void Book(string[] eventKeys, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null)
        {
            ChangeObjectStatus(eventKeys, objects, ObjectStatus.Booked, holdToken, orderId);
        }

        public void Release(string eventKey, IEnumerable<string> objects, string holdToken = null, string orderId = null)
        {
            ChangeObjectStatus(eventKey, objects, ObjectStatus.Free, holdToken, orderId);
        }   
        
        public void Release(string[] eventKeys, IEnumerable<string> objects, string holdToken = null, string orderId = null)
        {
            ChangeObjectStatus(eventKeys, objects, ObjectStatus.Free, holdToken, orderId);
        }   
        
        public void Release(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null)
        {
            ChangeObjectStatus(eventKey, objects, ObjectStatus.Free, holdToken, orderId);
        }   
        
        public void Release(string[] eventKeys, IEnumerable<ObjectProperties> objects, string holdToken = null, string orderId = null)
        {
            ChangeObjectStatus(eventKeys, objects, ObjectStatus.Free, holdToken, orderId);
        }

        public void Hold(string eventKey, IEnumerable<string> objects, string holdToken, string orderId = null)
        {
            ChangeObjectStatus(eventKey, objects, ObjectStatus.Held, holdToken, orderId);
        }    
        
        public void Hold(string[] eventKeys, IEnumerable<string> objects, string holdToken, string orderId = null)
        {
            ChangeObjectStatus(eventKeys, objects, ObjectStatus.Held, holdToken, orderId);
        } 
        
        public void Hold(string eventKey, IEnumerable<ObjectProperties> objects, string holdToken, string orderId = null)
        {
            ChangeObjectStatus(eventKey, objects, ObjectStatus.Held, holdToken, orderId);
        }    
        
        public void Hold(string[] eventKeys, IEnumerable<ObjectProperties> objects, string holdToken, string orderId = null)
        {
            ChangeObjectStatus(eventKeys, objects, ObjectStatus.Held, holdToken, orderId);
        }

        public void ChangeObjectStatus(string eventKey, IEnumerable<string> objects, string status, string holdToken = null, string orderId = null)
        {
            ChangeObjectStatus(new[] {eventKey}, objects.Select(o => new ObjectProperties(o)), status, holdToken, orderId);
        }

        public void ChangeObjectStatus(string eventKey, IEnumerable<ObjectProperties> objects, string status, string holdToken = null, string orderId = null)
        {
            ChangeObjectStatus(new[] {eventKey}, objects, status, holdToken, orderId);
        }

        public void ChangeObjectStatus(IEnumerable<string> events, IEnumerable<string> objects, string status, string holdToken = null, string orderId = null)
        {
            ChangeObjectStatus(events, objects.Select(o => new ObjectProperties(o)), status, holdToken, orderId);
        }

        public void ChangeObjectStatus(IEnumerable<string> events, IEnumerable<ObjectProperties> objects, string status, string holdToken = null, string orderId = null)
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
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }
        
        public BestAvailableResult ChangeObjectStatus(string eventKey, BestAvailable bestAvailable, string status)
        {
            var requestBody = new Dictionary<string, object>()
            {
                {"status", status},
                {"bestAvailable", bestAvailable.AsDictionary()}
            };

            var restRequest = new RestRequest("/events/{key}/actions/change-object-status", Method.POST)
                .AddUrlSegment("key", eventKey)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<BestAvailableResult>(restRequest));
        }
    }
}