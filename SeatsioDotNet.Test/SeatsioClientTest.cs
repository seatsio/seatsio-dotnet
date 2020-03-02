using System;
using System.IO;
using RestSharp;
using RestSharp.Authenticators;
using SeatsioDotNet.Subaccounts;
using SeatsioDotNet.Util;

namespace SeatsioDotNet.Test
{
    public class SeatsioClientTest
    {
        protected static readonly string BaseUrl = "https://api-staging.seatsio.net";

        protected readonly User User;
        protected readonly Subaccount Subaccount;
        protected readonly SeatsioClient Client;

        protected SeatsioClientTest()
        {
            TestCompany testCompany = CreateTestCompany();
            User = testCompany.admin;
            Subaccount = testCompany.subaccount;
            Client = CreateSeatsioClient(User.SecretKey);
        }

        private TestCompany CreateTestCompany()
        {
            var restClient = new RestClient(BaseUrl);
            var request = new RestRequest("/system/public/users/actions/create-test-company", Method.POST);
            return RestUtil.AssertOk(restClient.Execute<TestCompany>(request));
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
            restClient.Authenticator = new HttpBasicAuthenticator(User.SecretKey, null);
            var chartKey = Guid.NewGuid().ToString();
            var request = new RestRequest("/system/public/charts/{chartKey}", Method.POST)
                .AddUrlSegment("chartKey", chartKey)
                .AddParameter("application/json", json, ParameterType.RequestBody);
            RestUtil.AssertOk(restClient.Execute<object>(request));
            return chartKey;
        }

        protected SeatsioClient CreateSeatsioClient(string secretKey)
        {
            return new SeatsioClient(secretKey, null, BaseUrl);
        }

        protected SeatsioClient CreateSeatsioClient(string secretKey, string workspaceKey)
        {
            return new SeatsioClient(secretKey, workspaceKey, BaseUrl);
        }
    }
}