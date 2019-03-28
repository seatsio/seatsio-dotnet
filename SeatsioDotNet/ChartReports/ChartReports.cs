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

        public Dictionary<string, IEnumerable<ChartReportItem>> ByLabel(string eventKey)
        {
            return FetchReport("byLabel", eventKey);
        }
        
        public Dictionary<string, IEnumerable<ChartReportItem>> ByCategoryKey(string eventKey)
        {
            return FetchReport("byCategoryKey", eventKey);
        }
        
        public Dictionary<string, IEnumerable<ChartReportItem>> ByCategoryLabel(string eventKey)
        {
            return FetchReport("byCategoryLabel", eventKey);
        }

        private Dictionary<string, IEnumerable<ChartReportItem>> FetchReport(string reportType, string eventKey)
        {
            var restRequest = new RestRequest("/reports/charts/{key}/{reportType}", Method.GET)
                .AddUrlSegment("key", eventKey)
                .AddUrlSegment("reportType", reportType);
            return AssertOk(_restClient.Execute<Dictionary<string, IEnumerable<ChartReportItem>>>(restRequest));
        }
    }
}