using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class CopyChartTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync("my chart", "BOOTHS");

        var copiedChart = await Client.Charts.CopyAsync(chart.Key);

        Assert.Equal("my chart (copy)", copiedChart.Name);
        var drawing = await Client.Charts.RetrievePublishedVersionAsync(copiedChart.Key);
        Assert.Equal("BOOTHS", drawing.VenueType);
    }
}