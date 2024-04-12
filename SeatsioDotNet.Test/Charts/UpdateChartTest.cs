using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class UpdateChartTest : SeatsioClientTest
{
    [Fact]
    public async Task Name()
    {
        var chart = await Client.Charts.CreateAsync(null, "BOOTHS", new[] {new Category(1, "Category 1", "#aaaaaa")});

        await Client.Charts.UpdateAsync(chart.Key, "aChart");

        Chart retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);
        Assert.Equal("aChart", retrievedChart.Name);
        var drawing = await Client.Charts.RetrievePublishedVersionAsync(chart.Key);
        Assert.Equal("BOOTHS", drawing.VenueType);
        Assert.Single(drawing.Categories);
    }

    [Fact]
    public async Task Categories()
    {
        var chart = await Client.Charts.CreateAsync("aChart", "BOOTHS");

        await Client.Charts.UpdateAsync(chart.Key, categories: new[] {new Category(1, "Category 1", "#aaaaaa"), new Category("cat-2", "Category 2", "#bbbbbb", true)});

        Chart retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);
        Assert.Equal("aChart", retrievedChart.Name);
        var drawing = await Client.Charts.RetrievePublishedVersionAsync(chart.Key);
        Assert.Equal("BOOTHS", drawing.VenueType);
        Assert.Equal(2, drawing.Categories.Count);
    }
}