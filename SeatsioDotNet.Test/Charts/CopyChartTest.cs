using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class CopyChartTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync("my chart", "SIMPLE");

        var copiedChart = await Client.Charts.CopyAsync(chart.Key);

        Assert.Equal("my chart (copy)", copiedChart.Name);
        var drawing = await Client.Charts.RetrievePublishedVersionAsync(copiedChart.Key);
        Assert.Equal("SIMPLE", drawing.VenueType);
    }
}