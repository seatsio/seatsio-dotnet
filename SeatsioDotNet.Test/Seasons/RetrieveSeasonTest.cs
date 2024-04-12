using System;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Seasons;

public class RetrieveSeasonTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();

        var season = await Client.Seasons.CreateAsync(chartKey);
        var partialSeason1 = await Client.Seasons.CreatePartialSeasonAsync(season.Key);
        var partialSeason2 = await Client.Seasons.CreatePartialSeasonAsync(season.Key);

        var retrievedSeason = await Client.Seasons.RetrieveAsync(season.Key);

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