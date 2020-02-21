using System.Collections.Generic;
using RestSharp;
using SeatsioDotNet.Events;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Workspaces
{
    public class Workspaces
    {
        private readonly RestClient _restClient;

        public Workspaces(RestClient restClient)
        {
            _restClient = restClient;
        }

        public Workspace Create(string name)
        {
            return Create(name, null);
        }

        public Workspace Create(string name, bool? isTest)
        {
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
            requestBody.Add("name", name);

            if (isTest != null)
            {
                requestBody.Add("isTest", isTest);
            }

            var restRequest = new RestRequest("/workspaces", Method.POST).AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Workspace>(restRequest));
        }

        public Workspace Retrieve(string key)
        {
            var restRequest = new RestRequest("/workspaces/{key}", Method.GET)
                .AddUrlSegment("key", key);
            return AssertOk(_restClient.Execute<Workspace>(restRequest));
        }

        public void Update(string key, string name)
        {
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
            requestBody.Add("name", name);

            var restRequest = new RestRequest("/workspaces/{key}", Method.POST).AddJsonBody(requestBody)
                .AddUrlSegment("key", key);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public IEnumerable<Workspace> ListAll()
        {
            return List().All();
        }

        public Page<Workspace> ListFirstPage(int? pageSize = null)
        {
            return List().FirstPage(pageSize: pageSize);
        }

        public Page<Workspace> ListPageAfter(long id, int? pageSize = null)
        {
            return List().PageAfter(id, pageSize: pageSize);
        }

        public Page<Workspace> ListPageBefore(long id, int? pageSize = null)
        {
            return List().PageBefore(id, pageSize: pageSize);
        }

        private Lister<Workspace> List()
        {
            return new Lister<Workspace>(new PageFetcher<Workspace>(_restClient, "/workspaces"));
        }
    }
}