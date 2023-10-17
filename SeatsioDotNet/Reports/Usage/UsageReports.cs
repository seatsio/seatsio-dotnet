using System.Collections;
using System.Collections.Generic;
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

    public UsageSummaryForAllMonths SummaryForAllMonths()
    {
        var restRequest = new RestRequest("/reports/usage?version=2");
        return AssertOk(_restClient.Execute<UsageSummaryForAllMonths>(restRequest));
    }

    public IEnumerable<UsageDetails> DetailsForMonth(UsageMonth month)
    {
        var restRequest = new RestRequest("/reports/usage/month/{month}")
            .AddUrlSegment("month", month.Serialize());
        return AssertOk(_restClient.Execute<IEnumerable<UsageDetails>>(restRequest));
    }

    public IEnumerable<object> DetailsForEventInMonth(long eventId, UsageMonth month)
    {
        var restRequest = new RestRequest("/reports/usage/month/{month}/event/{event}")
            .AddUrlSegment("month", month.Serialize())
            .AddUrlSegment("event", eventId.ToString());
        if (month.Before(2022, 12))
        {
            return AssertOk(_restClient.Execute<IEnumerable<UsageForObjectV1>>(restRequest));
        }
        else
        {
            return AssertOk(_restClient.Execute<IEnumerable<UsageForObjectV2>>(restRequest));
        }
    }
}