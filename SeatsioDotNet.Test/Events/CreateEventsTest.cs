using System.Collections.Generic;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class CreateEventsTest : SeatsioClientTest
    {
        [Fact]
        public void MultipleEvents()
        {
            var chartKey = CreateTestChart();
            var eventCreationParams = new[]
            {
                new EventCreationParams(),
                new EventCreationParams()
            };

            var events = Client.Events.Create(chartKey, eventCreationParams);

            Assert.Equal(2, events.Length);
        }

        [Fact]
        public void SingleEvent()
        {
            var chartKey = CreateTestChart();
            var eventCreationParams = new[]
            {
                new EventCreationParams()
            };

            var events = Client.Events.Create(chartKey, eventCreationParams);

            Assert.Equal(1, events.Length);
            var e = events[0];
            Assert.NotNull(e.Id);
            Assert.NotNull(e.Key);
            Assert.Equal(chartKey, e.ChartKey);
            Assert.Equal("INHERIT", e.TableBookingConfig.Mode);
            Assert.True(e.SupportsBestAvailable);
            Assert.Null(e.ForSaleConfig);
            Assert.NotNull(e.CreatedOn);
            Assert.Null(e.UpdatedOn);
        }

        [Fact]
        public void EventKeyCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var eventCreationParams = new[]
            {
                new EventCreationParams("event1"),
                new EventCreationParams("event2")
            };

            var events = Client.Events.Create(chartKey, eventCreationParams);

            Assert.Equal("event1", events[0].Key);
            Assert.Equal("event2", events[1].Key);
        }

        [Fact]
        public void TableBookingModesCanBePassedIn()
        {
            var chartKey = CreateTestChartWithTables();
            var eventCreationParams = new[]
            {
                new EventCreationParams(null, TableBookingConfig.Custom(new() {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}})),
                new EventCreationParams(null, TableBookingConfig.Custom(new() {{"T1", "BY_SEAT"}, {"T2", "BY_TABLE"}}))
            };

            var events = Client.Events.Create(chartKey, eventCreationParams);

            Assert.Equal("CUSTOM", events[0].TableBookingConfig.Mode);
            Assert.Equal(new() {{"T1", "BY_TABLE"}, {"T2", "BY_SEAT"}}, events[0].TableBookingConfig.Tables);
            Assert.Equal("CUSTOM", events[1].TableBookingConfig.Mode);
            Assert.Equal(new() {{"T1", "BY_SEAT"}, {"T2", "BY_TABLE"}}, events[1].TableBookingConfig.Tables);
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
            var eventCreationParams = new[]
            {
                new EventCreationParams(null, "ruleset1"),
                new EventCreationParams(null, "ruleset1")
            };

            var events = Client.Events.Create(chartKey, eventCreationParams);

            Assert.Equal("ruleset1", events[0].SocialDistancingRulesetKey);
            Assert.Equal("ruleset1", events[1].SocialDistancingRulesetKey);
        }

        [Fact]
        public void ObjectCategoriesCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var objectCategories = new Dictionary<string, object>()
            {
                {"A-1", 10L}
            };
            var eventCreationParams = new[]
            {
                new EventCreationParams(null, objectCategories)
            };

            var events = Client.Events.Create(chartKey, eventCreationParams);

            Assert.Equal(objectCategories, events[0].ObjectCategories);
        }

        [Fact]
        public void CategoriesCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var eventCategory = new Category("eventCategory", "event-level category", "#AAABBB");
            var categories = new[] {eventCategory};
            var eventCreationParams = new[]
            {
                new EventCreationParams(null, categories)
            };
            var events = Client.Events.Create(chartKey, eventCreationParams);
            
            Assert.Equal(1, events.Length);
            Assert.Equal(TestChartCategories.Count + categories.Length, events[0].Categories.Count);
            Assert.Contains(eventCategory, events[0].Categories);
        }

        [Fact]
        public void ErrorOnDuplicateKeys()
        {
            var chartKey = CreateTestChart();
            var eventCreationParams = new[]
            {
                new EventCreationParams("e1"),
                new EventCreationParams("e1")
            };

            Assert.Throws<SeatsioException>(() => Client.Events.Create(chartKey, eventCreationParams));
        }
    }
}