using System;
using RestSharp;

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

        public Subaccount Create()
        {
            var restRequest = new RestRequest("/subaccounts", Method.POST);
            return AssertOk(_restClient.Execute<Subaccount>(restRequest));
        }

        private static T AssertOk<T>(IRestResponse<T> response)
        {
            if ((int) response.StatusCode < 200 || (int) response.StatusCode >= 300)
            {
                throw new Exception(response.StatusCode + "-" + response.Content);
            }

            return response.Data;
        }
    }
}