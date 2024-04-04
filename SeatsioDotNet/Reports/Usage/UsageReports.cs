using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using SeatsioDotNet.Reports.Usage.DetailsForEventInMonth;
using SeatsioDotNet.Reports.Usage.DetailsForMonth;
using SeatsioDotNet.Reports.Usage.SummaryForMonths;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Reports.Usage;

public class UsageReports
{
    private readonly RestClient _restClient;

    public UsageReports(RestClient restClient)
    {
        _restClient = restClient;
    }

    public async Task<UsageSummaryForAllMonths> SummaryForAllMonthsAsync()
    {
        var restRequest = new RestRequest("/reports/usage?version=2");
        return AssertOk(await _restClient.ExecuteAsync<UsageSummaryForAllMonths>(restRequest));
    }

    public async Task<IEnumerable<UsageDetails>> DetailsForMonthAsync(UsageMonth month)
    {
        var restRequest = new RestRequest("/reports/usage/month/{month}")
            .AddUrlSegment("month", month.Serialize());
        return AssertOk(await _restClient.ExecuteAsync<IEnumerable<UsageDetails>>(restRequest));
    }

    public async Task<IEnumerable<object>> DetailsForEventInMonthAsync(long eventId, UsageMonth month)
    {
        var restRequest = new RestRequest("/reports/usage/month/{month}/event/{event}")
            .AddUrlSegment("month", month.Serialize())
            .AddUrlSegment("event", eventId.ToString());
        if (month.Before(2022, 12))
        {
            return AssertOk(await _restClient.ExecuteAsync<IEnumerable<UsageForObjectV1>>(restRequest));
        }
        else
        {
            return AssertOk(await _restClient.ExecuteAsync<IEnumerable<UsageForObjectV2>>(restRequest));
        }
    }
}