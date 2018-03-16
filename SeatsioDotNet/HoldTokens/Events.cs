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
    }
}