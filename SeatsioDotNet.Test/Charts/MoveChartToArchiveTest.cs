using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class MoveChartToArchiveTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chart = Client.Charts.Create();

        Client.Charts.MoveToArchive(chart.Key);

        Chart retrievedChart = Client.Charts.Retrieve(chart.Key);
        Assert.True(retrievedChart.Archived);
    }
}