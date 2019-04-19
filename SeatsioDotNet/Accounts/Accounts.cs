using System.Collections.Generic;
using RestSharp;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Accounts
{
    public class Accounts
    {
        private readonly RestClient _restClient;

        public Accounts(RestClient restClient)
        {
            _restClient = restClient;
        }

        public Account RetrieveMyAccount()
        {
            var restRequest = new RestRequest("/accounts/me", Method.GET);
            return AssertOk(_restClient.Execute<Account>(restRequest));
        }

        public void UpdateSetting(string key, string value)
        {
            var requestBody = new Dictionary<string, object> {{"key", key}, {"value", value}};
            var req = new RestRequest("/accounts/me/settings", Method.POST)
                .AddJsonBody(requestBody);
            AssertOk(_restClient.Execute<object>(req));
        }
    }
}