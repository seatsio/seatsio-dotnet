using System.Collections.Generic;
using SeatsioDotNet.Events;
using Xunit;
using static SeatsioDotNet.EventReports.EventObjectInfo;

namespace SeatsioDotNet.Test.ChartReports
{
    public class ChartReportsSummaryTest : SeatsioClientTest
    {
        [Fact]
        public void SummaryByObjectType()
        {
            var chartKey = CreateTestChart();

            var report = Client.ChartReports.SummaryByObjectType(chartKey);

            Assert.Equal(32, report["seat"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 32}}, report["seat"].bySection);
            Assert.Equal(new Dictionary<string, int> {{"9", 16}, {"10", 16}}, report["seat"].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 16}, {"Cat2", 16}}, report["seat"].byCategoryLabel);

            Assert.Equal(200, report["generalAdmission"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 200}}, report["generalAdmission"].bySection);
            Assert.Equal(new Dictionary<string, int> {{"9", 100}, {"10", 100}},
                report["generalAdmission"].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 100}, {"Cat2", 100}},
                report["generalAdmission"].byCategoryLabel);
        }

        [Fact]
        public void SummaryByCategoryKey()
        {
            var chartKey = CreateTestChart();

            var report = Client.ChartReports.SummaryByCategoryKey(chartKey);

            Assert.Equal(116, report["9"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 116}}, report["9"].bySection);

            Assert.Equal(116, report["10"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 116}}, report["10"].bySection);

            Assert.Equal(0, report["NO_CATEGORY"].Count);
        }

        [Fact]
        public void SummaryByCategoryLabel()
        {
            var chartKey = CreateTestChart();

            var report = Client.ChartReports.SummaryByCategoryLabel(chartKey);

            Assert.Equal(116, report["Cat1"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 116}}, report["Cat1"].bySection);


            Assert.Equal(116, report["Cat2"].Count);
            Assert.Equal(new Dictionary<string, int> {{NoSection, 116}}, report["Cat2"].bySection);

            Assert.Equal(0, report["NO_CATEGORY"].Count);
        }

        [Fact]
        public void SummaryBySection()
        {
            var chartKey = CreateTestChart();

            var report = Client.ChartReports.SummaryBySection(chartKey);

            Assert.Equal(232, report[NoSection].Count);
            Assert.Equal(new Dictionary<string, int> {{"9", 116}, {"10", 116}}, report[NoSection].byCategoryKey);
            Assert.Equal(new Dictionary<string, int> {{"Cat1", 116}, {"Cat2", 116}}, report[NoSection].byCategoryLabel);
        }
    }
}