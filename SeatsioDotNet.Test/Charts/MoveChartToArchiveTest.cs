using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class MoveChartToArchiveTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync();

        await Client.Charts.MoveToArchiveAsync(chart.Key);

        Chart retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);
        Assert.True(retrievedChart.Archived);
    }
}