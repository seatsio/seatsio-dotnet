using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SeatsioDotNet.ChartReports;
using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.Reports.Charts.ChartReports.Version;

namespace SeatsioDotNet.Test.Reports.Charts;

public class ChartReportsTest : SeatsioClientTest
{
    public static IEnumerable<object[]> ByLabelTestCases
    {
        get
        {
            yield return new object[] { (Action<SeatsioClient, string>)((_, _) => { }), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByLabel(chartKey))};
            yield return new object[] { (Action<SeatsioClient, string>)(CreateDraftChart), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByLabel(chartKey, version: Draft))};
        }
    }
        
    [Theory]
    [MemberData(nameof(ByLabelTestCases), MemberType = typeof(ChartReportsTest))]
    public void ReportItemProperties(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChart();
        updateChart(Client, chartKey);

        var report = getReport(Client, chartKey);

        var reportItem = report["A-1"].First();
        Assert.Equal("A-1", reportItem.Label);
        reportItem.Labels.Should().BeEquivalentTo(new Labels("1", "seat", "A", "row"));
        reportItem.IDs.Should().BeEquivalentTo(new IDs("1", "A", null));
        Assert.Equal("Cat1", reportItem.CategoryLabel);
        Assert.Equal(9, reportItem.CategoryKey);
        Assert.Equal("seat", reportItem.ObjectType);
        Assert.Null(reportItem.Section);
        Assert.Null(reportItem.Entrance);
        Assert.Null(reportItem.Capacity);
        Assert.Null(reportItem.LeftNeighbour);
        Assert.Equal("A-2", reportItem.RightNeighbour);
        Assert.Null(reportItem.BookAsAWhole);
        Assert.NotNull(reportItem.DistanceToFocalPoint);
        Assert.NotNull(reportItem.IsAccessible);
        Assert.NotNull(reportItem.IsCompanionSeat);
        Assert.NotNull(reportItem.HasRestrictedView);
    }

    [Theory]
    [MemberData(nameof(ByLabelTestCases), MemberType = typeof(ChartReportsTest))]
    public void ReportItemPropertiesForGA(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChart();
        updateChart(Client, chartKey);

        var report = getReport(Client, chartKey);

        var reportItem = report["GA1"].First();
        Assert.Equal(100, reportItem.Capacity);
        Assert.Equal("generalAdmission", reportItem.ObjectType);
        Assert.False(reportItem.BookAsAWhole);
    }     
        
    public static IEnumerable<object[]> ReportItemPropertiesForTableTestCases
    {
        get
        {
            yield return new object[] { (Action<SeatsioClient, string>)((_, _) => { }), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByLabel(chartKey, bookWholeTablesMode: "true"))};
            yield return new object[] { (Action<SeatsioClient, string>)(CreateDraftChart), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByLabel(chartKey, bookWholeTablesMode: "true", version: Draft))};
        }
    }
        
    [Theory]
    [MemberData(nameof(ReportItemPropertiesForTableTestCases), MemberType = typeof(ChartReportsTest))]
    public void ReportItemPropertiesForTable(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChartWithTables();
        updateChart(Client, chartKey);

        var report = getReport(Client, chartKey);

        var reportItem = report["T1"].First();
        Assert.Equal(6, reportItem.NumSeats);
        Assert.False(reportItem.BookAsAWhole);
    }

    [Theory]
    [MemberData(nameof(ByLabelTestCases), MemberType = typeof(ChartReportsTest))]
    public void ByLabel(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChart();
        updateChart(Client, chartKey);

        var report = getReport(Client, chartKey);

        Assert.Single(report["A-1"]);
        Assert.Single(report["A-2"]);
    }  
        
    public static IEnumerable<object[]> ByObjectTypeTestCases
    {
        get
        {
            yield return new object[] { (Action<SeatsioClient, string>)((_, _) => { }), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByObjectType(chartKey))};
            yield return new object[] { (Action<SeatsioClient, string>)(CreateDraftChart), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByObjectType(chartKey, version: Draft))};
        }
    }
        
    [Theory]
    [MemberData(nameof(ByObjectTypeTestCases), MemberType = typeof(ChartReportsTest))]
    public void ByObjectType(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChart();
        updateChart(Client, chartKey);

        var report = getReport(Client, chartKey);

        Assert.Equal(32, report["seat"].Count());
        Assert.Equal(2, report["generalAdmission"].Count());
        Assert.Empty(report["booth"]);
        Assert.Empty(report["table"]);
    }

    public static IEnumerable<object[]> ByCategoryKeyTestCases
    {
        get
        {
            yield return new object[] { (Action<SeatsioClient, string>)((_, _) => { }), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByCategoryKey(chartKey))};
            yield return new object[] { (Action<SeatsioClient, string>)(CreateDraftChart), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByCategoryKey(chartKey, version: Draft))};
        }
    }
        
    [Theory]
    [MemberData(nameof(ByCategoryKeyTestCases), MemberType = typeof(ChartReportsTest))]
    public void ByCategoryKey(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChart();
        updateChart(Client, chartKey);

        var report = getReport(Client, chartKey);
        Assert.Equal(4, report.Count);
        Assert.Equal(17, report["9"].Count());
        Assert.Equal(17, report["10"].Count());
        Assert.Empty(report["string11"]);
        Assert.Empty(report["NO_CATEGORY"]);
    }

    public static IEnumerable<object[]> ByCategoryLabelTestCases
    {
        get
        {
            yield return new object[] { (Action<SeatsioClient, string>)((_, _) => { }), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByCategoryLabel(chartKey))};
            yield return new object[] { (Action<SeatsioClient, string>)(CreateDraftChart), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByCategoryLabel(chartKey, version: Draft))};
        }
    }
        
    [Theory]
    [MemberData(nameof(ByCategoryLabelTestCases), MemberType = typeof(ChartReportsTest))]
    public void ByCategoryLabel(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChart();
        updateChart(Client, chartKey);

        var report = getReport(Client, chartKey);
        Assert.Equal(4, report.Count);
        Assert.Equal(17, report["Cat1"].Count());
        Assert.Equal(17, report["Cat2"].Count());
        Assert.Empty(report["Cat3"]);
        Assert.Empty(report["NO_CATEGORY"]);
    }  
        
    public static IEnumerable<object[]> BySectionTestCases
    {
        get
        {
            yield return new object[] { (Action<SeatsioClient, string>)((_, _) => { }), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.BySection(chartKey))};
            yield return new object[] { (Action<SeatsioClient, string>)(CreateDraftChart), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.BySection(chartKey, version: Draft))};
        }
    }
        
    [Theory]
    [MemberData(nameof(BySectionTestCases), MemberType = typeof(ChartReportsTest))]
    public void BySection(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChartWithSections();
        updateChart(Client, chartKey);

        var report = getReport(Client, chartKey);
        Assert.Equal(3, report.Count);
        Assert.Equal(36, report["Section A"].Count());
        Assert.Equal(35, report["Section B"].Count());
        Assert.Empty(report["NO_SECTION"]);
    }

    [Theory]
    [MemberData(nameof(ByLabelTestCases), MemberType = typeof(ChartReportsTest))]
    public void ByLabel_BookWholeTablesModeNull(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChartWithTables();
        updateChart(Client, chartKey);

        var report = getReport(Client, chartKey);

        CustomAssert.ContainsOnly(new [] {"T1-1", "T1-2", "T1-3", "T1-4", "T1-5", "T1-6", "T2-1", "T2-2", "T2-3", "T2-4", "T2-5", "T2-6", "T1", "T2"}, report.Keys);
    }

    public static IEnumerable<object[]> ByLabel_BookWholeTablesModeChartTestCases
    {
        get
        {
            yield return new object[] { (Action<SeatsioClient, string>)((_, _) => { }), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByLabel(chartKey, bookWholeTablesMode: "chart"))};
            yield return new object[] { (Action<SeatsioClient, string>)(CreateDraftChart), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByLabel(chartKey, bookWholeTablesMode: "chart", version: Draft))};
        }
    }
        
    [Theory]
    [MemberData(nameof(ByLabel_BookWholeTablesModeChartTestCases), MemberType = typeof(ChartReportsTest))]
    public void ByLabel_BookWholeTablesModeChart(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChartWithTables();
        updateChart(Client, chartKey);

        var report = getReport(Client, chartKey);

        CustomAssert.ContainsOnly(new [] {"T1-1", "T1-2", "T1-3", "T1-4", "T1-5", "T1-6", "T2"}, report.Keys);
    }

    public static IEnumerable<object[]> ByLabel_BookWholeTablesModeTrueTestCases
    {
        get
        {
            yield return new object[] { (Action<SeatsioClient, string>)((_, _) => { }), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByLabel(chartKey, bookWholeTablesMode: "true"))};
            yield return new object[] { (Action<SeatsioClient, string>)(CreateDraftChart), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByLabel(chartKey, bookWholeTablesMode: "true", version: Draft))};
        }
    }
        
    [Theory]
    [MemberData(nameof(ByLabel_BookWholeTablesModeTrueTestCases), MemberType = typeof(ChartReportsTest))]
    public void ByLabel_BookWholeTablesModeTrue(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChartWithTables();
        updateChart(Client, chartKey);

        var report = Client.ChartReports.ByLabel(chartKey, "true");

        CustomAssert.ContainsOnly(new [] {"T1", "T2"}, report.Keys);
    }

    public static IEnumerable<object[]> ByLabel_BookWholeTablesModeFalseTestCases
    {
        get
        {
            yield return new object[] { (Action<SeatsioClient, string>)((_, _) => { }), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByLabel(chartKey, bookWholeTablesMode: "false"))};
            yield return new object[] { (Action<SeatsioClient, string>)(CreateDraftChart), (Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>>)((client, chartKey) => client.ChartReports.ByLabel(chartKey, bookWholeTablesMode: "false", version: Draft))};
        }
    }
        
    [Theory]
    [MemberData(nameof(ByLabel_BookWholeTablesModeFalseTestCases), MemberType = typeof(ChartReportsTest))]
    public void ByLabel_BookWholeTablesModeFalse(Action<SeatsioClient, string> updateChart, Func<SeatsioClient, string, Dictionary<string, IEnumerable<ChartObjectInfo>>> getReport)
    {
        var chartKey = CreateTestChartWithTables();
        updateChart(Client, chartKey);

        var report = getReport(Client, chartKey);

        CustomAssert.ContainsOnly(new [] {"T1-1", "T1-2", "T1-3", "T1-4", "T1-5", "T1-6", "T2-1", "T2-2", "T2-3", "T2-4", "T2-5", "T2-6"}, report.Keys);
    }
        
    private static void CreateDraftChart(SeatsioClient client, string chartKey)
    {
        client.Events.Create(chartKey);
        client.Charts.Update(chartKey, "foo");
    }
}