using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class AddObjectsToChannelTest : SeatsioClientTest
{
    [Fact]
    public async Task AddObjectsToChannel()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());
        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2"});
        await Client.Events.Channels.AddAsync(event1.Key, "channelKey2", "channel 2", "#FFFF99", 2, new[] {"A-3", "A-4"});

        await Client.Events.Channels.AddObjectsAsync(event1.Key, "channelKey1", new[] {"A-3", "A-4"});

        var retrievedChannels = (await Client.Events.RetrieveAsync(event1.Key)).Channels;
        Assert.Equal(new[] {"A-1", "A-2", "A-3", "A-4"}, retrievedChannels[0].Objects);
        Assert.Empty(retrievedChannels[1].Objects);
    }
}