using System.Collections.Generic;
using RestSharp;
using SeatsioDotNet.UsageReports.DetailsForEventInMonth;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.UsageReports
{
    public class UsageReports
    {
        private readonly RestClient _restClient;

        public UsageReports(RestClient restClient)
        {
            _restClient = restClient;
        }

        public IEnumerable<UsageSummaryForMonth> SummaryForAllMonths()
        {
            var restRequest = new RestRequest("/reports/usage");
            return AssertOk(_restClient.Execute<List<UsageSummaryForMonth>>(restRequest));
        }

        public IEnumerable<UsageDetails> DetailsForMonth(UsageMonth month)
        {
            var restRequest = new RestRequest("/reports/usage/month/{month}")
                .AddUrlSegment("month", month.Serialize());
            return AssertOk(_restClient.Execute<List<UsageDetails>>(restRequest));
        }

        public IEnumerable<object> DetailsForEventInMonth(long eventId, UsageMonth month)
        {
            var restRequest = new RestRequest("/reports/usage/month/{month}/event/{event}")
                .AddUrlSegment("month", month.Serialize())
                .AddUrlSegment("event", eventId.ToString());
            if (month.Before(2022, 12))
            {
                return AssertOk(_restClient.Execute<List<UsageForObjectV1>>(restRequest));
            }
            else
            {
                return AssertOk(_restClient.Execute<List<UsageForObjectV2>>(restRequest));
            }
        }
    }
}