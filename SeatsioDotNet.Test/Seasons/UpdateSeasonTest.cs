using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class UpdateSeasonTest : SeatsioClientTest
{
    [Fact]
    public async Task UpdateEventKey()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey);

        await Client.Seasons.UpdateAsync(season.Key, new UpdateSeasonParams().WithKey("newKey"));

        var retrievedSeason = await Client.Seasons.RetrieveAsync("newKey");
        Assert.Equal("newKey", retrievedSeason.Key);
        Assert.Equal(chartKey, retrievedSeason.ChartKey);
    }

    [Fact]
    public async Task UpdateTableBookingConfig()
    {
        var chartKey = CreateTestChartWithTables();
        var season = await Client.Seasons.CreateAsync(chartKey, tableBookingConfig: TableBookingConfig.Custom(new() {{"T1", "BY_TABLE"}}));

        await Client.Seasons.UpdateAsync(season.Key, new UpdateSeasonParams().WithTableBookingConfig(TableBookingConfig.Custom(new() {{"T1", "BY_SEAT"}})));

        var retrievedSeason = await Client.Seasons.RetrieveAsync(season.Key);
        Assert.Equal(season.Key, retrievedSeason.Key);
        Assert.Equal(chartKey, retrievedSeason.ChartKey);
        Assert.Equal("CUSTOM", retrievedSeason.TableBookingConfig.Mode);
        Assert.Equal(new() {{"T1", "BY_SEAT"}}, retrievedSeason.TableBookingConfig.Tables);
    }

    [Fact]
    public async Task UpdateName()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey, name: "A season");

        await Client.Seasons.UpdateAsync(season.Key, new UpdateSeasonParams().WithName("Another season"));

        var retrievedSeason = await Client.Seasons.RetrieveAsync(season.Key);
        Assert.Equal("Another season", retrievedSeason.Name);
    }  
    
    [Fact]
    public async Task UpdateObjectCategories()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey);

        var newObjectCategories = new Dictionary<string, object>()
        {
            {"A-2", 9L}
        };
        await Client.Seasons.UpdateAsync(season.Key, new UpdateSeasonParams().WithObjectCategories(newObjectCategories));

        var retrievedSeason = await Client.Seasons.RetrieveAsync(season.Key);
        Assert.Equal(newObjectCategories, retrievedSeason.ObjectCategories);
    }

    [Fact]
    public async Task UpdateCategories()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey);
        Category eventCategory = new Category("eventCategory", "event-level category", "#AAABBB");
        Category[] categories = new[] {eventCategory};

        await Client.Seasons.UpdateAsync(season.Key, new UpdateSeasonParams().WithCategories(categories));

        var retrievedSeason = await Client.Seasons.RetrieveAsync(season.Key);
        Assert.Equal(TestChartCategories.Count + categories.Length, retrievedSeason.Categories.Count);
        Assert.Contains(eventCategory, retrievedSeason.Categories);
    }
    
    [Fact]
    public async Task UpdateForSalePropagated()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey);

        await Client.Seasons.UpdateAsync(season.Key, new UpdateSeasonParams().WithForSalePropagated(false));

        var retrievedSeason = await Client.Seasons.RetrieveAsync(season.Key);
        Assert.False(retrievedSeason.ForSalePropagated);
    }
}