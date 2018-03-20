using System.Collections.Generic;
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

        public Chart Create(string name = null)
        {
            var requestBody = new Dictionary<string, object>();

            if (name != null)
            {
                requestBody.Add("name", name);
            }

            var restRequest = new RestRequest("/charts", Method.POST)
                .AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Chart>(restRequest));
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