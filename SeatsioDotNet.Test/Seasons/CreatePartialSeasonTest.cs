using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Seasons
{
    public class CreatePartialSeasonTest : SeatsioClientTest
    {
        [Fact]
        public void KeyCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var topLevelSeason = Client.Seasons.Create(chartKey);

            var partialSeason = Client.Seasons.CreatePartialSeason(topLevelSeason.Key, partialSeasonKey: "aPartialSeason");

            Assert.Equal("aPartialSeason", partialSeason.Key);
            Assert.True(partialSeason.IsPartialSeason);
            Assert.Equal(topLevelSeason.Key, partialSeason.TopLevelSeasonKey);
        }

        [Fact]
        public void EventKeysCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var topLevelSeason = Client.Seasons.Create(chartKey, eventKeys: new[] {"event1", "event2"});

            var partialSeason = Client.Seasons.CreatePartialSeason(topLevelSeason.Key, eventKeys: new[] {"event1", "event2"});

            Assert.Equal(new[] {"event1", "event2"}, partialSeason.Events.Select(e => e.Key));
        }
    }
}