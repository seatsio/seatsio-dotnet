using Xunit;

namespace SeatsioDotNet.Test.Events;

public class RemoveObjectsFromChannelTest : SeatsioClientTest
{
    [Fact]
    public void RemoveObjectsFromChannel()
    {
        var event1 = Client.Events.Create(CreateTestChart());
        Client.Events.Channels.Add(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2", "A-3", "A-4"});

        Client.Events.Channels.RemoveObjects(event1.Key, "channelKey1", new[] {"A-3", "A-4"});
            
        var retrievedChannels = Client.Events.Retrieve(event1.Key).Channels;
        Assert.Equal(new[] {"A-1", "A-2"}, retrievedChannels[0].Objects);
    }
}