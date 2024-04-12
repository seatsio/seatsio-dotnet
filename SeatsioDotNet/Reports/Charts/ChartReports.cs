using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using SeatsioDotNet.ChartReports;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Reports.Charts;

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

    public async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> ByLabelAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byLabel", chartKey, bookWholeTablesMode, version, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> ByObjectTypeAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byObjectType", chartKey, bookWholeTablesMode, version, cancellationToken);
    }

    public async Task<Dictionary<string, ChartReportSummaryItem>> SummaryByObjectTypeAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReportAsync("byObjectType", chartKey, bookWholeTablesMode, version, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> ByCategoryKeyAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byCategoryKey", chartKey, bookWholeTablesMode, version, cancellationToken);
    }

    public async Task<Dictionary<string, ChartReportSummaryItem>> SummaryByCategoryKeyAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReportAsync("byCategoryKey", chartKey, bookWholeTablesMode, version, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> ByCategoryLabelAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("byCategoryLabel", chartKey, bookWholeTablesMode, version, cancellationToken);
    }

    public async Task<Dictionary<string, ChartReportSummaryItem>> SummaryByCategoryLabelAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReportAsync("byCategoryLabel", chartKey, bookWholeTablesMode, version, cancellationToken);
    }

    public async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> BySectionAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production, CancellationToken cancellationToken = default)
    {
        return await FetchReportAsync("bySection", chartKey, bookWholeTablesMode, version, cancellationToken);
    }

    public async Task<Dictionary<string, ChartReportSummaryItem>> SummaryBySectionAsync(string eventKey, string bookWholeTablesMode = null, Version version = Version.Production, CancellationToken cancellationToken = default)
    {
        return await FetchSummaryReportAsync("bySection", eventKey, bookWholeTablesMode, version, cancellationToken);
    }

    private async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> FetchReportAsync(string reportType, string chartKey, string bookWholeTablesMode, Version version = Version.Production, CancellationToken cancellationToken = default)
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

        return AssertOk(await _restClient.ExecuteAsync<Dictionary<string, IEnumerable<ChartObjectInfo>>>(restRequest, cancellationToken));
    }

    private async Task<Dictionary<string, ChartReportSummaryItem>> FetchSummaryReportAsync(string reportType, string chartKey, string bookWholeTablesMode, Version version = Version.Production, CancellationToken cancellationToken = default)
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

        return AssertOk(await _restClient.ExecuteAsync<Dictionary<string, ChartReportSummaryItem>>(restRequest, cancellationToken));
    }
}