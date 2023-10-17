using System;
using System.Collections.Generic;
using SeatsioDotNet.Reports.Charts;
using Xunit;
using static SeatsioDotNet.Reports.Charts.ChartReports.Version;
using static SeatsioDotNet.EventReports.EventObjectInfo;

namespace SeatsioDotNet.Test.Reports.Charts
{
    public class ChartReportsSummaryTest : SeatsioClientTest
    {
        public static IEnumerable<object[]> SummaryByObjectTypeTestCases
        {
            get
            {
                yield return new object[]
                {
                    (Action<SeatsioClient, string>) ((_, _) => { }),
                    (Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>>) ((client, chartKey) =>
                        client.ChartReports.SummaryByObjectType(chartKey))
                };
                yield return new object[]
                {
                    (Action<SeatsioClient, string>) (CreateDraftChart),
                    (Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>>) ((client, chartKey) =>
                        client.ChartReports.SummaryByObjectType(chartKey, version: Draft))
                };
            }
        }

        [Theory]
        [MemberData(nameof(SummaryByObjectTypeTestCases), MemberType = typeof(ChartReportsSummaryTest))]
        public void SummaryByObjectType(Action<SeatsioClient, string> updateChart,
            Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>> getReport)
        {
            var chartKey = CreateTestChart();
            updateChart(Client, chartKey);

            var report = getReport(Client, chartKey);

            Assert.Equal(32, report["seat"].Count);
            Assert.Equal(new() {{NoSection, 32}}, report["seat"].bySection);
            Assert.Equal(new() {{"9", 16}, {"10", 16}}, report["seat"].byCategoryKey);
            Assert.Equal(new() {{"Cat1", 16}, {"Cat2", 16}}, report["seat"].byCategoryLabel);

            Assert.Equal(200, report["generalAdmission"].Count);
            Assert.Equal(new() {{NoSection, 200}}, report["generalAdmission"].bySection);
            Assert.Equal(new() {{"9", 100}, {"10", 100}},
                report["generalAdmission"].byCategoryKey);
            Assert.Equal(new() {{"Cat1", 100}, {"Cat2", 100}},
                report["generalAdmission"].byCategoryLabel);
        }

        public static IEnumerable<object[]> SummaryByObjectType_BookWholeTablesTrueTestCases
        {
            get
            {
                yield return new object[]
                {
                    (Action<SeatsioClient, string>) ((_, _) => { }),
                    (Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>>) ((client, chartKey) =>
                        client.ChartReports.SummaryByObjectType(chartKey, bookWholeTablesMode: "true"))
                };
                yield return new object[]
                {
                    (Action<SeatsioClient, string>) (CreateDraftChart),
                    (Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>>) ((client, chartKey) =>
                        client.ChartReports.SummaryByObjectType(chartKey, bookWholeTablesMode: "true", version: Draft))
                };
            }
        }

        [Theory]
        [MemberData(nameof(SummaryByObjectType_BookWholeTablesTrueTestCases),
            MemberType = typeof(ChartReportsSummaryTest))]
        public void SummaryByObjectType_BookWholeTablesTrue(Action<SeatsioClient, string> updateChart,
            Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>> getReport)
        {
            var chartKey = CreateTestChartWithTables();
            updateChart(Client, chartKey);

            var report = getReport(Client, chartKey);

            Assert.Equal(0, report["seat"].Count);
            Assert.Equal(2, report["table"].Count);
        }

        public static IEnumerable<object[]> SummaryByCategoryKeyTestCases
        {
            get
            {
                yield return new object[]
                {
                    (Action<SeatsioClient, string>) ((_, _) => { }),
                    (Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>>) ((client, chartKey) =>
                        client.ChartReports.SummaryByCategoryKey(chartKey))
                };
                yield return new object[]
                {
                    (Action<SeatsioClient, string>) (CreateDraftChart),
                    (Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>>) ((client, chartKey) =>
                        client.ChartReports.SummaryByCategoryKey(chartKey, version: Draft))
                };
            }
        }

        [Theory]
        [MemberData(nameof(SummaryByCategoryKeyTestCases), MemberType = typeof(ChartReportsSummaryTest))]
        public void SummaryByCategoryKey(Action<SeatsioClient, string> updateChart,
            Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>> getReport)
        {
            var chartKey = CreateTestChart();
            updateChart(Client, chartKey);

            var report = getReport(Client, chartKey);

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
                    (Action<SeatsioClient, string>) ((_, _) => { }),
                    (Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>>) ((client, chartKey) =>
                        client.ChartReports.SummaryByCategoryLabel(chartKey))
                };
                yield return new object[]
                {
                    (Action<SeatsioClient, string>) (CreateDraftChart),
                    (Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>>) ((client, chartKey) =>
                        client.ChartReports.SummaryByCategoryLabel(chartKey, version: Draft))
                };
            }
        }

        [Theory]
        [MemberData(nameof(SummaryByCategoryLabelTestCases), MemberType = typeof(ChartReportsSummaryTest))]
        public void SummaryByCategoryLabel(Action<SeatsioClient, string> updateChart,
            Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>> getReport)
        {
            var chartKey = CreateTestChart();
            updateChart(Client, chartKey);

            var report = getReport(Client, chartKey);

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
                    (Action<SeatsioClient, string>) ((_, _) => { }),
                    (Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>>) ((client, chartKey) =>
                        client.ChartReports.SummaryBySection(chartKey))
                };
                yield return new object[]
                {
                    (Action<SeatsioClient, string>) (CreateDraftChart),
                    (Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>>) ((client, chartKey) =>
                        client.ChartReports.SummaryBySection(chartKey, version: Draft))
                };
            }
        }

        [Theory]
        [MemberData(nameof(SummaryBySectionTestCases), MemberType = typeof(ChartReportsSummaryTest))]
        public void SummaryBySection(Action<SeatsioClient, string> updateChart,
            Func<SeatsioClient, string, Dictionary<string, ChartReportSummaryItem>> getReport)
        {
            var chartKey = CreateTestChart();
            updateChart(Client, chartKey);

            var report = getReport(Client, chartKey);

            Assert.Equal(232, report[NoSection].Count);
            Assert.Equal(new() {{"9", 116}, {"10", 116}}, report[NoSection].byCategoryKey);
            Assert.Equal(new() {{"Cat1", 116}, {"Cat2", 116}}, report[NoSection].byCategoryLabel);
        }

        private static void CreateDraftChart(SeatsioClient client, string chartKey)
        {
            client.Events.Create(chartKey);
            client.Charts.Update(chartKey, "foo");
        }
    }
}