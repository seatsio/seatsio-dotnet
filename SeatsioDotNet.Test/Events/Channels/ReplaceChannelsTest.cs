using System;
using System.Collections.Generic;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class ReplaceChannelsTest : SeatsioClientTest
    {
        [Fact]
        public void UpdateChannels()
        {
            var chartKey1 = CreateTestChart();
            var event1 = Client.Events.Create(chartKey1);

            var channels = new List<Channel>
            {
                new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"}),
                new("channelKey2", "channel 2", "#00FFFF", 2, new String[] { })
            };

            Client.Events.Channels.Replace(event1.Key, channels);

            var retrievedEvent = Client.Events.Retrieve(event1.Key);
            Assert.Equivalent(channels, retrievedEvent.Channels);
        }
    }
}