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

        public Seasons(RestClient restClient)
        {
            _restClient = restClient;
        }

        public Season Create(string chartKey, string key = null, int? numberOfEvents = null, IEnumerable<string> eventKeys = null, TableBookingConfig tableBookingConfig = null, string socialDistancingRulesetKey = null)
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
            
            if (socialDistancingRulesetKey != null)
            {
                requestBody.Add("socialDistancingRulesetKey", socialDistancingRulesetKey);
            }

            var restRequest = new RestRequest("/seasons", Method.POST).AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Season>(restRequest));
        }

        public Season CreatePartialSeason(string topLevelSeasonKey, string partialSeasonKey = null, IEnumerable<string> eventKeys = null)
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

            var restRequest = new RestRequest("/seasons/{topLevelSeasonKey}/partial-seasons", Method.POST)
                .AddUrlSegment("topLevelSeasonKey", topLevelSeasonKey)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Season>(restRequest));
        }

        public Season Retrieve(string key)
        {
            var restRequest = new RestRequest("/seasons/{key}", Method.GET)
                .AddUrlSegment("key", key);
            return AssertOk(_restClient.Execute<Season>(restRequest));
        }

        public Season RetrievePartialSeason(string topLevelSeasonKey, string partialSeasonKey)
        {
            var restRequest = new RestRequest("/seasons/{topLevelSeasonKey}/partial-seasons/{partialSeasonKey}", Method.GET)
                .AddUrlSegment("topLevelSeasonKey", topLevelSeasonKey)
                .AddUrlSegment("partialSeasonKey", partialSeasonKey);
            return AssertOk(_restClient.Execute<Season>(restRequest));
        }

        public void Delete(string key)
        {
            var restRequest = new RestRequest("/seasons/{key}", Method.DELETE)
                .AddUrlSegment("key", key);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void DeletePartialSeason(string topLevelSeasonKey, string partialSeasonKey)
        {
            var restRequest = new RestRequest("/seasons/{topLevelSeasonKey}/partial-seasons/{partialSeasonKey}", Method.DELETE)
                .AddUrlSegment("topLevelSeasonKey", topLevelSeasonKey)
                .AddUrlSegment("partialSeasonKey", partialSeasonKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public Season AddEventsToPartialSeason(string topLevelSeasonKey, string partialSeasonKey, string[] eventKeys)
        {
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
            requestBody.Add("eventKeys", eventKeys);
            var restRequest = new RestRequest("/seasons/{topLevelSeasonKey}/partial-seasons/{partialSeasonKey}/actions/add-events", Method.POST)
                .AddUrlSegment("topLevelSeasonKey", topLevelSeasonKey)
                .AddUrlSegment("partialSeasonKey", partialSeasonKey)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Season>(restRequest));
        }

        public Season RemoveEventFromPartialSeason(string topLevelSeasonKey, string partialSeasonKey, string eventKey)
        {
            var restRequest = new RestRequest("/seasons/{topLevelSeasonKey}/partial-seasons/{partialSeasonKey}/events/{eventKey}", Method.DELETE)
                    .AddUrlSegment("topLevelSeasonKey", topLevelSeasonKey)
                    .AddUrlSegment("partialSeasonKey", partialSeasonKey)
                    .AddUrlSegment("eventKey", eventKey);
            return AssertOk(_restClient.Execute<Season>(restRequest));
        }

        public Season CreateEvents(string key, string[] eventKeys = null, int? numberOfEvents = null)
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
            
            var restRequest = new RestRequest("/seasons/{key}/actions/create-events", Method.POST)
                .AddUrlSegment("key", key)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Season>(restRequest));
        }
        
        public IEnumerable<Season> ListAll()
        {
            return List().All();
        }

        public Page<Season> ListFirstPage(int? pageSize = null)
        {
            return List().FirstPage(pageSize: pageSize);
        }

        public Page<Season> ListPageAfter(long id, int? pageSize = null)
        {
            return List().PageAfter(id, pageSize: pageSize);
        }

        public Page<Season> ListPageBefore(long id, int? pageSize = null)
        {
            return List().PageBefore(id, pageSize: pageSize);
        }

        private Lister<Season> List()
        {
            return new Lister<Season>(new PageFetcher<Season>(_restClient, "/seasons"));
        }
    }
}