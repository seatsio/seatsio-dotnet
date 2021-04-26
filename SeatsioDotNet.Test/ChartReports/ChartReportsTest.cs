using System.Linq;
using FluentAssertions;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.ChartReports
{
    public class ChartReportsTest : SeatsioClientTest
    {
        [Fact]
        public void ReportItemProperties()
        {
            var chartKey = CreateTestChart();

            var report = Client.ChartReports.ByLabel(chartKey, null);

            var reportItem = report["A-1"].First();
            Assert.Equal("A-1", reportItem.Label);
            reportItem.Labels.Should().BeEquivalentTo(new Labels("1", "seat", "A", "row"));
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
        }

        [Fact]
        public void ReportItemPropertiesForGA()
        {
            var chartKey = CreateTestChart();

            var report = Client.ChartReports.ByLabel(chartKey, null);

            var reportItem = report["GA1"].First();
            Assert.Equal(100, reportItem.Capacity);
            Assert.Equal("generalAdmission", reportItem.ObjectType);
            Assert.False(reportItem.BookAsAWhole);
        }

        [Fact]
        public void ByLabel()
        {
            var chartKey = CreateTestChart();

            var report = Client.ChartReports.ByLabel(chartKey, null);

            Assert.Single(report["A-1"]);
            Assert.Single(report["A-2"]);
        }  
        
        [Fact]
        public void ByObjectType()
        {
            var chartKey = CreateTestChart();

            var report = Client.ChartReports.ByObjectType(chartKey, null);

            Assert.Equal(32, report["seat"].Count());
            Assert.Equal(2, report["generalAdmission"].Count());
        }

        [Fact]
        public void ByCategoryKey()
        {
            var chartKey = CreateTestChart();
            var report = Client.ChartReports.ByCategoryKey(chartKey, null);
            Assert.Equal(2, report.Count);
            Assert.Equal(17, report["9"].Count());
            Assert.Equal(17, report["10"].Count());
        }

        [Fact]
        public void ByCategoryLabel()
        {
            var chartKey = CreateTestChart();
            var report = Client.ChartReports.ByCategoryLabel(chartKey, null);
            Assert.Equal(2, report.Count);
            Assert.Equal(17, report["Cat1"].Count());
            Assert.Equal(17, report["Cat2"].Count());
        }

        [Fact]
        public void ByLabel_BookWholeTablesModeNull()
        {
            var chartKey = CreateTestChartWithTables();

            var report = Client.ChartReports.ByLabel(chartKey, null);

            CustomAssert.ContainsOnly(new [] {"T1-1", "T1-2", "T1-3", "T1-4", "T1-5", "T1-6", "T2-1", "T2-2", "T2-3", "T2-4", "T2-5", "T2-6", "T1", "T2"}, report.Keys);
        }

        [Fact]
        public void ByLabel_BookWholeTablesModeChart()
        {
            var chartKey = CreateTestChartWithTables();

            var report = Client.ChartReports.ByLabel(chartKey, "chart");

            CustomAssert.ContainsOnly(new [] {"T1-1", "T1-2", "T1-3", "T1-4", "T1-5", "T1-6", "T2"}, report.Keys);
        }

        [Fact]
        public void ByLabel_BookWholeTablesModeTrue()
        {
            var chartKey = CreateTestChartWithTables();

            var report = Client.ChartReports.ByLabel(chartKey, "true");

            CustomAssert.ContainsOnly(new [] {"T1", "T2"}, report.Keys);
        }

        [Fact]
        public void ByLabel_BookWholeTablesModeFalse()
        {
            var chartKey = CreateTestChartWithTables();

            var report = Client.ChartReports.ByLabel(chartKey, "false");

            CustomAssert.ContainsOnly(new [] {"T1-1", "T1-2", "T1-3", "T1-4", "T1-5", "T1-6", "T2-1", "T2-2", "T2-3", "T2-4", "T2-5", "T2-6"}, report.Keys);
        }
    }
}
