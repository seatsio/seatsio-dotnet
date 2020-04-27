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

        public string RegenerateSecretKey(string key)
        {
            var restRequest = new RestRequest("/workspaces/{key}/actions/regenerate-secret-key", Method.POST)
                .AddUrlSegment("key", key);
            var response = AssertOk(_restClient.Execute<Dictionary<string, string>>(restRequest));
            return response["secretKey"];
        }

        public IEnumerable<Workspace> ListAll()
        {
            return List().All();
        }

        public IEnumerable<Workspace> ListAll(string filter)
        {
            return ParametrizedList().All(WorkspaceListParams(filter));
        }

        public Page<Workspace> ListFirstPage(int? pageSize = null, string filter = null)
        {
            return ParametrizedList().FirstPage(WorkspaceListParams(filter), pageSize);
        }

        public Page<Workspace> ListPageAfter(long id, int? pageSize = null, string filter = null)
        {
            return ParametrizedList().PageAfter(id, WorkspaceListParams(filter), pageSize);
        }

        public Page<Workspace> ListPageBefore(long id, int? pageSize = null, string filter = null)
        {
            return ParametrizedList().PageBefore(id, WorkspaceListParams(filter), pageSize);
        }

        private Lister<Workspace> List()
        {
            return new Lister<Workspace>(new PageFetcher<Workspace>(_restClient, "/workspaces"));
        }

        private ParametrizedLister<Workspace> ParametrizedList()
        {
            return new ParametrizedLister<Workspace>(new PageFetcher<Workspace>(_restClient, "/workspaces"));
        }

        private Dictionary<string, object> WorkspaceListParams(string filter)
        {
            var workspaceListParams = new Dictionary<string, object>();

            if (filter != null)
            {
                workspaceListParams.Add("filter", filter);
            }

            return workspaceListParams;
        }
    }
}