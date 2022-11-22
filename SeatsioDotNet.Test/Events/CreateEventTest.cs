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
            Assert.Equal(TestChartCategories, evnt.Categories);
        }

        [Fact]
        public void EventKeyIsOptional()
        {
            var chartKey = CreateTestChart();

            var evnt = Client.Events.Create(chartKey, "eventje");

            Assert.Equal("eventje", evnt.Key);
        }

        [Fact]
        public void TableBookingModeCustomCanBeUsed()
        {
            var chartKey = CreateTestChartWithTables();
            var tableBookingConfig = TableBookingConfig.Custom(new() {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}});

            var evnt = Client.Events.Create(chartKey, null, tableBookingConfig);

            Assert.NotNull(evnt.Key);
            Assert.Equal("CUSTOM", evnt.TableBookingConfig.Mode);
            Assert.Equal(new Dictionary<string, string> {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}},
                evnt.TableBookingConfig.Tables);
        }

        [Fact]
        public void TableBookingModeInheritCanBeUsed()
        {
            var chartKey = CreateTestChartWithTables();

            var evnt = Client.Events.Create(chartKey, null, TableBookingConfig.Inherit());

            Assert.NotNull(evnt.Key);
            Assert.Equal("INHERIT", evnt.TableBookingConfig.Mode);
        }

        [Fact]
        public void SocialDistancingRulesetKeyCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var rulesets = new Dictionary<string, SocialDistancingRuleset>()
            {
                {"ruleset1", SocialDistancingRuleset.RuleBased("My first ruleset").Build()},
            };
            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);

            var evnt = Client.Events.Create(chartKey, null, null, "ruleset1");

            Assert.Equal("ruleset1", evnt.SocialDistancingRulesetKey);
        }


        [Fact]
        public void ObjectCategoriesCanBePassedIn()
        {
            var chartKey = CreateTestChart();

            var objectCategories = new Dictionary<string, object>()
            {
                {"A-1", 10L}
            };
            var evnt = Client.Events.Create(chartKey, null, null, null, objectCategories);
            Assert.Equal(objectCategories, evnt.ObjectCategories);
        }

        [Fact]
        public void CategoriesCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var eventCategory = new Category("eventCategory", "event-level category", "#AAABBB");
            var categories = new[] {eventCategory};

            var evnt = Client.Events.Create(chartKey, null, null, null, null, categories);
            
            Assert.Equal(TestChartCategories.Count + categories.Length, evnt.Categories.Count);
            Assert.Contains(eventCategory, evnt.Categories);
        }
    }
}