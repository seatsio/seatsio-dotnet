using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Seasons
{
    public class ListSeasonsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var season1 = Client.Seasons.Create(chartKey);
            var season2 = Client.Seasons.Create(chartKey);
            var season3 = Client.Seasons.Create(chartKey);

            var seasons = Client.Seasons.ListAll();

            Assert.Equal(new[] {season3.Key, season2.Key, season1.Key}, seasons.Select(s => s.Key));
        }
    }
}