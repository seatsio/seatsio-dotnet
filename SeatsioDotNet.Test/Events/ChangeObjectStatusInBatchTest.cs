using System.Collections.Generic;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class ChangeObjectStatusInBatchTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey1 = CreateTestChart();
            var chartKey2 = CreateTestChart();
            var evnt1 = Client.Events.Create(chartKey1);
            var evnt2 = Client.Events.Create(chartKey2);

            var result = Client.Events.ChangeObjectStatus(new[]
            {
                new StatusChangeRequest(evnt1.Key, new[] {"A-1"}, "lolzor"),
                new StatusChangeRequest(evnt2.Key, new[] {"A-2"}, "lolzor")
            });

            Assert.Equal("lolzor", result[0].Objects["A-1"].Status);
            Assert.Equal("lolzor", Client.Events.RetrieveObjectInfo(evnt1.Key, "A-1").Status);

            Assert.Equal("lolzor", result[1].Objects["A-2"].Status);
            Assert.Equal("lolzor", Client.Events.RetrieveObjectInfo(evnt2.Key, "A-2").Status);
        }

        [Fact]
        public void ChannelKeys()
        {
            var chartKey = CreateTestChart();
            var channels = new List<Channel>
            {
                new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1"})
            };
            var evnt = Client.Events.Create(chartKey, new CreateEventParams().WithChannels(channels));

            var result = Client.Events.ChangeObjectStatus(new[]
            {
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "lolzor", channelKeys: new[] {"channelKey1"}),
            });

            Assert.Equal("lolzor", result[0].Objects["A-1"].Status);
        }

        [Fact]
        public void IgnoreChannels()
        {
            var chartKey = CreateTestChart();
            var channels = new List<Channel>
            {
                new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1"})
            };
            var evnt = Client.Events.Create(chartKey, new CreateEventParams().WithChannels(channels));
            
            var result = Client.Events.ChangeObjectStatus(new[]
            {
                new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "lolzor", ignoreChannels: true),
            });

            Assert.Equal("lolzor", result[0].Objects["A-1"].Status);
        }
        
        [Fact]
        public void AllowedPreviousStatuses()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            
            Assert.Throws<SeatsioException>(() =>
            {
                Client.Events.ChangeObjectStatus(new[]
                {
                    new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "lolzor", ignoreChannels: true, allowedPreviousStatuses:new []{"someOtherStatus"}),
                });                
            });
            
        }
        
        [Fact]
        public void RejectedPreviousStatuses()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            
            Assert.Throws<SeatsioException>(() =>
            {
                Client.Events.ChangeObjectStatus(new[]
                {
                    new StatusChangeRequest(evnt.Key, new[] {"A-1"}, "lolzor", ignoreChannels: true, rejectedPreviousStatuses:new []{"free"}),
                });                
            });
        }
    }
}