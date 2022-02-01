using Xunit;

namespace SeatsioDotNet.Test.Seasons
{
    public class DeleteSeasonTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var season = Client.Seasons.Create(chartKey);

            Client.Seasons.Delete(season.Key);

            Assert.Throws<SeatsioException>(() => Client.Seasons.Retrieve(season.Key));
        }
    }
}