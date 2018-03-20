using RestSharp;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Subaccounts
{
    public class Subaccounts
    {
        private readonly RestClient _restClient;

        public Subaccounts(RestClient restClient)
        {
            _restClient = restClient;
        }

        public Subaccount Create(string name)
        {
            var restRequest = new RestRequest("/subaccounts", Method.POST)
                .AddJsonBody(new {name});
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

        public Lister<Subaccount> List()
        {
            return new Lister<Subaccount>(new PageFetcher<Subaccount>(_restClient, "/subaccounts"));
        }
    }
}