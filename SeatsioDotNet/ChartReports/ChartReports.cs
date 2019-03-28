using System.Collections.Generic;
using RestSharp;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.ChartReports
{
    public class ChartReports
    {
        private readonly RestClient _restClient;

        public ChartReports(RestClient restClient)
        {
            _restClient = restClient;
        }

        public Dictionary<string, IEnumerable<ChartReportItem>> ByLabel(string chartKey)
        {
            return FetchReport("byLabel", chartKey);
        }
        
        public Dictionary<string, IEnumerable<ChartReportItem>> ByCategoryKey(string chartKey)
        {
            return FetchReport("byCategoryKey", chartKey);
        }
        
        public Dictionary<string, IEnumerable<ChartReportItem>> ByCategoryLabel(string chartKey)
        {
            return FetchReport("byCategoryLabel", chartKey);
        }

        private Dictionary<string, IEnumerable<ChartReportItem>> FetchReport(string reportType, string chartKey)
        {
            var restRequest = new RestRequest("/reports/charts/{key}/{reportType}", Method.GET)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("reportType", reportType);
            return AssertOk(_restClient.Execute<Dictionary<string, IEnumerable<ChartReportItem>>>(restRequest));
        }
    }
}