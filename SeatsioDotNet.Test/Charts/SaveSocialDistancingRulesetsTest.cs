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
            var ruleset1 = SocialDistancingRuleset.RuleBased("My first ruleset")
                .WithIndex(0)
                .WithNumberOfDisabledSeatsToTheSides(1)
                .WithDisableSeatsInFrontAndBehind(true)
                .WithDisableDiagonalSeatsInFrontAndBehind(true)
                .WithNumberOfDisabledAisleSeats(2)
                .WithMaxGroupSize(1)
                .WithMaxOccupancyAbsolute(10)
                .WithOneGroupPerTable(true)
                .WithDisabledSeats(new List<string> {"A-1"})
                .WithEnabledSeats(new List<string> {"A-2"})
                .Build();
            
            var ruleset2 = SocialDistancingRuleset.Fixed("My second ruleset")
                .WithIndex(1)
                .WithDisabledSeats(new List<string> {"A-1"})
                .Build();
            
            var rulesets = new Dictionary<string, SocialDistancingRuleset>
            {
                {"ruleset1", ruleset1},
                {"ruleset2", ruleset2}
            };

            Client.Charts.SaveSocialDistancingRulesets(chartKey, rulesets);

            var retrievedChart = Client.Charts.Retrieve(chartKey);
            Assert.Equal(2, retrievedChart.SocialDistancingRulesets.Count);

            var retrievedRuleset1 = retrievedChart.SocialDistancingRulesets["ruleset1"];
            Assert.Equal(0, retrievedRuleset1.Index);
            Assert.Equal("My first ruleset", retrievedRuleset1.Name);
            Assert.Equal(1, retrievedRuleset1.NumberOfDisabledSeatsToTheSides);
            Assert.True(retrievedRuleset1.DisableSeatsInFrontAndBehind);
            Assert.True(retrievedRuleset1.DisableDiagonalSeatsInFrontAndBehind);
            Assert.Equal(2, retrievedRuleset1.NumberOfDisabledAisleSeats);
            Assert.Equal(1, retrievedRuleset1.MaxGroupSize);
            Assert.Equal(10, retrievedRuleset1.MaxOccupancyAbsolute);
            Assert.Equal(0, retrievedRuleset1.MaxOccupancyPercentage);
            Assert.True(retrievedRuleset1.OneGroupPerTable);
            Assert.False(retrievedRuleset1.FixedGroupLayout);
            Assert.Equal(new List<string> {"A-1"}, retrievedRuleset1.DisabledSeats);
            Assert.Equal(new List<string> {"A-2"}, retrievedRuleset1.EnabledSeats);

            var retrievedRuleset2 = retrievedChart.SocialDistancingRulesets["ruleset2"];
            Assert.Equal(1, retrievedRuleset2.Index);
            Assert.Equal("My second ruleset", retrievedRuleset2.Name);
            Assert.Equal(0, retrievedRuleset2.NumberOfDisabledSeatsToTheSides);
            Assert.False(retrievedRuleset2.DisableSeatsInFrontAndBehind);
            Assert.False(retrievedRuleset2.DisableDiagonalSeatsInFrontAndBehind);
            Assert.Equal(0, retrievedRuleset2.NumberOfDisabledAisleSeats);
            Assert.Equal(0, retrievedRuleset2.MaxGroupSize);
            Assert.Equal(0, retrievedRuleset2.MaxOccupancyAbsolute);
            Assert.Equal(0, retrievedRuleset2.MaxOccupancyPercentage);
            Assert.False(retrievedRuleset2.OneGroupPerTable);
            Assert.True(retrievedRuleset2.FixedGroupLayout);
            Assert.Equal(new List<string> {"A-1"}, retrievedRuleset1.DisabledSeats);
            Assert.Empty(retrievedRuleset2.EnabledSeats);
        }
    }
}