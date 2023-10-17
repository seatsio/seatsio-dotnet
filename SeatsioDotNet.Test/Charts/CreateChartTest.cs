using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class CreateChartTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chart = Client.Charts.Create();

        Chart retrievedChart = Client.Charts.Retrieve(chart.Key);

        Assert.NotEqual(0, retrievedChart.Id);
        Assert.NotNull(retrievedChart.Key);
        Assert.Equal("NOT_USED", retrievedChart.Status);
        Assert.Equal("Untitled chart", retrievedChart.Name);
        Assert.NotNull(retrievedChart.PublishedVersionThumbnailUrl);
        Assert.Null(retrievedChart.DraftVersionThumbnailUrl);
        Assert.Null(retrievedChart.Events);
        Assert.Empty(retrievedChart.Tags);
        Assert.False(retrievedChart.Archived);

        var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
        Assert.Equal("MIXED", drawing.VenueType);
        Assert.Empty(drawing.Categories);
    }

    [Fact]
    public void Name()
    {
        var chart = Client.Charts.Create("aChart");

        Chart retrievedChart = Client.Charts.Retrieve(chart.Key);

        Assert.Equal("aChart", retrievedChart.Name);
        var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
        Assert.Equal("aChart", drawing.Name);
        Assert.Empty(drawing.Categories);
    }

    [Fact]
    public void VenueType()
    {
        var chart = Client.Charts.Create(null, "BOOTHS");

        Chart retrievedChart = Client.Charts.Retrieve(chart.Key);

        Assert.Equal("Untitled chart", retrievedChart.Name);
        var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
        Assert.Equal("BOOTHS", drawing.VenueType);
        Assert.Empty(drawing.Categories);
    }

    [Fact]
    public void Categories()
    {
        var chart = Client.Charts.Create(null, null, new[]
        {
            new Category(1, "Category 1", "#aaaaaa"),
            new Category(2, "Category 2", "#bbbbbb", true),
            new Category("cat-3", "Category 3", "#cccccc")
        });

        Chart retrievedChart = Client.Charts.Retrieve(chart.Key);

        Assert.Equal("Untitled chart", retrievedChart.Name);
        var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
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