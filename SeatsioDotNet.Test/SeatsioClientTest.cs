using System;
using RestSharp;
using SeatsioDotNet.Util;

namespace SeatsioDotNet.Test
{
    public class SeatsioClientTest
    {
        protected static readonly string BaseUrl = "https://api-staging.seats.io";

        protected readonly TestUser User;
        protected readonly SeatsioClient Client;

        protected SeatsioClientTest()
        {
            User = createTestUser();
            Client = new SeatsioClient(User.SecretKey, BaseUrl);
        }

        private TestUser createTestUser()
        {
            var restClient = new RestClient(BaseUrl);
            var email = "test" + new Random().Next() + "@seats.io";
            var password = "12345678";
            var request = new RestRequest("/system/public/users", Method.POST)
                .AddJsonBody(new {email, password});
            return RestUtil.AssertOk(restClient.Execute<TestUser>(request));
        }
    }
}