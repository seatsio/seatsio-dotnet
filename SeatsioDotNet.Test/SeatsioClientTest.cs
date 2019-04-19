using System;
using System.IO;
using RestSharp;
using SeatsioDotNet.Util;

namespace SeatsioDotNet.Test
{
    public class SeatsioClientTest
    {
        private static readonly string BaseUrl = "https://api-staging.seatsio.net";

        protected readonly TestUser User;
        protected readonly SeatsioClient Client;

        protected SeatsioClientTest()
        {
            User = CreateTestUser();
            Client = CreateSeatsioClient(User.SecretKey);
        }

        private TestUser CreateTestUser()
        {
            var restClient = new RestClient(BaseUrl);
            var request = new RestRequest("/system/public/users/actions/create-test-user", Method.POST);
            return RestUtil.AssertOk(restClient.Execute<TestUser>(request));
        }

        protected static string RandomEmail()
        {
            return Guid.NewGuid() + "@mailinator.com";
        }

        protected string CreateTestChart()
        {
            return CreateTestChartFromJson(File.ReadAllText("./resources/sampleChart.json"));
        }

        protected string CreateTestChartWithSections()
        {
            return CreateTestChartFromJson(File.ReadAllText("./resources/sampleChartWithSections.json"));
        }

        protected string CreateTestChartWithTables()
        {
            return CreateTestChartFromJson(File.ReadAllText("./resources/sampleChartWithTables.json"));
        }

        protected string CreateTestChartWithErrors()
        {
            return CreateTestChartFromJson(File.ReadAllText("./resources/sampleChartWithErrors.json"));
        }

        protected string CreateTestChartFromJson(String json)
        {
            var restClient = new RestClient(BaseUrl);
            var chartKey = Guid.NewGuid().ToString();
            var request = new RestRequest("/system/public/{designerKey}/charts/{chartKey}", Method.POST)
                .AddUrlSegment("designerKey", User.DesignerKey)
                .AddUrlSegment("chartKey", chartKey)
                .AddParameter("application/json", json, ParameterType.RequestBody);
            RestUtil.AssertOk(restClient.Execute<object>(request));
            return chartKey;
        }

        protected SeatsioClient CreateSeatsioClient(string secretKey)
        {
            return new SeatsioClient(secretKey, BaseUrl);
        }
    }
}