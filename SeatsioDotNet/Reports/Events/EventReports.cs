using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using SeatsioDotNet.EventReports;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Reports.Events;

public class EventReports
{
    private readonly RestClient _restClient;

    public EventReports(RestClient restClient)
    {
        _restClient = restClient;
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByLabelAsync(string eventKey)
    {
        return await FetchReportAsync("byLabel", eventKey);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByLabelAsync(string eventKey, string label)
    {
        return await FetchReport("byLabel", eventKey, label);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByStatusAsync(string eventKey)
    {
        return await FetchReportAsync("byStatus", eventKey);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByStatusAsync(string eventKey)
    {
        return await FetchSummaryReport("byStatus", eventKey);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByStatusAsync(string eventKey)
    {
        return await FetchDeepSummaryReport("byStatus", eventKey);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByStatusAsync(string eventKey, string status)
    {
        return await FetchReport("byStatus", eventKey, status);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByObjectTypeAsync(string eventKey)
    {
        return await FetchReportAsync("byObjectType", eventKey);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByObjectTypeAsync(string eventKey)
    {
        return await FetchSummaryReport("byObjectType", eventKey);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByObjectTypeAsync(string eventKey)
    {
        return await FetchDeepSummaryReport("byObjectType", eventKey);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByObjectTypeAsync(string eventKey, string objectType)
    {
        return await FetchReport("byObjectType", eventKey, objectType);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByCategoryLabelAsync(string eventKey)
    {
        return await FetchReportAsync("byCategoryLabel", eventKey);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByCategoryLabeAsync(string eventKey)
    {
        return await FetchSummaryReport("byCategoryLabel", eventKey);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByCategoryLabelAsync(string eventKey)
    {
        return await FetchDeepSummaryReport("byCategoryLabel", eventKey);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByCategoryLabelAsync(string eventKey, string categoryLabel)
    {
        return await FetchReport("byCategoryLabel", eventKey, categoryLabel);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByCategoryKeyAsync(string eventKey)
    {
        return await FetchReportAsync("byCategoryKey", eventKey);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByCategoryKeyAsync(string eventKey)
    {
        return await FetchSummaryReport("byCategoryKey", eventKey);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByCategoryKeyAsync(string eventKey)
    {
        return await FetchDeepSummaryReport("byCategoryKey", eventKey);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByCategoryKeyAsync(string eventKey, string categoryKey)
    {
        return await FetchReport("byCategoryKey", eventKey, categoryKey);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByOrderIdAsync(string eventKey)
    {
        return await FetchReportAsync("byOrderId", eventKey);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByOrderIdAsync(string eventKey, string categoryKey)
    {
        return await FetchReport("byOrderId", eventKey, categoryKey);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> BySectionAsync(string eventKey)
    {
        return await FetchReportAsync("bySection", eventKey);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryBySectionAsync(string eventKey)
    {
        return await FetchSummaryReport("bySection", eventKey);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryBySectionAsync(string eventKey)
    {
        return await FetchDeepSummaryReport("bySection", eventKey);
    }

    public async Task<IEnumerable<EventObjectInfo>> BySection(string eventKey, string section)
    {
        return await FetchReport("bySection", eventKey, section);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByAvailabilityAsync(string eventKey)
    {
        return await FetchReportAsync("byAvailability", eventKey);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByAvailabilityAsync(string eventKey, string availability)
    {
        return await FetchReport("byAvailability", eventKey, availability);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByAvailabilityAsync(string eventKey)
    {
        return await FetchSummaryReport("byAvailability", eventKey);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByAvailabilityAsync(string eventKey)
    {
        return await FetchDeepSummaryReport("byAvailability", eventKey);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByAvailabilityReasonAsync(string eventKey)
    {
        return await FetchReportAsync("byAvailabilityReason", eventKey);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByAvailabilityReasonAsync(string eventKey, string availabilityReason)
    {
        return await FetchReport("byAvailabilityReason", eventKey, availabilityReason);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByAvailabilityReasonAsync(string eventKey)
    {
        return await FetchSummaryReport("byAvailabilityReason", eventKey);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByAvailabilityReasonAsync(string eventKey)
    {
        return await FetchDeepSummaryReport("byAvailabilityReason", eventKey);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByChannelAsync(string eventKey)
    {
        return await FetchReportAsync("byChannel", eventKey);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByChannelAsync(string eventKey)
    {
        return await FetchSummaryReport("byChannel", eventKey);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByChannelAsync(string eventKey)
    {
        return await FetchDeepSummaryReport("byChannel", eventKey);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByChannelAsync(string eventKey, string channelKey)
    {
        return await FetchReport("byChannel", eventKey, channelKey);
    }

    private async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> FetchReportAsync(string reportType, string eventKey)
    {
        var restRequest = new RestRequest("/reports/events/{key}/{reportType}", Method.Get)
            .AddUrlSegment("key", eventKey)
            .AddUrlSegment("reportType", reportType);
        return AssertOk(await _restClient.ExecuteAsync<Dictionary<string, IEnumerable<EventObjectInfo>>>(restRequest));
    }

    private async Task<Dictionary<string, EventReportSummaryItem>> FetchSummaryReport(string reportType, string eventKey)
    {
        var restRequest = new RestRequest("/reports/events/{key}/{reportType}/summary", Method.Get)
            .AddUrlSegment("key", eventKey)
            .AddUrlSegment("reportType", reportType);
        return AssertOk(await _restClient.ExecuteAsync<Dictionary<string, EventReportSummaryItem>>(restRequest));
    }

    private async Task<Dictionary<string, EventReportDeepSummaryItem>> FetchDeepSummaryReport(string reportType,
        string eventKey)
    {
        var restRequest = new RestRequest("/reports/events/{key}/{reportType}/summary/deep", Method.Get)
            .AddUrlSegment("key", eventKey)
            .AddUrlSegment("reportType", reportType);
        return AssertOk(await _restClient.ExecuteAsync<Dictionary<string, EventReportDeepSummaryItem>>(restRequest));
    }

    private async Task<IEnumerable<EventObjectInfo>> FetchReport(string reportType, string eventKey, string filter)
    {
        var restRequest = new RestRequest("/reports/events/{key}/{reportType}/{filter}", Method.Get)
            .AddUrlSegment("key", eventKey)
            .AddUrlSegment("reportType", reportType)
            .AddUrlSegment("filter", filter);
        var report = AssertOk(await _restClient.ExecuteAsync<Dictionary<string, IEnumerable<EventObjectInfo>>>(restRequest));
        if (report.ContainsKey(filter))
        {
            return report[filter];
        }

        return new List<EventObjectInfo>();
    }
}