using System.Collections.Generic;
using System.Threading;
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

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByLabelAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byLabel", eventKey, cancellationToken);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByLabelAsync(string eventKey, string label, CancellationToken cancellationToken = default)
    {
        return await FetchReport("byLabel", eventKey, label, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByStatusAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byStatus", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByStatusAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReport("byStatus", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByStatusAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchDeepSummaryReport("byStatus", eventKey, cancellationToken);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByStatusAsync(string eventKey, string status, CancellationToken cancellationToken = default)
    {
        return await FetchReport("byStatus", eventKey, status, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByObjectTypeAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byObjectType", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByObjectTypeAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReport("byObjectType", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByObjectTypeAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchDeepSummaryReport("byObjectType", eventKey, cancellationToken);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByObjectTypeAsync(string eventKey, string objectType, CancellationToken cancellationToken = default)
    {
        return await FetchReport("byObjectType", eventKey, objectType, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByCategoryLabelAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byCategoryLabel", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByCategoryLabeAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReport("byCategoryLabel", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByCategoryLabelAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchDeepSummaryReport("byCategoryLabel", eventKey, cancellationToken);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByCategoryLabelAsync(string eventKey, string categoryLabel, CancellationToken cancellationToken = default)
    {
        return await FetchReport("byCategoryLabel", eventKey, categoryLabel, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByCategoryKeyAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byCategoryKey", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByCategoryKeyAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReport("byCategoryKey", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByCategoryKeyAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchDeepSummaryReport("byCategoryKey", eventKey, cancellationToken);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByCategoryKeyAsync(string eventKey, string categoryKey, CancellationToken cancellationToken = default)
    {
        return await FetchReport("byCategoryKey", eventKey, categoryKey, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByOrderIdAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byOrderId", eventKey, cancellationToken);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByOrderIdAsync(string eventKey, string categoryKey, CancellationToken cancellationToken = default)
    {
        return await FetchReport("byOrderId", eventKey, categoryKey, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> BySectionAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("bySection", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryBySectionAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReport("bySection", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryBySectionAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchDeepSummaryReport("bySection", eventKey, cancellationToken);
    }

    public async Task<IEnumerable<EventObjectInfo>> BySectionAsync(string eventKey, string section, CancellationToken cancellationToken = default)
    {
        return await FetchReport("bySection", eventKey, section, cancellationToken);
    }
    
    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByZoneAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byZone", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByZoneAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReport("byZone", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByZoneAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchDeepSummaryReport("byZone", eventKey, cancellationToken);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByZoneAsync(string eventKey, string zone, CancellationToken cancellationToken = default)
    {
        return await FetchReport("byZone", eventKey, zone, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByAvailabilityAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byAvailability", eventKey, cancellationToken);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByAvailabilityAsync(string eventKey, string availability, CancellationToken cancellationToken = default)
    {
        return await FetchReport("byAvailability", eventKey, availability, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByAvailabilityAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReport("byAvailability", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByAvailabilityAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchDeepSummaryReport("byAvailability", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByAvailabilityReasonAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byAvailabilityReason", eventKey, cancellationToken);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByAvailabilityReasonAsync(string eventKey, string availabilityReason, CancellationToken cancellationToken = default)
    {
        return await FetchReport("byAvailabilityReason", eventKey, availabilityReason, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByAvailabilityReasonAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReport("byAvailabilityReason", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByAvailabilityReasonAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchDeepSummaryReport("byAvailabilityReason", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> ByChannelAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byChannel", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportSummaryItem>> SummaryByChannelAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReport("byChannel", eventKey, cancellationToken);
    }

    public async Task<Dictionary<string, EventReportDeepSummaryItem>> DeepSummaryByChannelAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        return await FetchDeepSummaryReport("byChannel", eventKey, cancellationToken);
    }

    public async Task<IEnumerable<EventObjectInfo>> ByChannelAsync(string eventKey, string channelKey, CancellationToken cancellationToken = default)
    {
        return await FetchReport("byChannel", eventKey, channelKey, cancellationToken);
    }

    private async Task<Dictionary<string, IEnumerable<EventObjectInfo>>> FetchReportAsync(string reportType, string eventKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/reports/events/{key}/{reportType}", Method.Get)
            .AddUrlSegment("key", eventKey)
            .AddUrlSegment("reportType", reportType);
        return AssertOk(await _restClient.ExecuteAsync<Dictionary<string, IEnumerable<EventObjectInfo>>>(restRequest, cancellationToken));
    }

    private async Task<Dictionary<string, EventReportSummaryItem>> FetchSummaryReport(string reportType, string eventKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/reports/events/{key}/{reportType}/summary", Method.Get)
            .AddUrlSegment("key", eventKey)
            .AddUrlSegment("reportType", reportType);
        return AssertOk(await _restClient.ExecuteAsync<Dictionary<string, EventReportSummaryItem>>(restRequest, cancellationToken));
    }

    private async Task<Dictionary<string, EventReportDeepSummaryItem>> FetchDeepSummaryReport(string reportType,
        string eventKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/reports/events/{key}/{reportType}/summary/deep", Method.Get)
            .AddUrlSegment("key", eventKey)
            .AddUrlSegment("reportType", reportType);
        return AssertOk(await _restClient.ExecuteAsync<Dictionary<string, EventReportDeepSummaryItem>>(restRequest, cancellationToken));
    }

    private async Task<IEnumerable<EventObjectInfo>> FetchReport(string reportType, string eventKey, string filter, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/reports/events/{key}/{reportType}/{filter}", Method.Get)
            .AddUrlSegment("key", eventKey)
            .AddUrlSegment("reportType", reportType)
            .AddUrlSegment("filter", filter);
        var report = AssertOk(await _restClient.ExecuteAsync<Dictionary<string, IEnumerable<EventObjectInfo>>>(restRequest, cancellationToken));
        if (report.ContainsKey(filter))
        {
            return report[filter];
        }

        return new List<EventObjectInfo>();
    }
}