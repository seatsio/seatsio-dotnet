using System.Collections.Generic;
using FluentAssertions;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Events;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class HoldObjectsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            HoldToken holdToken = Client.HoldTokens.Create();

            var result = Client.Events.Hold(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

            var status1 = Client.Events.RetrieveObjectStatus(evnt.Key, "A-1");
            Assert.Equal(ObjectStatus.Held, status1.Status);
            Assert.Equal(holdToken.Token, status1.HoldToken);

            var status2 = Client.Events.RetrieveObjectStatus(evnt.Key, "A-2");
            Assert.Equal(ObjectStatus.Held, status2.Status);
            Assert.Equal(holdToken.Token, status2.HoldToken);
            Assert.Equal(new[] {"A-1", "A-2"}, result.Objects.Keys);
        }

        [Fact]
        public void OrderId()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            HoldToken holdToken = Client.HoldTokens.Create();

            Client.Events.Hold(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token, "order1");

            Assert.Equal("order1", Client.Events.RetrieveObjectStatus(evnt.Key, "A-1").OrderId);
            Assert.Equal("order1", Client.Events.RetrieveObjectStatus(evnt.Key, "A-2").OrderId);
        }

        [Fact]
        public void BestAvailable()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            HoldToken holdToken = Client.HoldTokens.Create();

            var bestAvailableResult = Client.Events.Hold(evnt.Key, new BestAvailable(3), holdToken.Token, "order1");

            Assert.True(bestAvailableResult.NextToEachOther);
            Assert.Equal(new[] {"B-4", "B-5", "B-6"}, bestAvailableResult.Objects);
            Assert.Equal("order1", Client.Events.RetrieveObjectStatus(evnt.Key, "B-4").OrderId);
        }

        [Fact]
        public void KeepExtraData()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            var extraData = new Dictionary<string, object> {{"foo1", "bar1"}};
            Client.Events.UpdateExtraData(evnt.Key, "A-1", extraData);
            HoldToken holdToken = Client.HoldTokens.Create();

            Client.Events.Hold(evnt.Key, new[] {"A-1"}, holdToken.Token, null, true);

            Assert.Equal(extraData, Client.Events.RetrieveObjectStatus(evnt.Key, "A-1").ExtraData);
        }

       [Fact]
        public void ChannelKeys()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            HoldToken holdToken = Client.HoldTokens.Create();
            var channels = new Dictionary<string, Channel>()
            {
                { "channelKey1", new Channel("channel 1", "#FFFF00", 1) }
            };
            Client.Events.UpdateChannels(evnt.Key, channels);
            Client.Events.AssignObjectsToChannel(evnt.Key, new
            {
                channelKey1 = new [] {"A-1", "A-2"}
            });

            Client.Events.Hold(evnt.Key, new[] {"A-1"}, holdToken.Token, null, true, null, new[] {"channelKey1"});

            Assert.Equal(ObjectStatus.Held, Client.Events.RetrieveObjectStatus(evnt.Key, "A-1").Status);
        }    
        
        [Fact]
        public void IgnoreChannels()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            HoldToken holdToken = Client.HoldTokens.Create();
            var channels = new Dictionary<string, Channel>()
            {
                { "channelKey1", new Channel("channel 1", "#FFFF00", 1) }
            };
            Client.Events.UpdateChannels(evnt.Key, channels);
            Client.Events.AssignObjectsToChannel(evnt.Key, new
            {
                channelKey1 = new [] {"A-1", "A-2"}
            });

            Client.Events.Hold(evnt.Key, new[] {"A-1"}, holdToken.Token, null, true, true);

            Assert.Equal(ObjectStatus.Held, Client.Events.RetrieveObjectStatus(evnt.Key, "A-1").Status);
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
            Client.Events.Update(evnt.Key, null, null, null, "ruleset");
            HoldToken holdToken = Client.HoldTokens.Create();

            Client.Events.Hold(evnt.Key, new[] {"A-1"}, holdToken.Token, null, null, null, null, true);

            Assert.Equal(ObjectStatus.Held, Client.Events.RetrieveObjectStatus(evnt.Key, "A-1").Status);
        }
    }
}
