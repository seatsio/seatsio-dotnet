using System.Collections.Generic;
using RestSharp;
using SeatsioDotNet.Events;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Workspaces
{
    public class Workspaces
    {
        public WorkspaceLister Active { get; }
        public WorkspaceLister Inactive { get; }

        private readonly RestClient _restClient;

        public Workspaces(RestClient restClient)
        {
            _restClient = restClient;
            Active = new WorkspaceLister(new PageFetcher<Workspace>(_restClient, "/workspaces/active"));
            Inactive = new WorkspaceLister(new PageFetcher<Workspace>(_restClient, "/workspaces/inactive"));
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

            var restRequest = new RestRequest("/workspaces", Method.Post).AddJsonBody(requestBody);
            return AssertOk(_restClient.Execute<Workspace>(restRequest));
        }

        public Workspace Retrieve(string key)
        {
            var restRequest = new RestRequest("/workspaces/{key}", Method.Get)
                .AddUrlSegment("key", key);
            return AssertOk(_restClient.Execute<Workspace>(restRequest));
        }

        public void Update(string key, string name)
        {
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
            requestBody.Add("name", name);

            var restRequest = new RestRequest("/workspaces/{key}", Method.Post).AddJsonBody(requestBody)
                .AddUrlSegment("key", key);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public string RegenerateSecretKey(string key)
        {
            var restRequest = new RestRequest("/workspaces/{key}/actions/regenerate-secret-key", Method.Post)
                .AddUrlSegment("key", key);
            var response = AssertOk(_restClient.Execute<Dictionary<string, string>>(restRequest));
            return response["secretKey"];
        }  
        
        public void Activate(string key)
        {
            var restRequest = new RestRequest("/workspaces/{key}/actions/activate", Method.Post)
                .AddUrlSegment("key", key);
            AssertOk(_restClient.Execute<Dictionary<string, string>>(restRequest));
        }   
        
        public void Deactivate(string key)
        {
            var restRequest = new RestRequest("/workspaces/{key}/actions/deactivate", Method.Post)
                .AddUrlSegment("key", key);
            AssertOk(_restClient.Execute<Dictionary<string, string>>(restRequest));
        }  
        
        public void SetDefault(string key)
        {
            var restRequest = new RestRequest("/workspaces/actions/set-default/{key}", Method.Post)
                .AddUrlSegment("key", key);
            AssertOk(_restClient.Execute<Dictionary<string, string>>(restRequest));
        }

        public IEnumerable<Workspace> ListAll(string filter = null)
        {
            return ParametrizedList().All(filter);
        }

        public Page<Workspace> ListFirstPage(int? pageSize = null, string filter = null)
        {
            return ParametrizedList().FirstPage(filter, pageSize);
        }

        public Page<Workspace> ListPageAfter(long id, int? pageSize = null, string filter = null)
        {
            return ParametrizedList().PageAfter(id, filter, pageSize);
        }

        public Page<Workspace> ListPageBefore(long id, int? pageSize = null, string filter = null)
        {
            return ParametrizedList().PageBefore(id, filter, pageSize);
        }

        private WorkspaceLister ParametrizedList()
        {
            return new WorkspaceLister(new PageFetcher<Workspace>(_restClient, "/workspaces"));
        }
    }
}