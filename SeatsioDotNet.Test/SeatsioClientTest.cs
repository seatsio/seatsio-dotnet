using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using RestSharp;
using RestSharp.Authenticators;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Events;
using SeatsioDotNet.Util;

namespace SeatsioDotNet.Test
{
    public class SeatsioClientTest
    {
        protected static readonly string BaseUrl = "https://api-staging-eu.seatsio.net";

        protected static readonly List<Category> TestChartCategories = new List<Category>()
        {
            new Category(9, "Cat1", "#87A9CD", false),
            new Category(10, "Cat2", "#5E42ED", false),
            new Category("string11", "Cat3", "#5E42BB", false)
        };

        protected readonly User User;
        protected readonly Workspace Workspace;
        protected readonly SeatsioClient Client;

        protected SeatsioClientTest()
        {
            TestCompany testCompany = CreateTestCompany();
            User = testCompany.admin;
            Workspace = testCompany.Workspace;
            Client = CreateSeatsioClient(User.SecretKey);
        }

        private TestCompany CreateTestCompany()
        {
            var restClient = new RestClient(BaseUrl);
            var request = new RestRequest("/system/public/users/actions/create-test-company", Method.Post);
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
            var options = new RestClientOptions(BaseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(User.SecretKey, null)
            };
            var restClient = new RestClient(options);
            var chartKey = Guid.NewGuid().ToString();
            var request = new RestRequest("/system/public/charts/{chartKey}", Method.Post)
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

        protected void WaitForStatusChanges(SeatsioClient client, Event evnt, int numStatusChanges)
        {
            var start = DateTimeOffset.Now;
            while (true)
            {
                var statusChanges = client.Events.StatusChanges(evnt.Key).All();
                if (statusChanges.ToList().Count != numStatusChanges)
                {
                    var duration = DateTimeOffset.Now.ToUnixTimeSeconds() - start.ToUnixTimeSeconds();
                    if (duration > 10)
                    {
                        throw new Exception("Event has no status changes: " + evnt.Key);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }
}