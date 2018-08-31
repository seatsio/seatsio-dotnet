using RestSharp;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.HoldTokens
{
    public class HoldTokens
    {
        private readonly RestClient _restClient;

        public HoldTokens(RestClient restClient)
        {
            _restClient = restClient;
        }

        public HoldToken Create()
        {
            var restRequest = new RestRequest("/hold-tokens", Method.POST);
            return AssertOk(_restClient.Execute<HoldToken>(restRequest));
        }

        public HoldToken Create(int expiresInMinutes)
        {
            var restRequest = new RestRequest("/hold-tokens", Method.POST)
                .AddJsonBody(new {expiresInMinutes});
            return AssertOk(_restClient.Execute<HoldToken>(restRequest));
        }

        public HoldToken ExpiresInMinutes(string token, int expiresInMinutes)
        {
            var restRequest = new RestRequest("/hold-tokens/{token}", Method.POST)
                .AddUrlSegment("token", token)
                .AddJsonBody(new {expiresInMinutes});
            return AssertOk(_restClient.Execute<HoldToken>(restRequest));
        }

        public HoldToken Retrieve(string token)
        {
            var restRequest = new RestRequest("/hold-tokens/{token}", Method.GET)
                .AddUrlSegment("token", token);
            return AssertOk(_restClient.Execute<HoldToken>(restRequest));
        }
    }
}