using System.Collections.Generic;
using RestSharp;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Subaccounts
{
    public class Subaccounts
    {
        public Lister<Subaccount> Active { get; }
        public Lister<Subaccount> Inactive { get; }

        private readonly RestClient _restClient;

        public Subaccounts(RestClient restClient)
        {
            _restClient = restClient;
            Active = new Lister<Subaccount>(new PageFetcher<Subaccount>(_restClient, "/subaccounts/active"));
            Inactive = new Lister<Subaccount>(new PageFetcher<Subaccount>(_restClient, "/subaccounts/inactive"));
        }

        public Subaccount Create(string name)
        {
            var restRequest = new RestRequest("/subaccounts", Method.POST)
                .AddJsonBody(new {name});
            return AssertOk(_restClient.Execute<Subaccount>(restRequest));
        } 
        
        public Subaccount CreateWithEmail(string email)
        {
            var restRequest = new RestRequest("/subaccounts", Method.POST)
                .AddJsonBody(new {email});
            return AssertOk(_restClient.Execute<Subaccount>(restRequest));
        }

        public Subaccount Retrieve(long id)
        {
            var restRequest = new RestRequest("/subaccounts/{id}", Method.GET)
                .AddUrlSegment("id", id);
            return AssertOk(_restClient.Execute<Subaccount>(restRequest));
        }

        public void Update(long id, string name)
        {
            var restRequest = new RestRequest("/subaccounts/{id}", Method.POST)
                .AddUrlSegment("id", id)
                .AddJsonBody(new {name});
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public Subaccount Create()
        {
            var restRequest = new RestRequest("/subaccounts", Method.POST);
            return AssertOk(_restClient.Execute<Subaccount>(restRequest));
        }

        public void Activate(long id)
        {
            var restRequest = new RestRequest("/subaccounts/{id}/actions/activate", Method.POST)
                .AddUrlSegment("id", id);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void Deactivate(long id)
        {
            var restRequest = new RestRequest("/subaccounts/{id}/actions/deactivate", Method.POST)
                .AddUrlSegment("id", id);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void RegenerateSecretKey(long id)
        {
            var restRequest = new RestRequest("/subaccounts/{id}/secret-key/actions/regenerate", Method.POST)
                .AddUrlSegment("id", id);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public void RegenerateDesignerKey(long id)
        {
            var restRequest = new RestRequest("/subaccounts/{id}/designer-key/actions/regenerate", Method.POST)
                .AddUrlSegment("id", id);
            AssertOk(_restClient.Execute<object>(restRequest));
        }

        public Chart CopyChartToParent(long id, string chartKey)
        {
            var restRequest = new RestRequest("/subaccounts/{id}/charts/{chartKey}/actions/copy-to/parent", Method.POST)
                .AddUrlSegment("id", id)
                .AddUrlSegment("chartKey", chartKey);
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public Chart CopyChartToSubaccount(long fromId, long toId, string chartKey)
        {
            var restRequest = new RestRequest("/subaccounts/{fromId}/charts/{chartKey}/actions/copy-to/{toId}", Method.POST)
                .AddUrlSegment("fromId", fromId)
                .AddUrlSegment("chartKey", chartKey)
                .AddUrlSegment("toId", toId.ToString());
            return AssertOk(_restClient.Execute<Chart>(restRequest));
        }

        public IEnumerable<Subaccount> ListAll()
        {
            return List().All();
        }

        public Page<Subaccount> ListFirstPage(int? pageSize = null)
        {
            return List().FirstPage(pageSize: pageSize);
        }

        public Page<Subaccount> ListPageAfter(long id, int? pageSize = null)
        {
            return List().PageAfter(id, pageSize: pageSize);
        }

        public Page<Subaccount> ListPageBefore(long id, int? pageSize = null)
        {
            return List().PageBefore(id, pageSize: pageSize);
        }

        private Lister<Subaccount> List()
        {
            return new Lister<Subaccount>(new PageFetcher<Subaccount>(_restClient, "/subaccounts"));
        }
    }
}