using System.Collections.Generic;
using FluentAssertions;
using SeatsioDotNet.Charts;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Events;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class BookObjectsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var result = Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"});

            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").Status);
            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(evnt.Key, "A-2").Status);
            Assert.Equal(EventObjectInfo.Free, Client.Events.RetrieveObjectInfo(evnt.Key, "A-3").Status);
            CustomAssert.ContainsOnly(new[] {"A-1", "A-2"}, result.Objects.Keys);
        }

        [Fact]
        public void Sections()
        {
            var chartKey = CreateTestChartWithSections();
            var evnt = Client.Events.Create(chartKey);

            var result = Client.Events.Book(evnt.Key, new[] {"Section A-A-1", "Section A-A-2"});

            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(evnt.Key, "Section A-A-1").Status);
            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(evnt.Key, "Section A-A-2").Status);
            Assert.Equal(EventObjectInfo.Free, Client.Events.RetrieveObjectInfo(evnt.Key, "Section A-A-3").Status);

            var reportItem = result.Objects["Section A-A-1"];
            Assert.Equal("Section A", reportItem.Section);
            Assert.Equal("Entrance 1", reportItem.Entrance);
            reportItem.Labels.Should().BeEquivalentTo(new Labels("1", "seat", "A", "row", "Section A"));
            reportItem.IDs.Should().BeEquivalentTo(new IDs("1", "A", "Section A"));
        }

        [Fact]
        public void HoldToken()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            HoldToken holdToken = Client.HoldTokens.Create();
            Client.Events.Hold(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

            var objectInfo1 = Client.Events.RetrieveObjectInfo(evnt.Key, "A-1");
            Assert.Equal(EventObjectInfo.Booked, objectInfo1.Status);
            Assert.Null(objectInfo1.HoldToken);

            var objectInfo2 = Client.Events.RetrieveObjectInfo(evnt.Key, "A-2");
            Assert.Equal(EventObjectInfo.Booked, objectInfo2.Status);
            Assert.Null(objectInfo2.HoldToken);
        }

        [Fact]
        public void OrderId()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"}, null, "order1");

            Assert.Equal("order1", Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").OrderId);
            Assert.Equal("order1", Client.Events.RetrieveObjectInfo(evnt.Key, "A-2").OrderId);
        }

        [Fact]
        public void KeepExtraData()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.UpdateExtraData(evnt.Key, "A-1", extraData);

            Client.Events.Book(evnt.Key, new[] {"A-1"}, null, null, true);

            var loadedExtraData = Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").ExtraData;
            Assert.Equal(extraData, loadedExtraData);
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

            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").Status);
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

            Client.Events.Book(evnt.Key, new[] {"A-1"}, null, null, true, true);

            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").Status);
        }  
        
        [Fact]
        public void IgnoreSocialDistancing()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var ruleset = SocialDistancingRuleset.Fixed("ruleset")
                .WithDisabledSeats(new List<string> {"A-1"})
                .Build();
            
            var rulesets = new Dictionary<string, SocialDistancingRuleset>
            {
                {"ruleset", ruleset},
            };
            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);
            Client.Events.Update(evnt.Key, new UpdateEventParams().WithSocialDistancingRulesetKey("ruleset"));

            Client.Events.Book(evnt.Key, new[] {"A-1"}, null, null, null, null, null, true);

            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(evnt.Key, "A-1").Status);
        }
    }
}
