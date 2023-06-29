using System.Collections.Generic;
using SeatsioDotNet.Charts;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Events;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class ChangeObjectStatusForMultipleEventsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var event1 = Client.Events.Create(chartKey);
            var event2 = Client.Events.Create(chartKey);

            Client.Events.ChangeObjectStatus(new[] {event1.Key, event2.Key}, new[] {"A-1", "A-2"}, "foo");

            Assert.Equal("foo", Client.Events.RetrieveObjectInfo(event1.Key, "A-1").Status);
            Assert.Equal("foo", Client.Events.RetrieveObjectInfo(event1.Key, "A-2").Status);
            Assert.Equal("foo", Client.Events.RetrieveObjectInfo(event2.Key, "A-1").Status);
            Assert.Equal("foo", Client.Events.RetrieveObjectInfo(event2.Key, "A-2").Status);
        }      
        
        [Fact]
        public void Book()
        {
            var chartKey = CreateTestChart();
            var event1 = Client.Events.Create(chartKey);
            var event2 = Client.Events.Create(chartKey);

            Client.Events.Book(new[] {event1.Key, event2.Key}, new[] {"A-1", "A-2"});

            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(event1.Key, "A-1").Status);
            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(event1.Key, "A-2").Status);
            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(event2.Key, "A-1").Status);
            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(event2.Key, "A-2").Status);
        } 
        
        [Fact]
        public void Hold()
        {
            var chartKey = CreateTestChart();
            var event1 = Client.Events.Create(chartKey);
            var event2 = Client.Events.Create(chartKey);
            HoldToken holdToken = Client.HoldTokens.Create();
            
            Client.Events.Hold(new[] {event1.Key, event2.Key}, new[] {"A-1", "A-2"}, holdToken.Token);

            Assert.Equal(EventObjectInfo.Held, Client.Events.RetrieveObjectInfo(event1.Key, "A-1").Status);
            Assert.Equal(EventObjectInfo.Held, Client.Events.RetrieveObjectInfo(event1.Key, "A-2").Status);
            Assert.Equal(EventObjectInfo.Held, Client.Events.RetrieveObjectInfo(event2.Key, "A-1").Status);
            Assert.Equal(EventObjectInfo.Held, Client.Events.RetrieveObjectInfo(event2.Key, "A-2").Status);
        }  
        
        [Fact]
        public void Release()
        {
            var chartKey = CreateTestChart();
            var event1 = Client.Events.Create(chartKey);
            var event2 = Client.Events.Create(chartKey);
            Client.Events.Book(new[] {event1.Key, event2.Key}, new[] {"A-1", "A-2"});
            
            Client.Events.Release(new[] {event1.Key, event2.Key}, new[] {"A-1", "A-2"});

            Assert.Equal(EventObjectInfo.Free, Client.Events.RetrieveObjectInfo(event1.Key, "A-1").Status);
            Assert.Equal(EventObjectInfo.Free, Client.Events.RetrieveObjectInfo(event1.Key, "A-2").Status);
            Assert.Equal(EventObjectInfo.Free, Client.Events.RetrieveObjectInfo(event2.Key, "A-1").Status);
            Assert.Equal(EventObjectInfo.Free, Client.Events.RetrieveObjectInfo(event2.Key, "A-2").Status);
        }
        
        [Fact]
        public void IgnoreSocialDistancing()
        {
            var chartKey = CreateTestChart();
            var ruleset = SocialDistancingRuleset.Fixed("ruleset")
                .WithDisabledSeats(new List<string> {"A-1"})
                .Build();
            var rulesets = new Dictionary<string, SocialDistancingRuleset>
            {
                {"ruleset", ruleset},
            };
            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);
            var event1 = Client.Events.Create(chartKey, new CreateEventParams().WithSocialDistancingRulesetKey("ruleset"));
            var event2 = Client.Events.Create(chartKey, new CreateEventParams().WithSocialDistancingRulesetKey("ruleset"));

            Client.Events.Book(new[] {event1.Key, event2.Key}, new[] {"A-1"}, null, null, null, null, null, true);

            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(event1.Key, "A-1").Status);
            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo(event2.Key, "A-1").Status);
        }

    }
}