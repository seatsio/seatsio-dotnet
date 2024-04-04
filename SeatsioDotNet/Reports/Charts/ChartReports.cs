using System.Collections.Generic;
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

    public async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> ByLabelAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
    {
        return await FetchReportAsync("byLabel", chartKey, bookWholeTablesMode, version);
    }    
        
    public async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> ByObjectTypeAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
    {
        return await FetchReportAsync("byObjectType", chartKey, bookWholeTablesMode, version);
    }
        
    public async Task<Dictionary<string, ChartReportSummaryItem>> SummaryByObjectTypeAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
    {
        return await FetchSummaryReportAsync("byObjectType", chartKey, bookWholeTablesMode, version);
    }
        
    public async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> ByCategoryKeyAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
    {
        return await FetchReportAsync("byCategoryKey", chartKey, bookWholeTablesMode, version);
    }
        
    public async Task<Dictionary<string, ChartReportSummaryItem>> SummaryByCategoryKeyAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
    {
        return await FetchSummaryReportAsync("byCategoryKey", chartKey, bookWholeTablesMode, version);
    }
        
    public async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> ByCategoryLabelAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
    {
        return await FetchReportAsync("byCategoryLabel", chartKey, bookWholeTablesMode, version);
    }   
        
    public async Task<Dictionary<string, ChartReportSummaryItem>> SummaryByCategoryLabelAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
    {
        return await FetchSummaryReportAsync("byCategoryLabel", chartKey, bookWholeTablesMode, version);
    }
        
    public async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> BySectionAsync(string chartKey, string bookWholeTablesMode = null, Version version = Version.Production)
    {
        return await FetchReportAsync("bySection", chartKey, bookWholeTablesMode, version);
    }
        
    public async Task<Dictionary<string, ChartReportSummaryItem>> SummaryBySectionAsync(string eventKey, string bookWholeTablesMode = null, Version version = Version.Production)
    {
        return await FetchSummaryReportAsync("bySection", eventKey, bookWholeTablesMode, version);
    }

    private async Task<Dictionary<string, IEnumerable<ChartObjectInfo>>> FetchReportAsync(string reportType, string chartKey, string bookWholeTablesMode, Version version = Version.Production)
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
            
        return AssertOk(await _restClient.ExecuteAsync<Dictionary<string, IEnumerable<ChartObjectInfo>>>(restRequest));
    }
        
    private async Task<Dictionary<string, ChartReportSummaryItem>> FetchSummaryReportAsync(string reportType, string chartKey, string bookWholeTablesMode, Version version = Version.Production)
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
            
        return AssertOk(await _restClient.ExecuteAsync<Dictionary<string, ChartReportSummaryItem>>(restRequest));
    }
}