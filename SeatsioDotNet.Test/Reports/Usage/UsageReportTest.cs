using System;
using System.Linq;
using System.Threading.Tasks;
using SeatsioDotNet.Reports.Usage;
using SeatsioDotNet.Reports.Usage.DetailsForEventInMonth;
using Xunit;
using Xunit.Abstractions;

namespace SeatsioDotNet.Test.Reports.Usage;

public class UsageReportTest
{
    private readonly ITestOutputHelper TestOutputHelper;

    public UsageReportTest(ITestOutputHelper testOutputHelper)
    {
        TestOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestUsageReportForAllMonths()
    {
        if (!IsConfigured())
        {
            warnAboutNotConfigured();
            return;
        }

        var client = UsageReportingClient();

        var report = await client.UsageReports.SummaryForAllMonthsAsync();

        Assert.True(report.UsageCutoffDate.Year > 2000);
        Assert.True(report.Usage.Any());
        Assert.Equal(2, report.Usage.ElementAt(0).Month.Month);
        Assert.Equal(2014, report.Usage.ElementAt(0).Month.Year);
    }

    [Fact]
    public async Task TestUsageReportForMonth()
    {
        if (!IsConfigured())
        {
            warnAboutNotConfigured();
            return;
        }

        var client = UsageReportingClient();

        var report = await client.UsageReports.DetailsForMonthAsync(new UsageMonth(2021, 11));

        Assert.True(report.Any());
        Assert.True(report.ElementAt(0).UsageByChart.Any());
        Assert.Equal(143, report.ElementAt(0).UsageByChart.ElementAt(0).UsageByEvent.ElementAt(0).NumUsedObjects);
    }

    [Fact]
    public async Task TestUsageReportForEventInMonth()
    {
        if (!IsConfigured())
        {
            warnAboutNotConfigured();
            return;
        }

        var client = UsageReportingClient();

        var report = await client.UsageReports.DetailsForEventInMonthAsync(580293, new UsageMonth(2021, 11));

        Assert.True(report.Any());
        Assert.Equal(1, ((UsageForObjectV1) report.ElementAt(0)).NumFirstSelections);
    }

    private static SeatsioClient UsageReportingClient()
    {
        return new SeatsioClient(SecretKey(), null, ApiUrl());
    }

    private static string ApiUrl()
    {
        return Environment.GetEnvironmentVariable("USAGE_REPORTING_TESTS_API_URL");
    }

    private static string SecretKey()
    {
        return Environment.GetEnvironmentVariable("USAGE_REPORTING_TESTS_SECRET_KEY");
    }

    private static bool IsConfigured()
    {
        return !string.IsNullOrWhiteSpace(ApiUrl()) && !string.IsNullOrWhiteSpace(SecretKey());
    }

    private void warnAboutNotConfigured()
    {
        TestOutputHelper.WriteLine(
            "USAGE_REPORTING_TESTS_API_URL and/or USAGE_REPORTING_TESTS_SECRET_KEY environment variables not set. Skipping test.");
    }
}
