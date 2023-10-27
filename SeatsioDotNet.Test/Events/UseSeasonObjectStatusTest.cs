using SeatsioDotNet.EventReports;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class UseSeasonObjectStatusTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var season = Client.Seasons.Create(chartKey, null, null, new[] {"event1"});
            Client.Events.Book(season.Key, new[] {"A-1", "A-2"});
            Client.Events.OverrideSeasonObjectStatus("event1", new[] {"A-1", "A-2"});
            
            Client.Events.UseSeasonObjectStatus("event1", new[] {"A-1", "A-2"});

            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo("event1", "A-1").Status);
            Assert.Equal(EventObjectInfo.Booked, Client.Events.RetrieveObjectInfo("event1", "A-2").Status);
        }
    }
}