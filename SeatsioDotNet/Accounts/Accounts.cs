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
    }
}