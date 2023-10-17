using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class MoveChartOutOfArchiveTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chart = Client.Charts.Create();
        Client.Charts.MoveToArchive(chart.Key);
            
        Client.Charts.MoveOutOfArchive(chart.Key);

        Chart retrievedChart = Client.Charts.Retrieve(chart.Key);
        Assert.False(retrievedChart.Archived);
    }
}