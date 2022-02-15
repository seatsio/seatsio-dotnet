using System;
using Xunit;

namespace SeatsioDotNet.Test.Seasons
{
    public class RetrieveSeasonTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();

            var season = Client.Seasons.Create(chartKey);
            var partialSeason1 = Client.Seasons.CreatePartialSeason(season.Key);
            var partialSeason2 = Client.Seasons.CreatePartialSeason(season.Key);

            var retrievedSeason = Client.Seasons.Retrieve(season.Key);

            Assert.NotNull(retrievedSeason.Key);
            Assert.NotEqual(0, retrievedSeason.Id);
            Assert.Equal(new[] {partialSeason1.Key, partialSeason2.Key}, retrievedSeason.PartialSeasonKeys);
            Assert.Empty(retrievedSeason.Events);
            Assert.Equal(chartKey, season.ChartKey);
            Assert.Equal("INHERIT", season.TableBookingConfig.Mode);
            Assert.True(season.SupportsBestAvailable);
            Assert.Null(season.ForSaleConfig);
            CustomAssert.CloseTo(DateTimeOffset.Now, season.CreatedOn.Value);
            Assert.Null(season.UpdatedOn);
        }
    }
}