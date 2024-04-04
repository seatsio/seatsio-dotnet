using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class DiscardDraftTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync("oldname");
        await Client.Events.CreateAsync(chart.Key);
        await Client.Charts.UpdateAsync(chart.Key, "newname");

        await Client.Charts.DiscardDraftVersionAsync(chart.Key);

        var retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);
        Assert.Equal("oldname", retrievedChart.Name);
        Assert.Equal("PUBLISHED", retrievedChart.Status);
    }
}