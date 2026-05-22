using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class UpdateChannelTest : SeatsioClientTest
{
    [Fact]
    public async Task UpdateName()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());
        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2"});

        await Client.Events.Channels.UpdateAsync(event1.Key, "channelKey1", "new channel name", null, null);

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        Assert.Equal(1, retrievedEvent.Channels.Count);
        Assert.Equivalent(new Channel("channelKey1", retrievedEvent.Channels[0].Id, "new channel name", "#FFFF98", 1, new[] {"A-1", "A-2"}, new Dictionary<string, int>()), retrievedEvent.Channels[0]);
    }

    [Fact]
    public async Task UpdateColor()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());
        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2"});

        await Client.Events.Channels.UpdateAsync(event1.Key, "channelKey1", null, "red", null);

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        Assert.Equal(1, retrievedEvent.Channels.Count);
        Assert.Equivalent(new Channel("channelKey1", retrievedEvent.Channels[0].Id, "channel 1", "red", 1, new[] {"A-1", "A-2"}, new Dictionary<string, int>()), retrievedEvent.Channels[0]);
    }

    [Fact]
    public async Task UpdateObjects()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());
        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2"});

        await Client.Events.Channels.UpdateAsync(event1.Key, "channelKey1", null, null, new[] {"B-1"});

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        Assert.Equal(1, retrievedEvent.Channels.Count);
        Assert.Equivalent(new Channel("channelKey1", retrievedEvent.Channels[0].Id, "channel 1", "#FFFF98", 1, new[] {"B-1"}, new Dictionary<string, int>()), retrievedEvent.Channels[0]);
    }

    [Fact]
    public async Task UpdateAreaPlaces()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());
        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, null,
            new Dictionary<string, int> { { "GA1", 3 } });

        await Client.Events.Channels.UpdateAsync(event1.Key, "channelKey1", null, null, null,
            new Dictionary<string, int> { { "GA1", 5 } });

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        var channel1 = retrievedEvent.Channels[0];
        Assert.Equal(new Dictionary<string, int> { { "GA1", 5 } }, channel1.AreaPlaces);
    }
}