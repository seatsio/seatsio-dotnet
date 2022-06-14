using System;
using System.Collections.Generic;
using SeatsioDotNet.Events;
using Xunit;


namespace SeatsioDotNet.Test.Events
{
    public class RemoveChannelTest : SeatsioClientTest
    {
        [Fact]
        public void AddChannel()
        {
            var event1 = Client.Events.Create(CreateTestChart());
            Client.Events.Channels.Add(event1.Key, "channelKey1", "channel 1", "#FFFF98", 1, new[] {"A-1", "A-2"});
            Client.Events.Channels.Add(event1.Key, "channelKey2", "channel 2", "#FFFF99", 2, new[] {"A-3"});

            Client.Events.Channels.Remove(event1.Key, "channelKey2");

            var retrievedEvent = Client.Events.Retrieve(event1.Key);
            Assert.Equal(1, retrievedEvent.Channels.Count);

            var channel1 = retrievedEvent.Channels[0];
            Assert.Equal("channelKey1", channel1.Key);
        }
    }
}