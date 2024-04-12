using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class CopyDraftVersionTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync();
        await Client.Events.CreateAsync(chart.Key);
        await Client.Charts.UpdateAsync(chart.Key, "newname");
            
        var copiedChart = await Client.Charts.CopyDraftVersionAsync(chart.Key);
            
        Assert.Equal("newname (copy)", copiedChart.Name);
    }
}