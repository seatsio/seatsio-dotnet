using System.Collections.Generic;
using RestSharp;
using SeatsioDotNet.ChartReports;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Reports.Charts
{
    public class ChartReports
    {
        public enum Version
        {
            Production,
            Draft
        }
        
        private readonly RestClient _restClient;

        public ChartReports(RestClient restClient)
        {
            _restClient = restClient;
        }

        public Dictionary<string, IEnumerable<ChartObjectInfo>> ByLabel(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
        {
            return FetchReport("byLabel", chartKey, bookWholeTablesMode, version);
        }    
        
        public Dictionary<string, IEnumerable<ChartObjectInfo>> ByObjectType(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
        {
            return FetchReport("byObjectType", chartKey, bookWholeTablesMode, version);
        }
        
        public Dictionary<string, ChartReportSummaryItem> SummaryByObjectType(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
        {
            return FetchSummaryReport("byObjectType", chartKey, bookWholeTablesMode, version);
        }
        
        public Dictionary<string, IEnumerable<ChartObjectInfo>> ByCategoryKey(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
        {
            return FetchReport("byCategoryKey", chartKey, bookWholeTablesMode, version);
        }
        
        public Dictionary<string, ChartReportSummaryItem> SummaryByCategoryKey(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
        {
            return FetchSummaryReport("byCategoryKey", chartKey, bookWholeTablesMode, version);
        }
        
        public Dictionary<string, IEnumerable<ChartObjectInfo>> ByCategoryLabel(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
        {
            return FetchReport("byCategoryLabel", chartKey, bookWholeTablesMode, version);
        }   
        
        public Dictionary<string, ChartReportSummaryItem> SummaryByCategoryLabel(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
        {
            return FetchSummaryReport("byCategoryLabel", chartKey, bookWholeTablesMode, version);
        }
        
        public Dictionary<string, IEnumerable<ChartObjectInfo>> BySection(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
        {
            return FetchReport("bySection", chartKey, bookWholeTablesMode, version);
        }
        
        public Dictionary<string, ChartReportSummaryItem> SummaryBySection(string eventKey, string bookWholeTablesMode = null, Version version = Version.Production)
        {
            return FetchSummaryReport("bySection", eventKey, bookWholeTablesMode, version);
        }

        private Dictionary<string, IEnumerable<ChartObjectInfo>> FetchReport(string reportType, string chartKey, string bookWholeTablesMode, Version version = Version.Production)
        {
            var restRequest = new RestRequest("/reports/charts/{key}/{reportType}", Method.Get)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("reportType", reportType);
            
            if (bookWholeTablesMode != null)
            {
                restRequest.AddQueryParameter("bookWholeTables", bookWholeTablesMode);
            }
            if (version == Version.Draft)
            {
                restRequest.AddQueryParameter("version", version.ToString());
            }
            
            return AssertOk(_restClient.Execute<Dictionary<string, IEnumerable<ChartObjectInfo>>>(restRequest));
        }
        
        private Dictionary<string, ChartReportSummaryItem> FetchSummaryReport(string reportType, string chartKey, string bookWholeTablesMode, Version version = Version.Production)
        {
            var restRequest = new RestRequest("/reports/charts/{key}/{reportType}/summary", Method.Get)
                .AddUrlSegment("key", chartKey)
                .AddUrlSegment("reportType", reportType);
            
            if (bookWholeTablesMode != null)
            {
                restRequest.AddQueryParameter("bookWholeTables", bookWholeTablesMode);
            }
            if (version == Version.Draft)
            {
                restRequest.AddQueryParameter("version", version.ToString());
            }
            
            return AssertOk(_restClient.Execute<Dictionary<string, ChartReportSummaryItem>>(restRequest));
        }
    }
}