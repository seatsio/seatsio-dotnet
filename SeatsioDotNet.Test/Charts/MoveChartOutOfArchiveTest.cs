using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class MoveChartOutOfArchiveTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync();
        await Client.Charts.MoveToArchiveAsync(chart.Key);
            
        await Client.Charts.MoveOutOfArchiveAsync(chart.Key);

        Chart retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);
        Assert.False(retrievedChart.Archived);
    }
}