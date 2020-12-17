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

        public SeatsioClient(string secretKey, string workspaceKey, string baseUrl)
        {
            var restClient = CreateRestClient(secretKey, workspaceKey, baseUrl);
            Charts = new Charts.Charts(restClient);
            Events = new Events.Events(restClient);
            Subaccounts = new Subaccounts.Subaccounts(restClient);
            Workspaces = new Workspaces.Workspaces(restClient);
            HoldTokens = new HoldTokens.HoldTokens(restClient);
            EventReports = new EventReports.EventReports(restClient);
            ChartReports = new ChartReports.ChartReports(restClient);
            UsageReports = new UsageReports.UsageReports(restClient);
        }

        public SeatsioClient(Region region, string secretKey, string workspaceKey) : this(secretKey, workspaceKey, region.Url())
        {
        }

        public SeatsioClient(Region region, string secretKey) : this(region, secretKey, null)
        {
        }

        private static RestClient CreateRestClient(string secretKey, string workspaceKey, string baseUrl)
        {
            var client = new RestClient(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(secretKey, null);
            if (workspaceKey != null)
            {
                client.AddDefaultHeader("X-Workspace-Key", workspaceKey.ToString());
            }

            return client;
        }
    }
}