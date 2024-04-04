using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class ReplaceChannelsTest : SeatsioClientTest
{
    [Fact]
    public async Task UpdateChannels()
    {
        var chartKey1 = CreateTestChart();
        var event1 = await Client.Events.CreateAsync(chartKey1);

        var channels = new List<Channel>
        {
            new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"}),
            new("channelKey2", "channel 2", "#00FFFF", 2, new String[] { })
        };

        await Client.Events.Channels.ReplaceAsync(event1.Key, channels);

        var retrievedEvent = await Client.Events.RetrieveAsync(event1.Key);
        Assert.Equivalent(channels, retrievedEvent.Channels);
    }
}