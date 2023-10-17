using System;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class RetrieveEventTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);

        var retrievedEvent = Client.Events.Retrieve(evnt.Key);

        Assert.NotNull(retrievedEvent.Key);
        Assert.NotEqual(0, retrievedEvent.Id);
        Assert.False(retrievedEvent.IsEventInSeason);
        Assert.Equal(chartKey, retrievedEvent.ChartKey);
        Assert.Equal("INHERIT", evnt.TableBookingConfig.Mode);
        Assert.True(retrievedEvent.SupportsBestAvailable);
        Assert.Null(retrievedEvent.ForSaleConfig);
        CustomAssert.CloseTo(DateTimeOffset.Now, retrievedEvent.CreatedOn.Value);
        Assert.Null(retrievedEvent.UpdatedOn);
        Assert.Equal(TestChartCategories, retrievedEvent.Categories);
    }

    [Fact]
    public void RetrieveSeason()
    {
        var chartKey = CreateTestChart();

        var season = Client.Seasons.Create(chartKey);
        var partialSeason1 = Client.Seasons.CreatePartialSeason(season.Key);
        var partialSeason2 = Client.Seasons.CreatePartialSeason(season.Key);

        var retrievedSeason = Client.Events.Retrieve(season.Key);

        Assert.NotNull(retrievedSeason.Key);
        Assert.NotEqual(0, retrievedSeason.Id);
        Assert.True(retrievedSeason.IsTopLevelSeason);
        Assert.Null(retrievedSeason.TopLevelSeasonKey);
        Assert.Equal(new[] {partialSeason1.Key, partialSeason2.Key}, retrievedSeason.PartialSeasonKeys);
        Assert.Empty(retrievedSeason.Events);
        Assert.Equal(chartKey, season.ChartKey);
        Assert.Equal("INHERIT", season.TableBookingConfig.Mode);
        Assert.True(season.SupportsBestAvailable);
        Assert.Null(season.ForSaleConfig);
        CustomAssert.CloseTo(DateTimeOffset.Now, season.CreatedOn.Value);
        Assert.Null(season.UpdatedOn);
        Assert.Equal(TestChartCategories, season.Categories);
    }
}