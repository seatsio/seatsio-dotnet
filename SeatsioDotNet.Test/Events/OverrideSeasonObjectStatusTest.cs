using System.Threading.Tasks;
using SeatsioDotNet.EventReports;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class OverrideSeasonObjectStatusTest : SeatsioClientTest
    {
        [Fact]
        public async Task Test()
        {
            var chartKey = CreateTestChart();
            var season = await Client.Seasons.CreateAsync(chartKey, null, null, new[] {"event1"});
            await Client.Events.BookAsync(season.Key, new[] {"A-1", "A-2"});

            await Client.Events.OverrideSeasonObjectStatusAsync("event1", new[] {"A-1", "A-2"});

            Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync("event1", "A-1")).Status);
            Assert.Equal(EventObjectInfo.Free, (await Client.Events.RetrieveObjectInfoAsync("event1", "A-2")).Status);
        }
    }
}