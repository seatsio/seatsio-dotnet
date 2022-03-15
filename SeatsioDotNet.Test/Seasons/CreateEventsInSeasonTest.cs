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

            var events = Client.Seasons.CreateEvents(season.Key, eventKeys: new[] {"event1", "event2"});

            Assert.Equal(new[] {"event2", "event1"}, events.Select(e => e.Key));
            Assert.True(events[0].IsEventInSeason);
            Assert.Equal(season.Key, events[0].TopLevelSeasonKey);
        }

        [Fact]
        public void NumberOfEventsCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var season = Client.Seasons.Create(chartKey);

            var events = Client.Seasons.CreateEvents(season.Key, numberOfEvents: 2);

            Assert.Equal(2, events.Length);
        }
    }
}