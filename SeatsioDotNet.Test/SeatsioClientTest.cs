using System;
using RestSharp;

namespace SeatsioDotNet.Test
{
    public class SeatsioClientTest
    {
        protected static string BASE_URL = "https://api-staging.seats.io";

        protected readonly TestUser user;
        protected readonly SeatsioClient client;

        protected SeatsioClientTest()
        {
            user = createTestUser();
            client = new SeatsioClient(user.SecretKey, BASE_URL);
        }

        private TestUser createTestUser()
        {
            var client = new RestClient(BASE_URL);
            var email = "test" + new Random().Next() + "@seats.io";
            var password = "12345678";
            var request = new RestRequest("/system/public/users", Method.POST)
                .AddJsonBody(new {email, password});
            var response = client.Execute<TestUser>(request);
            if ((int) response.StatusCode < 200 || (int) response.StatusCode >= 300)
            {
                throw new Exception(response.StatusCode + "-" + response.Content);
            }

            return response.Data;
        }
    }
}