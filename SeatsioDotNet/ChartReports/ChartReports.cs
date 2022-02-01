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

        public Dictionary<string, IEnumerable<ChartObjectInfo>> ByLabel(string chartKey, string bookWholeTablesMode = null)
        {
            return FetchReport("byLabel", chartKey, bookWholeTablesMode);
        }    
        
        public Dictionary<string, IEnumerable<ChartObjectInfo>> ByObjectType(string chartKey, string bookWholeTablesMode = null)
        {
            return FetchReport("byObjectType", chartKey, bookWholeTablesMode);
        }
        
        public Dictionary<string, ChartReportSummaryItem> SummaryByObjectType(string chartKey, string bookWholeTablesMode = null)
        {
            return FetchSummaryReport("byObjectType", chartKey, bookWholeTablesMode);
        }
        
        public Dictionary<string, IEnumerable<ChartObjectInfo>> ByCategoryKey(string chartKey, string bookWholeTablesMode = null)
        {
            return FetchReport("byCategoryKey", chartKey, bookWholeTablesMode);
        }
        
        public Dictionary<string, ChartReportSummaryItem> SummaryByCategoryKey(string chartKey, string bookWholeTablesMode = null)
        {
            return FetchSummaryReport("byCategoryKey", chartKey, bookWholeTablesMode);
        }
        
        public Dictionary<string, IEnumerable<ChartObjectInfo>> ByCategoryLabel(string chartKey, string bookWholeTablesMode = null)
        {
            return FetchReport("byCategoryLabel", chartKey, bookWholeTablesMode);
        }   
        
        public Dictionary<string, ChartReportSummaryItem> SummaryByCategoryLabel(string chartKey, string bookWholeTablesMode = null)
        {
            return FetchSummaryReport("byCategoryLabel", chartKey, bookWholeTablesMode);
        }
        
        public Dictionary<string, IEnumerable<ChartObjectInfo>> BySection(string chartKey, string bookWholeTablesMode = null)
        {
            return FetchReport("bySection", chartKey, bookWholeTablesMode);
        }
        
        public Dictionary<string, ChartReportSummaryItem> SummaryBySection(string eventKey, string bookWholeTablesMode = null)
        {
            return FetchSummaryReport("bySection", eventKey, bookWholeTablesMode);
        }

        private Dictionary<string, IEnumerable<ChartObjectInfo>> FetchReport(string reportType, string chartKey, string bookWholeTablesMode)
        {
            var restRequest = new RestRequest("/reports/charts/{key}/{reportType}", Method.GET)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("reportType", reportType);
            
            if (bookWholeTablesMode != null)
            {
                restRequest.AddQueryParameter("bookWholeTables", bookWholeTablesMode);
            }
            
            return AssertOk(_restClient.Execute<Dictionary<string, IEnumerable<ChartObjectInfo>>>(restRequest));
        }
        
        private Dictionary<string, ChartReportSummaryItem> FetchSummaryReport(string reportType, string chartKey, string bookWholeTablesMode)
        {
            var restRequest = new RestRequest("/reports/charts/{key}/{reportType}/summary", Method.GET)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("reportType", reportType);
            
            if (bookWholeTablesMode != null)
            {
                restRequest.AddQueryParameter("bookWholeTables", bookWholeTablesMode);
            }
            
            return AssertOk(_restClient.Execute<Dictionary<string, ChartReportSummaryItem>>(restRequest));
        }

    }
}