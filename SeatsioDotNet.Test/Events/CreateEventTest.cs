using System;
using System.Collections.Generic;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class CreateEventTest : SeatsioClientTest
    {
        [Fact]
        public void ChartKeyIsMandatory()
        {
            var chartKey = CreateTestChart();

            var evnt = Client.Events.Create(chartKey);

            Assert.NotNull(evnt.Key);
            Assert.NotEqual(0, evnt.Id);
            Assert.Equal(chartKey, evnt.ChartKey);
            Assert.Equal("INHERIT", evnt.TableBookingConfig.Mode);
            Assert.True(evnt.SupportsBestAvailable);
            Assert.Null(evnt.ForSaleConfig);
            CustomAssert.CloseTo(DateTimeOffset.Now, evnt.CreatedOn.Value);
            Assert.Null(evnt.UpdatedOn);
        }

        [Fact]
        public void EventKeyIsOptional()
        {
            var chartKey = CreateTestChart();

            var evnt = Client.Events.Create(chartKey, "eventje");

            Assert.Equal("eventje", evnt.Key);
        }

        [Fact]
        public void TableBookingModesAreOptional()
        {
            var chartKey = CreateTestChartWithTables();
            var tableBookingConfig = TableBookingConfig.Custom(new Dictionary<string, string> {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}});

            var evnt = Client.Events.Create(chartKey, null, tableBookingConfig);

            Assert.NotNull(evnt.Key);
            Assert.Equal("CUSTOM", evnt.TableBookingConfig.Mode);
            Assert.Equal(new Dictionary<string, string> {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}}, evnt.TableBookingConfig.Tables);
        }   
        
        [Fact]
        public void SocialDistancingRulesetKeyCanBepassedIn()
        {
            var chartKey = CreateTestChart();
            var rulesets = new Dictionary<string, SocialDistancingRuleset>()
            {
                { "ruleset1", new SocialDistancingRuleset(0, "My first ruleset") },
            };
            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);

            var evnt = Client.Events.Create(chartKey, null, null, "ruleset1");
            
            Assert.Equal("ruleset1", evnt.SocialDistancingRulesetKey);
        }
    }
}