using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class PublishDraftVersionTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync("oldname");
        await Client.Events.CreateAsync(chart.Key);
        await Client.Charts.UpdateAsync(chart.Key, "newname");

        await Client.Charts.PublishDraftVersionAsync(chart.Key);

        var retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);
        Assert.Equal("newname", retrievedChart.Name);
        Assert.Equal("PUBLISHED", retrievedChart.Status);
    }
}