using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class RemoveChannelTest : SeatsioClientTest
{
    [Fact]
    public async Task AddChannel()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());
        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2"});
        await Client.Events.Channels.AddAsync(event1.Key, "channelKey2", "channel 2", "#FFFF99", 2, new[] {"A-3"});

        await Client.Events.Channels.RemoveAsync(event1.Key, "channelKey2");

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        Assert.Equal(1, retrievedEvent.Channels.Count);

        var channel1 = retrievedEvent.Channels[0];
        Assert.Equal("channelKey1", channel1.Key);
    }
}