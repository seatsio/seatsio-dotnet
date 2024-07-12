using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.Reports.Charts;
using Xunit;
using static SeatsioDotNet.Reports.Charts.ChartReports.Version;
using static SeatsioDotNet.EventReports.EventObjectInfo;

namespace SeatsioDotNet.Test.Reports.Charts;

public class ChartReportsSummaryTest : SeatsioClientTest
{
    public static IEnumerable<object[]> SummaryByObjectTypeTestCases
    {
        get
        {
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) ((_, _) => Task.CompletedTask),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryByObjectTypeAsync(chartKey))
            };
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) (CreateDraftChart),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryByObjectTypeAsync(chartKey, version: Draft))
            };
        }
    }

    [Theory]
    [MemberData(nameof(SummaryByObjectTypeTestCases), MemberType = typeof(ChartReportsSummaryTest))]
    public async Task SummaryByObjectType(Func<SeatsioClient, string, Task> updateChart,
        Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>> getReport)
    {
        var chartKey = CreateTestChart();
        await updateChart(Client, chartKey);

        var report = await getReport(Client, chartKey);

        Assert.Equal(32, report["seat"].Count);
        Assert.Equal(new() {{NoSection, 32}}, report["seat"].bySection);
        Assert.Equal(new() {{"9", 16}, {"10", 16}}, report["seat"].byCategoryKey);
        Assert.Equal(new() {{"Cat1", 16}, {"Cat2", 16}}, report["seat"].byCategoryLabel);
        Assert.Equal(new() {{NoZone, 32}}, report["seat"].byZone);

        Assert.Equal(200, report["generalAdmission"].Count);
        Assert.Equal(new() {{NoSection, 200}}, report["generalAdmission"].bySection);
        Assert.Equal(new() {{"9", 100}, {"10", 100}},
            report["generalAdmission"].byCategoryKey);
        Assert.Equal(new() {{"Cat1", 100}, {"Cat2", 100}},
            report["generalAdmission"].byCategoryLabel);
        Assert.Equal(new() {{NoZone, 200}}, report["generalAdmission"].byZone);
    }

    public static IEnumerable<object[]> SummaryByObjectType_BookWholeTablesTrueTestCases
    {
        get
        {
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) ((_, _) => Task.CompletedTask),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryByObjectTypeAsync(chartKey, bookWholeTablesMode: "true"))
            };
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) (CreateDraftChart),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryByObjectTypeAsync(chartKey, bookWholeTablesMode: "true", version: Draft))
            };
        }
    }

    [Theory]
    [MemberData(nameof(SummaryByObjectType_BookWholeTablesTrueTestCases),
        MemberType = typeof(ChartReportsSummaryTest))]
    public async Task SummaryByObjectType_BookWholeTablesTrue(Func<SeatsioClient, string, Task> updateChart,
        Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>> getReport)
    {
        var chartKey = CreateTestChartWithTables();
        await updateChart(Client, chartKey);

        var report = await getReport(Client, chartKey);

        Assert.Equal(0, report["seat"].Count);
        Assert.Equal(2, report["table"].Count);
    }

    public static IEnumerable<object[]> SummaryByCategoryKeyTestCases
    {
        get
        {
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) ((_, _) => Task.CompletedTask),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryByCategoryKeyAsync(chartKey))
            };
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) (CreateDraftChart),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryByCategoryKeyAsync(chartKey, version: Draft))
            };
        }
    }

    [Theory]
    [MemberData(nameof(SummaryByCategoryKeyTestCases), MemberType = typeof(ChartReportsSummaryTest))]
    public async Task SummaryByCategoryKey(Func<SeatsioClient, string, Task> updateChart,
        Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>> getReport)
    {
        var chartKey = CreateTestChart();
        await updateChart(Client, chartKey);

        var report = await getReport(Client, chartKey);

        Assert.Equal(116, report["9"].Count);
        Assert.Equal(new() {{NoSection, 116}}, report["9"].bySection);

        Assert.Equal(116, report["10"].Count);
        Assert.Equal(new() {{NoSection, 116}}, report["10"].bySection);

        Assert.Equal(0, report["NO_CATEGORY"].Count);
    }

    public static IEnumerable<object[]> SummaryByCategoryLabelTestCases
    {
        get
        {
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) ((_, _) => Task.CompletedTask),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryByCategoryLabelAsync(chartKey))
            };
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) (CreateDraftChart),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryByCategoryLabelAsync(chartKey, version: Draft))
            };
        }
    }

    [Theory]
    [MemberData(nameof(SummaryByCategoryLabelTestCases), MemberType = typeof(ChartReportsSummaryTest))]
    public async Task SummaryByCategoryLabel(Func<SeatsioClient, string, Task> updateChart,
        Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>> getReport)
    {
        var chartKey = CreateTestChart();
        await updateChart(Client, chartKey);

        var report = await getReport(Client, chartKey);

        Assert.Equal(116, report["Cat1"].Count);
        Assert.Equal(new() {{NoSection, 116}}, report["Cat1"].bySection);


        Assert.Equal(116, report["Cat2"].Count);
        Assert.Equal(new() {{NoSection, 116}}, report["Cat2"].bySection);

        Assert.Equal(0, report["NO_CATEGORY"].Count);
    }

    public static IEnumerable<object[]> SummaryBySectionTestCases
    {
        get
        {
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) ((_, _) => Task.CompletedTask),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryBySectionAsync(chartKey))
            };
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) (CreateDraftChart),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryBySectionAsync(chartKey, version: Draft))
            };
        }
    }

    [Theory]
    [MemberData(nameof(SummaryBySectionTestCases), MemberType = typeof(ChartReportsSummaryTest))]
    public async Task SummaryBySection(Func<SeatsioClient, string, Task> updateChart,
        Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>> getReport)
    {
        var chartKey = CreateTestChart();
        await updateChart(Client, chartKey);

        var report = await getReport(Client, chartKey);

        Assert.Equal(232, report[NoSection].Count);
        Assert.Equal(new() {{"9", 116}, {"10", 116}}, report[NoSection].byCategoryKey);
        Assert.Equal(new() {{"Cat1", 116}, {"Cat2", 116}}, report[NoSection].byCategoryLabel);
    }

    public static IEnumerable<object[]> SummaryByZoneTestCases
    {
        get
        {
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) ((_, _) => Task.CompletedTask),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryByZoneAsync(chartKey))
            };
            yield return new object[]
            {
                (Func<SeatsioClient, string, Task>) (CreateDraftChart),
                (Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>>) ((client, chartKey) =>
                    client.ChartReports.SummaryByZoneAsync(chartKey, version: Draft))
            };
        }
    }

    [Theory]
    [MemberData(nameof(SummaryByZoneTestCases), MemberType = typeof(ChartReportsSummaryTest))]
    public async Task SummaryByZone(Func<SeatsioClient, string, Task> updateChart,
        Func<SeatsioClient, string, Task<Dictionary<string, ChartReportSummaryItem>>> getReport)
    {
        var chartKey = CreateTestChartWithZones();
        await updateChart(Client, chartKey);

        var report = await getReport(Client, chartKey);

        Assert.Equal(6032, report["midtrack"].Count);
        Assert.Equal(new() {{"2", 6032}}, report["midtrack"].byCategoryKey);
        Assert.Equal(new() {{"Mid Track Stand", 6032}}, report["midtrack"].byCategoryLabel);
        Assert.Equal(new() {{"seat", 6032}}, report["midtrack"].byObjectType);
        Assert.Equal(new() {{"MT1", 2418}, {"MT3", 3614}}, report["midtrack"].bySection);
    }

    private static async Task CreateDraftChart(SeatsioClient client, string chartKey)
    {
        await client.Events.CreateAsync(chartKey);
        await client.Charts.UpdateAsync(chartKey, "foo");
    }
}