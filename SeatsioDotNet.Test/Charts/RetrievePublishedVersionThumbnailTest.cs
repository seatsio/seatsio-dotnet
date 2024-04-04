using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class RetrievePublishedVersionThumbnailTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync();

        byte[] thumbnail = await Client.Charts.RetrievePublishedVersionThumbnailAsync(chart.Key);
        Assert.NotEmpty(thumbnail);
    }
}