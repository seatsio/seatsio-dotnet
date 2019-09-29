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

        public SeatsioClient(string secretKey, long? accountId, string baseUrl)
        {
            var restClient = CreateRestClient(secretKey, accountId, baseUrl);
            Charts = new Charts.Charts(restClient);
            Events = new Events.Events(restClient);
            Accounts = new Accounts.Accounts(restClient);
            Subaccounts = new Subaccounts.Subaccounts(restClient);
            HoldTokens = new HoldTokens.HoldTokens(restClient);
            EventReports = new EventReports.EventReports(restClient);
            ChartReports = new ChartReports.ChartReports(restClient);
            UsageReports = new UsageReports.UsageReports(restClient);
        }

        public SeatsioClient(string secretKey, long? accountId) : this(secretKey, accountId, "https://api.seatsio.net")
        {
        }

        public SeatsioClient(string secretKey) : this(secretKey, null)
        {
        }

        private static RestClient CreateRestClient(string secretKey, long? accountId, string baseUrl)
        {
            var client = new RestClient(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(secretKey, null);
            if (accountId != null)
            {
                client.AddDefaultHeader("X-Account-ID", accountId.ToString());
            }

            return client;
        }
    }
}