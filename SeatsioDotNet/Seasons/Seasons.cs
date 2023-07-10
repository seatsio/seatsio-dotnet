using System.Collections.Generic;
using RestSharp;
using SeatsioDotNet.Events;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Seasons
{
    public class Seasons
    {
        private readonly RestClient _restClient;
        private readonly SeatsioClient _seatsioClient;

        public Seasons(RestClient restClient, SeatsioClient seatsioClient)
        {
            _restClient = restClient;
            _seatsioClient = seatsioClient;
        }

        public Event Create(string chartKey, string key = null, int? numberOfEvents = null, IEnumerable<string> eventKeys = null, TableBookingConfig tableBookingConfig = null)
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
            
            var restRequest = new RestRequest("/seasons", Method.Post).AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public Event CreatePartialSeason(string topLevelSeasonKey, string partialSeasonKey = null, IEnumerable<string> eventKeys = null)
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
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public Event Retrieve(string key)
        {
            return _seatsioClient.Events.Retrieve(key);
        }

        public Event AddEventsToPartialSeason(string topLevelSeasonKey, string partialSeasonKey, string[] eventKeys)
        {
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
            requestBody.Add("eventKeys", eventKeys);
            var restRequest = new RestRequest("/seasons/{topLevelSeasonKey}/partial-seasons/{partialSeasonKey}/actions/add-events", Method.Post)
                .AddUrlSegment("topLevelSeasonKey", topLevelSeasonKey)
                .AddUrlSegment("partialSeasonKey", partialSeasonKey)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public Event RemoveEventFromPartialSeason(string topLevelSeasonKey, string partialSeasonKey, string eventKey)
        {
            var restRequest = new RestRequest("/seasons/{topLevelSeasonKey}/partial-seasons/{partialSeasonKey}/events/{eventKey}", Method.Delete)
                    .AddUrlSegment("topLevelSeasonKey", topLevelSeasonKey)
                    .AddUrlSegment("partialSeasonKey", partialSeasonKey)
                    .AddUrlSegment("eventKey", eventKey);
            return AssertOk(_restClient.Execute<Event>(restRequest));
        }

        public Event[] CreateEvents(string key, string[] eventKeys = null, int? numberOfEvents = null)
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
            return AssertOk(_restClient.Execute<MultipleEvents>(restRequest)).events.ToArray();
        }
    }
}