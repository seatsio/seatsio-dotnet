using RestSharp;
using RestSharp.Authenticators;

namespace SeatsioDotNet
{
    public class SeatsioClient
    {
        public Charts.Charts Charts { get; }
        public Events.Events Events { get; }
        public Accounts.Accounts Accounts { get; }
        public Subaccounts.Subaccounts Subaccounts { get; }
        public HoldTokens.HoldTokens HoldTokens { get; }
        public EventReports.EventReports EventReports { get; }
        public ChartReports.ChartReports ChartReports { get; }
        public UsageReports.UsageReports UsageReports { get; }

        public SeatsioClient(string secretKey, string baseUrl)
        {
            var restClient = CreateRestClient(secretKey, baseUrl);
            Charts = new Charts.Charts(restClient);
            Events = new Events.Events(restClient);
            Accounts = new Accounts.Accounts(restClient);
            Subaccounts = new Subaccounts.Subaccounts(restClient);
            HoldTokens = new HoldTokens.HoldTokens(restClient);
            EventReports = new EventReports.EventReports(restClient);
            ChartReports = new ChartReports.ChartReports(restClient);
            UsageReports = new UsageReports.UsageReports(restClient);
        }

        public SeatsioClient(string secretKey): this(secretKey, "https://api.seatsio.net")
        {
        }

        private static RestClient CreateRestClient(string secretKey, string baseUrl)
        {
            var client = new RestClient(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(secretKey, null);
            return client;
        }
    }
}