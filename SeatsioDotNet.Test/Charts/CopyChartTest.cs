using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class CopyChartTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chart = Client.Charts.Create("my chart", "BOOTHS");

            var copiedChart = Client.Charts.Copy(chart.Key);

            Assert.Equal("my chart (copy)", copiedChart.Name);
        }
    }
}