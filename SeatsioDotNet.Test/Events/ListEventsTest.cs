using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class ListEventsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var event1 = Client.Events.Create(chartKey);
            var event2 = Client.Events.Create(chartKey);
            var event3 = Client.Events.Create(chartKey);

            var events = Client.Events.ListAll();

            Assert.Equal(new[] {event3.Key, event2.Key, event1.Key}, events.Select(e => e.Key));
        }

        [Fact]
        public void ListSeasons()
        {
            var chartKey = CreateTestChart();
            var season1 = Client.Seasons.Create(chartKey);
            var season2 = Client.Seasons.Create(chartKey);
            var season3 = Client.Seasons.Create(chartKey);

            var seasons = Client.Events.ListAll();

            Assert.Equal(new[] {true, true, true}, seasons.Select(s => s.IsSeason));
        }
    }
}