using System;
using System.Collections.Generic;
using SeatsioDotNet.Events;
using Xunit;


namespace SeatsioDotNet.Test.Events
{
    public class AddObjectsToChannelTest : SeatsioClientTest
    {
        [Fact]
        public void AddObjectsToChannel()
        {
            var event1 = Client.Events.Create(CreateTestChart());
            Client.Events.Channels.Add(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2"});
            Client.Events.Channels.Add(event1.Key, "channelKey2", "channel 2", "#FFFF99", 2, new[] {"A-3", "A-4"});

            Client.Events.Channels.AddObjects(event1.Key, "channelKey1", new[] {"A-3", "A-4"});
            
            var retrievedChannels = Client.Events.Retrieve(event1.Key).Channels;
            Assert.Equal(new[] {"A-1", "A-2", "A-3", "A-4"}, retrievedChannels[0].Objects);
            Assert.Empty(retrievedChannels[1].Objects);
        }
    }
}