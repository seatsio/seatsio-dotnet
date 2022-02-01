using Xunit;

namespace SeatsioDotNet.Test.Seasons
{
    public class DeletePartialSeasonTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var season = Client.Seasons.Create(chartKey);
            var partialSeason = Client.Seasons.CreatePartialSeason(season.Key);

            Client.Seasons.DeletePartialSeason(season.Key, partialSeason.Key);

            Assert.Throws<SeatsioException>(() => Client.Seasons.RetrievePartialSeason(season.Key, partialSeason.Key));
        }
    }
}