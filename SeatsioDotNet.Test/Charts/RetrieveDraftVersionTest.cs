using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class RetrieveDraftVersionTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync();
        await Client.Events.CreateAsync(chart.Key);
        await Client.Charts.UpdateAsync(chart.Key, "aChart");

        var drawing = await Client.Charts.RetrieveDraftVersionAsync(chart.Key);
        Assert.Equal("aChart", drawing.Name);
    }

}