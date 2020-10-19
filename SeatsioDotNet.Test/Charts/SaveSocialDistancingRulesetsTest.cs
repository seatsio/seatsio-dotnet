using System.Collections.Generic;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Charts
{
    public class SaveSocialDistancingRulesetsTest : SeatsioClientTest
    {
        [Fact]
        public void SaveRulesets()
        {
            var chartKey = CreateTestChart();
            var rulesets = new Dictionary<string, SocialDistancingRuleset>()
            {
                {"ruleset1", new SocialDistancingRuleset(0, "My first ruleset", 1, true, 2, 1, 10, 0, true, false, new List<string> {"A-1"}, new List<string> {"A-2"})},
                {"ruleset2", new SocialDistancingRuleset(1, "My second ruleset")}
            };

            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);

            var retrievedChart = Client.Charts.Retrieve(chartKey);
            Assert.Equal(2, retrievedChart.SocialDistancingRulesets.Count);

            var ruleset1 = retrievedChart.SocialDistancingRulesets["ruleset1"];
            Assert.Equal(0, ruleset1.Index);
            Assert.Equal("My first ruleset", ruleset1.Name);
            Assert.Equal(1, ruleset1.NumberOfDisabledSeatsToTheSides);
            Assert.True(ruleset1.DisableSeatsInFrontAndBehind);
            Assert.Equal(2, ruleset1.NumberOfDisabledAisleSeats);
            Assert.Equal(1, ruleset1.MaxGroupSize);
            Assert.Equal(10, ruleset1.MaxOccupancyAbsolute);
            Assert.Equal(0, ruleset1.MaxOccupancyPercentage);
            Assert.True(ruleset1.OneGroupPerTable);
            Assert.False(ruleset1.FixedGroupLayout);
            Assert.Equal(new List<string> {"A-1"}, ruleset1.DisabledSeats);
            Assert.Equal(new List<string> {"A-2"}, ruleset1.EnabledSeats);

            var ruleset2 = retrievedChart.SocialDistancingRulesets["ruleset2"];
            Assert.Equal(1, ruleset2.Index);
            Assert.Equal("My second ruleset", ruleset2.Name);
            Assert.Equal(0, ruleset2.NumberOfDisabledSeatsToTheSides);
            Assert.False(ruleset2.DisableSeatsInFrontAndBehind);
            Assert.Equal(0, ruleset2.NumberOfDisabledAisleSeats);
            Assert.Equal(0, ruleset2.MaxGroupSize);
            Assert.Equal(0, ruleset2.MaxOccupancyAbsolute);
            Assert.Equal(0, ruleset2.MaxOccupancyPercentage);
            Assert.False(ruleset2.OneGroupPerTable);
            Assert.False(ruleset2.FixedGroupLayout);
            Assert.Empty(ruleset2.DisabledSeats);
            Assert.Empty(ruleset2.EnabledSeats);
        }

        [Fact]
        public void RuleBased()
        {
            var chartKey = CreateTestChart();
            var rulesets = new Dictionary<string, SocialDistancingRuleset>()
            {
                { "ruleset1", SocialDistancingRuleset.RuleBased(0, "My first ruleset", 1, true, 2, 1, 10, 0, true, new List<string> {"A-1"}, new List<string> {"A-2"})},
            };

            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);

            var retrievedChart = Client.Charts.Retrieve(chartKey);
            Assert.Equal(1, retrievedChart.SocialDistancingRulesets.Count);

            var ruleset1 = retrievedChart.SocialDistancingRulesets["ruleset1"];
            Assert.Equal(0, ruleset1.Index);
            Assert.Equal("My first ruleset", ruleset1.Name);
            Assert.Equal(1, ruleset1.NumberOfDisabledSeatsToTheSides);
            Assert.True(ruleset1.DisableSeatsInFrontAndBehind);
            Assert.Equal(2, ruleset1.NumberOfDisabledAisleSeats);
            Assert.Equal(1, ruleset1.MaxGroupSize);
            Assert.Equal(10, ruleset1.MaxOccupancyAbsolute);
            Assert.Equal(0, ruleset1.MaxOccupancyPercentage);
            Assert.True(ruleset1.OneGroupPerTable);
            Assert.False(ruleset1.FixedGroupLayout);
            Assert.Equal(new List<string> {"A-1"}, ruleset1.DisabledSeats);
            Assert.Equal(new List<string> {"A-2"}, ruleset1.EnabledSeats);
        }

        [Fact]
        public void Fixed()
        {
            var chartKey = CreateTestChart();
            var rulesets = new Dictionary<string, SocialDistancingRuleset>()
            {
                {"ruleset1", SocialDistancingRuleset.Fixed(0, "My first ruleset", new List<string> {"A-1"})},
            };

            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);

            var retrievedChart = Client.Charts.Retrieve(chartKey);
            Assert.Equal(1, retrievedChart.SocialDistancingRulesets.Count);

            var ruleset1 = retrievedChart.SocialDistancingRulesets["ruleset1"];
            Assert.Equal(0, ruleset1.Index);
            Assert.Equal("My first ruleset", ruleset1.Name);
            Assert.True(ruleset1.FixedGroupLayout);
            Assert.Equal(new List<string> {"A-1"}, ruleset1.DisabledSeats);
        }
    }
}