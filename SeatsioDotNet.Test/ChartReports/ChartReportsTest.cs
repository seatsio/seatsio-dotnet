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

            var report = Client.ChartReports.ByLabel(chartKey);

            var reportItem = report["A-1"].First();
            Assert.Equal("A-1", reportItem.Label);
            reportItem.Labels.Should().BeEquivalentTo(new Labels("1", "seat", "A", "row"));
            Assert.Equal("Cat1", reportItem.CategoryLabel);
            Assert.Equal(9, reportItem.CategoryKey);
            Assert.Equal("seat", reportItem.ObjectType);
            Assert.Null(reportItem.Section);
            Assert.Null(reportItem.Entrance);
            Assert.Null(reportItem.Capacity);
        }

        [Fact]
        public void ReportItemPropertiesForGA()
        {
            var chartKey = CreateTestChart();
            
            var report = Client.ChartReports.ByLabel(chartKey);

            var reportItem = report["GA1"].First();
            Assert.Equal(100, reportItem.Capacity);
            Assert.Equal("generalAdmission", reportItem.ObjectType);
        }

        [Fact]
        public void ByLabel()
        {
            var chartKey = CreateTestChart();            

            var report = Client.ChartReports.ByLabel(chartKey);

            Assert.Single(report["A-1"]);
            Assert.Single(report["A-2"]);
        }
    }
}