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

            Client.Events.Update(evnt.Key, new UpdateEventParams().WithChartKey(chartKey2));

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

            Client.Events.Update(evnt.Key, new UpdateEventParams().WithKey("newKey"));

            var retrievedEvent = Client.Events.Retrieve("newKey");
            Assert.Equal("newKey", retrievedEvent.Key);
            Assert.Equal(chartKey, retrievedEvent.ChartKey);
        }

        [Fact]
        public void UpdateTableBookingConfig()
        {
            var chartKey = CreateTestChartWithTables();
            var evnt = Client.Events.Create(chartKey,
                new CreateEventParams().WithTableBookingConfig(TableBookingConfig.Custom(new() {{"T1", "BY_TABLE"}})));

            Client.Events.Update(evnt.Key, new UpdateEventParams().WithTableBookingConfig(TableBookingConfig.Custom(new() {{"T1", "BY_SEAT"}})));

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(evnt.Key, retrievedEvent.Key);
            Assert.Equal(chartKey, retrievedEvent.ChartKey);
            Assert.Equal("CUSTOM", retrievedEvent.TableBookingConfig.Mode);
            Assert.Equal(new() {{"T1", "BY_SEAT"}}, retrievedEvent.TableBookingConfig.Tables);
        }

        [Fact]
        public void UpdateSocialDistancingRulesetKey()
        {
            var chartKey = CreateTestChart();
            var rulesets = new Dictionary<string, SocialDistancingRuleset>()
            {
                {"ruleset1", SocialDistancingRuleset.RuleBased("My first ruleset").Build()},
                {"ruleset2", SocialDistancingRuleset.RuleBased("My second ruleset").Build()}
            };
            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);
            var evnt = Client.Events.Create(chartKey,
                new CreateEventParams().WithSocialDistancingRulesetKey("ruleset1"));

            Client.Events.Update(evnt.Key, new UpdateEventParams().WithSocialDistancingRulesetKey("ruleset2"));

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(retrievedEvent.SocialDistancingRulesetKey, "ruleset2");
        }

        [Fact]
        public void RemoveSocialDistancingRulesetKey()
        {
            var chartKey = CreateTestChart();
            var rulesets = new Dictionary<string, SocialDistancingRuleset>()
            {
                {"ruleset1", SocialDistancingRuleset.RuleBased("My first ruleset").Build()}
            };
            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);
            var evnt = Client.Events.Create(chartKey,
                new CreateEventParams().WithSocialDistancingRulesetKey("ruleset1"));

            Client.Events.Update(evnt.Key, new UpdateEventParams().WithSocialDistancingRulesetKey(""));

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Null(retrievedEvent.SocialDistancingRulesetKey);
        }

        [Fact]
        public void UpdateObjectCategories()
        {
            var chartKey = CreateTestChart();
            var objectCategories = new Dictionary<string, object>()
            {
                {"A-1", 10L}
            };
            var evnt = Client.Events.Create(chartKey, new CreateEventParams().WithObjectCategories(objectCategories));

            var newObjectCategories = new Dictionary<string, object>()
            {
                {"A-2", 9L}
            };
            Client.Events.Update(evnt.Key, new UpdateEventParams().WithObjectCategories(newObjectCategories));

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(newObjectCategories, retrievedEvent.ObjectCategories);
        }

        [Fact]
        public void RemoveObjectCategories()
        {
            var chartKey = CreateTestChart();
            var objectCategories = new Dictionary<string, object>()
            {
                {"A-1", 10L}
            };
            var evnt = Client.Events.Create(chartKey, new CreateEventParams().WithObjectCategories(objectCategories));

            Client.Events.Update(evnt.Key, new UpdateEventParams().WithObjectCategories(new Dictionary<string, object>()));

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Null(retrievedEvent.ObjectCategories);
        }

        [Fact]
        public void UpdateCategories()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            Category eventCategory = new Category("eventCategory", "event-level category", "#AAABBB");
            Category[] categories = new[] {eventCategory};

            Client.Events.Update(evnt.Key, new UpdateEventParams().WithCategories(categories));

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(TestChartCategories.Count + categories.Length, retrievedEvent.Categories.Count);
            Assert.Contains(eventCategory, retrievedEvent.Categories);
        }

        [Fact]
        public void RemoveCategories()
        {
            var chartKey = CreateTestChart();
            Category eventCategory = new Category("eventCategory", "event-level category", "#AAABBB");
            Category[] categories = new[] {eventCategory};

            var evnt = Client.Events.Create(chartKey, new CreateEventParams().WithCategories(categories));

            Client.Events.Update(evnt.Key, new UpdateEventParams().WithCategories(new Category[] {}));

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(TestChartCategories.Count, retrievedEvent.Categories.Count);
            Assert.DoesNotContain(eventCategory, retrievedEvent.Categories);
        }

        [Fact]
        public void UpdateName()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey, new CreateEventParams().WithName("An event"));

            Client.Events.Update(evnt.Key, new UpdateEventParams().WithName("Another event"));

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal("Another event", retrievedEvent.Name);
        }  
        
        [Fact]
        public void UpdateDate()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey, new CreateEventParams().WithName("An event"));

            Client.Events.Update(evnt.Key, new UpdateEventParams().WithDate(new DateOnly(2022, 1, 10)));

            var retrievedEvent = Client.Events.Retrieve(evnt.Key);
            Assert.Equal(new DateOnly(2022, 1, 10), retrievedEvent.Date);
        }
    }
}