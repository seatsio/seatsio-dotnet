using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class CopyChartTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chart = Client.Charts.Create("my chart", "BOOTHS");

        var copiedChart = Client.Charts.Copy(chart.Key);

        Assert.Equal("my chart (copy)", copiedChart.Name);
        var drawing = Client.Charts.RetrievePublishedVersion(copiedChart.Key);
        Assert.Equal("BOOTHS", drawing.VenueType);
    }
}