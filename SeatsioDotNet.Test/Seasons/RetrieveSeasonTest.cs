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

            var seasonEvent = retrievedSeason.SeasonEvent;
            Assert.Equal(season.Key, seasonEvent.Key);
            Assert.NotEqual(0, seasonEvent.Id);
            Assert.Equal(chartKey, seasonEvent.ChartKey);
            Assert.Equal("INHERIT", seasonEvent.TableBookingConfig.Mode);
            Assert.True(seasonEvent.SupportsBestAvailable);
            Assert.Null(seasonEvent.ForSaleConfig);
            CustomAssert.CloseTo(DateTimeOffset.Now, seasonEvent.CreatedOn.Value);
            Assert.Null(seasonEvent.UpdatedOn);
        }
    }
}