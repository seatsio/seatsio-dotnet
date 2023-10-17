using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class RetrieveDraftVersionThumbnailTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chart = Client.Charts.Create();
        Client.Events.Create(chart.Key);
        Client.Charts.Update(chart.Key, "aChart");

        byte[] thumbnail = Client.Charts.RetrieveDraftVersionThumbnail(chart.Key);
        Assert.NotEmpty(thumbnail);
    }
}