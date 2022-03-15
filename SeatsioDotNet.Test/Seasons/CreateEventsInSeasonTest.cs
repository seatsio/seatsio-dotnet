using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Seasons
{
    public class CreateEventsInSeasonTest : SeatsioClientTest
    {
        [Fact]
        public void EventsKeysCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var season = Client.Seasons.Create(chartKey);

            var updatedSeason = Client.Seasons.CreateEvents(season.Key, eventKeys: new[] {"event1", "event2"});

            Assert.Equal(new[] {"event2", "event1"}, updatedSeason.Events.Select(e => e.Key));
            Assert.True(updatedSeason.Events[0].IsEventInSeason);
            Assert.Equal(season.Key, updatedSeason.Events[0].TopLevelSeasonKey);
        }

        [Fact]
        public void NumberOfEventsCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var season = Client.Seasons.Create(chartKey);

            var updatedSeason = Client.Seasons.CreateEvents(season.Key, numberOfEvents: 2);

            Assert.Equal(2, updatedSeason.Events.Count);
        }
    }
}