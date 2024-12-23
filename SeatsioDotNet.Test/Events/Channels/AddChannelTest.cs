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

        var channel1 = retrievedEvent.Channels[0];
        Assert.Equal("channelKey1", channel1.Key);
        Assert.Equal("channel 1", channel1.Name);
        Assert.Equal("#FFFF98", channel1.Color);
        Assert.Equal(1, channel1.Index);
        Assert.Equal(new[] {"A-1", "A-2"}, channel1.Objects);


        var channel2 = retrievedEvent.Channels[1];
        Assert.Equal("channelKey2", channel2.Key);
        Assert.Equal("channel 2", channel2.Name);
        Assert.Equal("#FFFF99", channel2.Color);
        Assert.Equal(2, channel2.Index);
        Assert.Equal(new[] {"A-3"}, channel2.Objects);
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

        var channel1 = retrievedEvent.Channels[0];
        Assert.Equal("channelKey1", channel1.Key);
        Assert.Equal("channel 1", channel1.Name);
        Assert.Equal("#FFFF98", channel1.Color);
        Assert.Equal(1, channel1.Index);
        Assert.Equal(new[] {"A-1", "A-2"}, channel1.Objects);


        var channel2 = retrievedEvent.Channels[1];
        Assert.Equal("channelKey2", channel2.Key);
        Assert.Equal("channel 2", channel2.Name);
        Assert.Equal("#FFFF99", channel2.Color);
        Assert.Equal(2, channel2.Index);
        Assert.Equal(new[] {"A-3"}, channel2.Objects);
    }

    [Fact]
    public async Task IndexIsOptional()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());

        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", null, new[] {"A-1", "A-2"});

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        Assert.Single(retrievedEvent.Channels);

        var channel1 = retrievedEvent.Channels[0];
        Assert.Equal("channelKey1", channel1.Key);
        Assert.Equal("channel 1", channel1.Name);
        Assert.Equal("#FFFF98", channel1.Color);
        Assert.Equal(0, channel1.Index);
        Assert.Equal(new[] {"A-1", "A-2"}, channel1.Objects);
    }

    [Fact]
    public async Task ObjectsAreOptional()
    {
        var event1 = await Client.Events.CreateAsync(CreateTestChart());

        await Client.Events.Channels.AddAsync(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, null);

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        Assert.Single(retrievedEvent.Channels);

        var channel1 = retrievedEvent.Channels[0];
        Assert.Equal("channelKey1", channel1.Key);
        Assert.Equal("channel 1", channel1.Name);
        Assert.Equal("#FFFF98", channel1.Color);
        Assert.Equal(1, channel1.Index);
        Assert.Empty(channel1.Objects);
    }
}
