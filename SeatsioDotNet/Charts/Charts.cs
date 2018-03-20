using System.Collections.Generic;
using System.Linq;
using RestSharp;
using RestSharp.Deserializers;
using SeatsioDotNet.Test.Charts;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Charts
{
    public class Charts
    {
        private readonly RestClient _restClient;

        public Charts(RestClient restClient)
        {
            _restClient = restClient;
        }

        public Chart Create(string name = null, string venueType = null, IEnumerable<Category> categories = null)
        {
            var requestBody = new Dictionary<string, object>();

            if (name != null)
            {
                requestBody.Add("name", name);
            }

            if (venueType != null)
            {
                requestBody.Add("venueType", venueType);
            }

            if (categories != null)
            {
                requestBody.Add("categories", categories.Select(c => c.AsDictionary()));
            }

            var restRequest = new RestRequest("/charts", Method.POST)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public void Update(string chartKey, string name = null, IEnumerable<Category> categories = null)
        {
            var requestBody = new Dictionary<string, object>();

            if (name != null)
            {
                requestBody.Add("name", name);
            }

            if (categories != null)
            {
                requestBody.Add("categories", categories.Select(c => c.AsDictionary()));
            }

            var restRequest = new RestRequest("/charts/{key}", Method.POST)
                .AddUrlSegment("key", chartKey)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void AddTag(string chartKey, string tag)
        {
            var restRequest = new RestRequest("/charts/{key}/tags/{tag}", Method.POST)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("tag", tag);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void RemoveTag(string chartKey, string tag)
        {
            var restRequest = new RestRequest("/charts/{key}/tags/{tag}", Method.DELETE)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("tag", tag);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public Chart Retrieve(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}", Method.GET)
                .AddUrlSegment("key", chartKey);
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public Chart RetrieveWithEvents(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}", Method.GET)
                .AddUrlSegment("key", chartKey)
                .AddQueryParameter("expand", "events");
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public IEnumerable<string> ListAllTags()
        {
            var restRequest = new RestRequest("/charts/tags", Method.GET);
            return AssertOk(_restClient.Execute<Tags>(restRequest)).List;
        }

        private class Tags
        {
            [DeserializeAs(Name = "tags")]
            public IEnumerable<string> List { get; set; }
        }

        public void MoveToArchive(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/actions/move-to-archive", Method.POST)
                .AddUrlSegment("key", chartKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void MoveOutOfArchive(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/actions/move-out-of-archive", Method.POST)
                .AddUrlSegment("key", chartKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public dynamic RetrievePublishedVersion(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/published", Method.GET)
                .AddUrlSegment("key", chartKey);
            return AssertOk(_restClient.ExecuteDynamic(restRequest));
        }
        
        public void PublishDraftVersion(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/draft/actions/publish", Method.POST)
                .AddUrlSegment("key", chartKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }   
        
        public void DiscardDraftVersion(string chartKey)
        {
            var restRequest = new RestRequest("/charts/{key}/version/draft/actions/discard", Method.POST)
                .AddUrlSegment("key", chartKey);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public ChartLister List()
        {
            return new ChartLister(_restClient);
        }

        public Lister<Chart, ListParams> Archive()
        {
            return new Lister<Chart, ListParams>(new PageFetcher<Chart>(_restClient, "/charts/archive"));
        }

    }
}