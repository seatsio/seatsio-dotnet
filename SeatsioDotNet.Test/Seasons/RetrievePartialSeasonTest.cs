using System;
using Xunit;

namespace SeatsioDotNet.Test.Seasons
{
    public class RetrievePartialSeasonTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();

            var season = Client.Seasons.Create(chartKey);
            var partialSeason = Client.Seasons.CreatePartialSeason(season.Key);

            var retrievedPartialSeason = Client.Seasons.RetrievePartialSeason(season.Key, partialSeason.Key);

            Assert.NotNull(retrievedPartialSeason.Key);
            Assert.NotEqual(0, retrievedPartialSeason.Id);

            var seasonEvent = retrievedPartialSeason.SeasonEvent;
            Assert.Equal(retrievedPartialSeason.Key, seasonEvent.Key);
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