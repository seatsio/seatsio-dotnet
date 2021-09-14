using System;
using System.Net;
using System.Threading;
using RestSharp;
using RestSharp.Authenticators;

namespace SeatsioDotNet
{
    public class SeatsioClient
    {
        public Charts.Charts Charts { get; }
        public Events.Events Events { get; }
        public Subaccounts.Subaccounts Subaccounts { get; }
        public Workspaces.Workspaces Workspaces { get; }
        public HoldTokens.HoldTokens HoldTokens { get; }
        public EventReports.EventReports EventReports { get; }
        public ChartReports.ChartReports ChartReports { get; }
        public UsageReports.UsageReports UsageReports { get; }

        private SeatsioRestClient RestClient;

        static SeatsioClient()
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
        }

        public SeatsioClient(string secretKey, string workspaceKey, string baseUrl)
        {
            RestClient = CreateRestClient(secretKey, workspaceKey, baseUrl);
            Charts = new Charts.Charts(RestClient);
            Events = new Events.Events(RestClient);
            Subaccounts = new Subaccounts.Subaccounts(RestClient);
            Workspaces = new Workspaces.Workspaces(RestClient);
            HoldTokens = new HoldTokens.HoldTokens(RestClient);
            EventReports = new EventReports.EventReports(RestClient);
            ChartReports = new ChartReports.ChartReports(RestClient);
            UsageReports = new UsageReports.UsageReports(RestClient);
        }

        public SeatsioClient(Region region, string secretKey, string workspaceKey) : this(secretKey, workspaceKey,
            region.Url())
        {
        }

        public SeatsioClient(Region region, string secretKey) : this(region, secretKey, null)
        {
        }

        public SeatsioClient SetMaxRetries(int maxRetries)
        {
            RestClient.SetMaxRetries(maxRetries);
            return this;
        }

        private static SeatsioRestClient CreateRestClient(string secretKey, string workspaceKey, string baseUrl)
        {
            var client = new SeatsioRestClient(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(secretKey, null);
            if (workspaceKey != null)
            {
                client.AddDefaultHeader("X-Workspace-Key", workspaceKey.ToString());
            }

            return client;
        }
    }
}

public class SeatsioRestClient : RestClient
{
    private int MaxRetries = 5;

    public SeatsioRestClient(string baseUrl) : base(baseUrl)
    {
    }

    public override IRestResponse<T> Execute<T>(IRestRequest request)
    {
        var retryCount = 0;
        var response = base.Execute<T>(request);
        while (retryCount < MaxRetries && (int) response.StatusCode == 429)
        {
            var waitTime = (int) Math.Pow(2, retryCount + 2) * 100;
            retryCount++;
            Thread.Sleep(waitTime);
            response = base.Execute<T>(request);
        }

        return response;
    }

    public SeatsioRestClient SetMaxRetries(int maxRetries)
    {
        MaxRetries = maxRetries;
        return this;
    }
}