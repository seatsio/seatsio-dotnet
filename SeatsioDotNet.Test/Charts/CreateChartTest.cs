using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class CreateChartTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync();

        Chart retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);

        Assert.NotEqual(0, retrievedChart.Id);
        Assert.NotNull(retrievedChart.Key);
        Assert.Equal("NOT_USED", retrievedChart.Status);
        Assert.Equal("Untitled chart", retrievedChart.Name);
        Assert.NotNull(retrievedChart.PublishedVersionThumbnailUrl);
        Assert.Null(retrievedChart.DraftVersionThumbnailUrl);
        Assert.Null(retrievedChart.Events);
        Assert.Empty(retrievedChart.Tags);
        Assert.False(retrievedChart.Archived);

        var drawing = await Client.Charts.RetrievePublishedVersionAsync(chart.Key);
        Assert.Equal("SIMPLE", drawing.VenueType);
        Assert.Empty(drawing.Categories);
    }

    [Fact]
    public async Task Name()
    {
        var chart = await Client.Charts.CreateAsync("aChart");

        Chart retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);

        Assert.Equal("aChart", retrievedChart.Name);
        var drawing = await Client.Charts.RetrievePublishedVersionAsync(chart.Key);
        Assert.Equal("aChart", drawing.Name);
        Assert.Empty(drawing.Categories);
    }

    [Fact]
    public async Task VenueType()
    {
        var chart = await Client.Charts.CreateAsync(null, "SIMPLE");

        Chart retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);

        Assert.Equal("Untitled chart", retrievedChart.Name);
        var drawing = await Client.Charts.RetrievePublishedVersionAsync(chart.Key);
        Assert.Equal("SIMPLE", drawing.VenueType);
        Assert.Empty(drawing.Categories);
    }

    [Fact]
    public async Task Categories()
    {
        var chart = await Client.Charts.CreateAsync(null, null, new[]
        {
            new Category(1, "Category 1", "#aaaaaa"),
            new Category(2, "Category 2", "#bbbbbb", true),
            new Category("cat-3", "Category 3", "#cccccc")
        });

        Chart retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);

        Assert.Equal("Untitled chart", retrievedChart.Name);
        var drawing = await Client.Charts.RetrievePublishedVersionAsync(chart.Key);
        var actualCategories = drawing.Categories;

        Assert.Equal(3, actualCategories.Count);

        Assert.Equal(1L, actualCategories[0].Key);
        Assert.Equal("Category 1", actualCategories[0].Label);
        Assert.Equal("#aaaaaa", actualCategories[0].Color);
        Assert.False(actualCategories[0].Accessible);

        Assert.Equal(2L, actualCategories[1].Key);
        Assert.Equal("Category 2", actualCategories[1].Label);
        Assert.Equal("#bbbbbb", actualCategories[1].Color);
        Assert.True(actualCategories[1].Accessible);

        Assert.Equal("cat-3", actualCategories[2].Key);
    }
}