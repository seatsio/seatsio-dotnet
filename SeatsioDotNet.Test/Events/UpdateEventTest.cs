using System;
using System.Collections.Generic;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class UpdateEventTest : SeatsioClientTest
    {
        [Fact]
        public void UpdateChartKey()
        {
            var chartKey1 = CreateTestChart();
            var chartKey2 = CreateTestChart();
            var evnt = Client.Events.Create(chartKey1);

            Client.Events.Update(evnt.Key, chartKey2, null);

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(evnt.Key, retrievedEvent.Key);
            Assert.Equal(chartKey2, retrievedEvent.ChartKey);
            CustomAssert.CloseTo(DateTimeOffset.Now, retrievedEvent.UpdatedOn.Value);
        }    
        
        [Fact]
        public void UpdateEventKey()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            Client.Events.Update(evnt.Key, null, "newKey");

            var retrievedEvent = Client.Events.Retrieve("newKey");
            Assert.Equal("newKey", retrievedEvent.Key);
            Assert.Equal(chartKey, retrievedEvent.ChartKey);
        }

        [Fact]
        public void UpdateTableBookingConfig()
        {
            var chartKey = CreateTestChartWithTables();
            var evnt = Client.Events.Create(chartKey, null, TableBookingConfig.Custom(new Dictionary<string, string> {{"T1", "BY_TABLE"}}));

            Client.Events.Update(evnt.Key, null, null, TableBookingConfig.Custom(new Dictionary<string, string> {{"T1", "BY_SEAT"}}));

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(evnt.Key, retrievedEvent.Key);
            Assert.Equal(chartKey, retrievedEvent.ChartKey);
            Assert.Equal("CUSTOM", retrievedEvent.TableBookingConfig.Mode);
            Assert.Equal(new Dictionary<string, string> {{"T1", "BY_SEAT"}}, retrievedEvent.TableBookingConfig.Tables);
        }     
        
        [Fact]
        public void UpdateSocialDistancingRulesetKey()
        {
            var chartKey = CreateTestChart();
            var rulesets = new Dictionary<string, SocialDistancingRuleset>()
            {
                { "ruleset1", SocialDistancingRuleset.RuleBased(0, "My first ruleset") },
                { "ruleset2", SocialDistancingRuleset.RuleBased(0, "My second ruleset") }
            };
            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);
            var evnt = Client.Events.Create(chartKey, null, null, "ruleset1");

            Client.Events.Update(evnt.Key, null, null, null, "ruleset2");

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(retrievedEvent.SocialDistancingRulesetKey, "ruleset2");
        }      
        
        [Fact]
        public void RemoveSocialDistancingRulesetKey()
        {
            var chartKey = CreateTestChart();
            var rulesets = new Dictionary<string, SocialDistancingRuleset>()
            {
                { "ruleset1", new SocialDistancingRuleset(0, "My first ruleset") }
            };
            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);
            var evnt = Client.Events.Create(chartKey, null, null, "ruleset1");

            Client.Events.Update(evnt.Key, null, null, null, "");

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Null(retrievedEvent.SocialDistancingRulesetKey);
        }
    }
}