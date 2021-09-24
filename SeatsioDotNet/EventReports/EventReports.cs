using System.Collections.Generic;
using RestSharp;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.EventReports
{
    public class EventReports
    {
        private readonly RestClient _restClient;

        public EventReports(RestClient restClient)
        {
            _restClient = restClient;
        }

        public Dictionary<string, IEnumerable<EventObjectInfo>> ByLabel(string eventKey)
        {
            return FetchReport("byLabel", eventKey);
        }

        public IEnumerable<EventObjectInfo> ByLabel(string eventKey, string label)
        {
            return FetchReport("byLabel", eventKey, label);
        }

        public Dictionary<string, IEnumerable<EventObjectInfo>> ByStatus(string eventKey)
        {
            return FetchReport("byStatus", eventKey);
        }

        public Dictionary<string, EventReportSummaryItem> SummaryByStatus(string eventKey)
        {
            return FetchSummaryReport("byStatus", eventKey);
        }   
        
        public Dictionary<string, EventReportDeepSummaryItem> DeepSummaryByStatus(string eventKey)
        {
            return FetchDeepSummaryReport("byStatus", eventKey);
        }

        public IEnumerable<EventObjectInfo> ByStatus(string eventKey, string status)
        {
            return FetchReport("byStatus", eventKey, status);
        }      
        
        public Dictionary<string, IEnumerable<EventObjectInfo>> ByObjectType(string eventKey)
        {
            return FetchReport("byObjectType", eventKey);
        }

        public Dictionary<string, EventReportSummaryItem> SummaryByObjectType(string eventKey)
        {
            return FetchSummaryReport("byObjectType", eventKey);
        }   
        
        public Dictionary<string, EventReportDeepSummaryItem> DeepSummaryByObjectType(string eventKey)
        {
            return FetchDeepSummaryReport("byObjectType", eventKey);
        }

        public IEnumerable<EventObjectInfo> ByObjectType(string eventKey, string objectType)
        {
            return FetchReport("byObjectType", eventKey, objectType);
        }

        public Dictionary<string, IEnumerable<EventObjectInfo>> ByCategoryLabel(string eventKey)
        {
            return FetchReport("byCategoryLabel", eventKey);
        }

        public Dictionary<string, EventReportSummaryItem> SummaryByCategoryLabel(string eventKey)
        {
            return FetchSummaryReport("byCategoryLabel", eventKey);
        }
        
        public Dictionary<string, EventReportDeepSummaryItem> DeepSummaryByCategoryLabel(string eventKey)
        {
            return FetchDeepSummaryReport("byCategoryLabel", eventKey);
        }

        public IEnumerable<EventObjectInfo> ByCategoryLabel(string eventKey, string categoryLabel)
        {
            return FetchReport("byCategoryLabel", eventKey, categoryLabel);
        }

        public Dictionary<string, IEnumerable<EventObjectInfo>> ByCategoryKey(string eventKey)
        {
            return FetchReport("byCategoryKey", eventKey);
        }

        public Dictionary<string, EventReportSummaryItem> SummaryByCategoryKey(string eventKey)
        {
            return FetchSummaryReport("byCategoryKey", eventKey);
        }  
        
        public Dictionary<string, EventReportDeepSummaryItem> DeepSummaryByCategoryKey(string eventKey)
        {
            return FetchDeepSummaryReport("byCategoryKey", eventKey);
        }

        public IEnumerable<EventObjectInfo> ByCategoryKey(string eventKey, string categoryKey)
        {
            return FetchReport("byCategoryKey", eventKey, categoryKey);
        }

        public Dictionary<string, IEnumerable<EventObjectInfo>> ByOrderId(string eventKey)
        {
            return FetchReport("byOrderId", eventKey);
        }

        public IEnumerable<EventObjectInfo> ByOrderId(string eventKey, string categoryKey)
        {
            return FetchReport("byOrderId", eventKey, categoryKey);
        }

        public Dictionary<string, IEnumerable<EventObjectInfo>> BySection(string eventKey)
        {
            return FetchReport("bySection", eventKey);
        }

        public Dictionary<string, EventReportSummaryItem> SummaryBySection(string eventKey)
        {
            return FetchSummaryReport("bySection", eventKey);
        }   
        
        public Dictionary<string, EventReportDeepSummaryItem> DeepSummaryBySection(string eventKey)
        {
            return FetchDeepSummaryReport("bySection", eventKey);
        }

        public IEnumerable<EventObjectInfo> BySection(string eventKey, string section)
        {
            return FetchReport("bySection", eventKey, section);
        }  
        
        public Dictionary<string, IEnumerable<EventObjectInfo>> BySelectability(string eventKey)
        {
            return FetchReport("bySelectability", eventKey);
        }

        public Dictionary<string, EventReportSummaryItem> SummaryBySelectability(string eventKey)
        {
            return FetchSummaryReport("bySelectability", eventKey);
        }       
        
        public Dictionary<string, EventReportDeepSummaryItem> DeepSummaryBySelectability(string eventKey)
        {
            return FetchDeepSummaryReport("bySelectability", eventKey);
        }

        public IEnumerable<EventObjectInfo> BySelectability(string eventKey, string selectability)
        {
            return FetchReport("bySelectability", eventKey, selectability);
        }   
        
        public Dictionary<string, IEnumerable<EventObjectInfo>> ByChannel(string eventKey)
        {
            return FetchReport("byChannel", eventKey);
        }

        public Dictionary<string, EventReportSummaryItem> SummaryByChannel(string eventKey)
        {
            return FetchSummaryReport("byChannel", eventKey);
        }  
        
        public Dictionary<string, EventReportDeepSummaryItem> DeepSummaryByChannel(string eventKey)
        {
            return FetchDeepSummaryReport("byChannel", eventKey);
        }

        public IEnumerable<EventObjectInfo> ByChannel(string eventKey, string channelKey)
        {
            return FetchReport("byChannel", eventKey, channelKey);
        }

        private Dictionary<string, IEnumerable<EventObjectInfo>> FetchReport(string reportType, string eventKey)
        {
            var restRequest = new RestRequest("/reports/events/{key}/{reportType}", Method.GET)
                .AddUrlSegment("key", eventKey)
                .AddUrlSegment("reportType", reportType);
            return AssertOk(_restClient.Execute<Dictionary<string, IEnumerable<EventObjectInfo>>>(restRequest));
        }

        private Dictionary<string, EventReportSummaryItem> FetchSummaryReport(string reportType, string eventKey)
        {
            var restRequest = new RestRequest("/reports/events/{key}/{reportType}/summary", Method.GET)
                .AddUrlSegment("key", eventKey)
                .AddUrlSegment("reportType", reportType);
            return AssertOk(_restClient.Execute<Dictionary<string, EventReportSummaryItem>>(restRequest));
        }    
        
        private Dictionary<string, EventReportDeepSummaryItem> FetchDeepSummaryReport(string reportType, string eventKey)
        {
            var restRequest = new RestRequest("/reports/events/{key}/{reportType}/summary/deep", Method.GET)
                .AddUrlSegment("key", eventKey)
                .AddUrlSegment("reportType", reportType);
            return AssertOk(_restClient.Execute<Dictionary<string, EventReportDeepSummaryItem>>(restRequest));
        }

        private IEnumerable<EventObjectInfo> FetchReport(string reportType, string eventKey, string filter)
        {
            var restRequest = new RestRequest("/reports/events/{key}/{reportType}/{filter}", Method.GET)
                .AddUrlSegment("key", eventKey)
                .AddUrlSegment("reportType", reportType)
                .AddUrlSegment("filter", filter);
            var report = AssertOk(_restClient.Execute<Dictionary<string, IEnumerable<EventObjectInfo>>>(restRequest));
            if (report.ContainsKey(filter))
            {
                return report[filter];
            }

            return new List<EventObjectInfo>();
        }
    }
}