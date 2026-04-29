using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class RemoveObjectsFromChannelTest : SeatsioClientTest
{
    [Fact]
    public async Task RemoveObjectsFromChannel()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());
        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2", "A-3", "A-4"});

        await Client.Events.Channels.RemoveObjectsAsync(event1.Key, "channelKey1", new[] {"A-3", "A-4"});

        var retrievedChannels = (await Client.Events.RetrieveAsync(event1.Key)).Channels;
        Assert.Equal(new[] {"A-1", "A-2"}, retrievedChannels[0].Objects);
    }

    [Fact]
    public async Task RemoveAreaPlacesFromChannel()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());
        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, null,
            new Dictionary<string, int> { { "GA1", 3 } });

        await Client.Events.Channels.RemoveObjectsAsync(event1.Key, "channelKey1",
            areaPlaces: new Dictionary<string, int> { { "GA1", 3 } });

        var retrievedChannels = (await Client.Events.RetrieveAsync(event1.Key)).Channels;
        Assert.Null(retrievedChannels[0].AreaPlaces);
    }
}