using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class AddChannelTest : SeatsioClientTest
{
    [Fact]
    public async Task AddChannel()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());

        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2"});
        await Client.Events.Channels.AddAsync(event1.Key, "channelKey2", "channel 2", "#FFFF99", 2, new[] {"A-3"});

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        Assert.Equal(2, retrievedEvent.Channels.Count);
        Assert.Equivalent(new Channel("channelKey1", retrievedEvent.Channels[0].Id, "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2"}, new Dictionary<string, int>()), retrievedEvent.Channels[0]);
        Assert.Equivalent(new Channel("channelKey2", retrievedEvent.Channels[1].Id, "channel 2", "#FFFF99", 2, new[] {"A-3"}, new Dictionary<string, int>()), retrievedEvent.Channels[1]);
    }

    [Fact]
    public async Task AddChannels()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());

        await Client.Events.Channels.AddAsync(
            event1.Key,
            new[]
            {
                new ChannelCreationParams("channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2"}),
                new ChannelCreationParams("channelKey2", "channel 2", "#FFFF99", 2, new[] {"A-3"}),
            }
        );

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        Assert.Equal(2, retrievedEvent.Channels.Count);
        Assert.Equivalent(new Channel("channelKey1", retrievedEvent.Channels[0].Id, "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2"}, new Dictionary<string, int>()), retrievedEvent.Channels[0]);
        Assert.Equivalent(new Channel("channelKey2", retrievedEvent.Channels[1].Id, "channel 2", "#FFFF99", 2, new[] {"A-3"}, new Dictionary<string, int>()), retrievedEvent.Channels[1]);
    }

    [Fact]
    public async Task IndexIsOptional()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());

        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", null, new[] {"A-1", "A-2"});

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        Assert.Single(retrievedEvent.Channels);
        Assert.Equivalent(new Channel("channelKey1", retrievedEvent.Channels[0].Id, "channel 1", "#FFFF98", 0, new[] {"A-1", "A-2"}, new Dictionary<string, int>()), retrievedEvent.Channels[0]);
    }

    [Fact]
    public async Task ObjectsAreOptional()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());

        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, null);

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        Assert.Single(retrievedEvent.Channels);
        Assert.Equivalent(new Channel("channelKey1", retrievedEvent.Channels[0].Id, "channel 1", "#FFFF98", 1, new string[] { }, new Dictionary<string, int>()), retrievedEvent.Channels[0]);
    }

    [Fact]
    public async Task AddChannelWithAreaPlaces()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());

        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, null,
            new Dictionary<string, int> { { "GA1", 3 } });

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        var channel1 = retrievedEvent.Channels[0];
        Assert.Equal(new Dictionary<string, int> { { "GA1", 3 } }, channel1.AreaPlaces);
    }

    [Fact]
    public async Task AddChannelsWithAreaPlaces()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());

        await Client.Events.Channels.AddAsync(
            event1.Key,
            new[]
            {
                new ChannelCreationParams("channelKey1", "channel 1", "#FFFF98", 1, null,
                    new Dictionary<string, int> { { "GA1", 3 } }),
            }
        );

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        var channel1 = retrievedEvent.Channels[0];
        Assert.Equal(new Dictionary<string, int> { { "GA1", 3 } }, channel1.AreaPlaces);
    }

    [Fact]
    public async Task ChannelIdIsReceivedFromServer()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());

        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1"});

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        var channel1 = retrievedEvent.Channels[0];
        Assert.NotNull(channel1.Id);
        Assert.NotEmpty(channel1.Id);
    }
}
