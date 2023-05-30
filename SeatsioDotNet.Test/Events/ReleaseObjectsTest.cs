using System.Collections.Generic;
using FluentAssertions;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Events;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class ReleaseObjectsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"});

            var result = Client.Events.Release(evnt.Key, new[] {"A-1", "A-2"});

            Assert.Equal(EventObjectInfo.Free, Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").Status);
            Assert.Equal(EventObjectInfo.Free, Client.Events.RetrieveObjectInfo(evnt.Key, "A-2").Status);
            CustomAssert.ContainsOnly(new[] {"A-1", "A-2"}, result.Objects.Keys);
        }

        [Fact]
        public void HoldToken()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            HoldToken holdToken = Client.HoldTokens.Create();
            Client.Events.Hold(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

            Client.Events.Release(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

            var objectInfo1 = Client.Events.RetrieveObjectInfo(evnt.Key, "A-1");
            Assert.Equal(EventObjectInfo.Free, objectInfo1.Status);
            Assert.Null(objectInfo1.HoldToken);

            var objectInfo2 = Client.Events.RetrieveObjectInfo(evnt.Key, "A-2");
            Assert.Equal(EventObjectInfo.Free, objectInfo2.Status);
            Assert.Null(objectInfo2.HoldToken);
        }

        [Fact]
        public void OrderId()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            Client.Events.Release(evnt.Key, new[] {"A-1", "A-2"}, null, "order1");

            Assert.Equal("order1", Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").OrderId);
            Assert.Equal("order1", Client.Events.RetrieveObjectInfo(evnt.Key, "A-2").OrderId);
        }

        [Fact]
        public void KeepExtraData()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.Book(evnt.Key, new[] {new ObjectProperties("A-1", extraData)});

            Client.Events.Release(evnt.Key, new[] {"A-1"}, null, null, true);

            Assert.Equal(extraData, Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").ExtraData);
        }

        [Fact]
        public void ChannelKeys()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Channels.Replace(evnt.Key, new List<Channel>
            {
                new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"})
            });

            Client.Events.Book(evnt.Key, new[] {"A-1"}, null, null, true, null, new[] {"channelKey1"});

            Client.Events.Release(evnt.Key, new[] {"A-1"}, null, null, true, null, new[] {"channelKey1"});

            Assert.Equal(EventObjectInfo.Free, Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").Status);
        }    
        
        [Fact]
        public void IgnoreChannels()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Client.Events.Channels.Replace(evnt.Key, new List<Channel>
            {
                new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"})
            });

            Client.Events.Book(evnt.Key, new[] {"A-1"}, null, null, true, null, new[] {"channelKey1"});

            Client.Events.Release(evnt.Key, new[] {"A-1"}, null, null, true, true);

            Assert.Equal(EventObjectInfo.Free, Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").Status);
        }
    }
}
