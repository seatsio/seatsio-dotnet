using System.Collections.Generic;
using RestSharp;
using RestSharp.Deserializers;
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

        public Chart Create()
        {
            var restRequest = new RestRequest("/charts", Method.POST);
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

        public Lister<Chart> Archive()
        {
            return new Lister<Chart>(new PageFetcher<Chart>(_restClient, "/charts/archive"));
        }
    }
}