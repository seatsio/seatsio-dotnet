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

        public Dictionary<string, IEnumerable<ChartReportItem>> ByLabel(string chartKey, string bookWholeTablesMode)
        {
            return FetchReport("byLabel", chartKey, bookWholeTablesMode);
        }    
        
        public Dictionary<string, IEnumerable<ChartReportItem>> ByObjectType(string chartKey, string bookWholeTablesMode)
        {
            return FetchReport("byObjectType", chartKey, bookWholeTablesMode);
        }
        
        public Dictionary<string, IEnumerable<ChartReportItem>> ByCategoryKey(string chartKey, string bookWholeTablesMode)
        {
            return FetchReport("byCategoryKey", chartKey, bookWholeTablesMode);
        }
        
        public Dictionary<string, IEnumerable<ChartReportItem>> ByCategoryLabel(string chartKey, string bookWholeTablesMode)
        {
            return FetchReport("byCategoryLabel", chartKey, bookWholeTablesMode);
        }

        private Dictionary<string, IEnumerable<ChartReportItem>> FetchReport(string reportType, string chartKey, string bookWholeTablesMode)
        {
            var restRequest = new RestRequest("/reports/charts/{key}/{reportType}", Method.GET)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("reportType", reportType);
            
            if (bookWholeTablesMode != null)
            {
                restRequest.AddQueryParameter("bookWholeTables", bookWholeTablesMode);
            }
            
            return AssertOk(_restClient.Execute<Dictionary<string, IEnumerable<ChartReportItem>>>(restRequest));
        }
    }
}