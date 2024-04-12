using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class RetrieveDraftVersionThumbnailTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync();
        await Client.Events.CreateAsync(chart.Key);
        await Client.Charts.UpdateAsync(chart.Key, "aChart");

        byte[] thumbnail = await Client.Charts.RetrieveDraftVersionThumbnailAsync(chart.Key);
        Assert.NotEmpty(thumbnail);
    }
}