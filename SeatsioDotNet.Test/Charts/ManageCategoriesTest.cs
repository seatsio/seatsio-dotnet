using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class ManageCategoriesTest : SeatsioClientTest
{
    [Fact]
    public async Task AddCategory()
    {
        var chart = await Client.Charts.CreateAsync("aChart", "BOOTHS");

        await Client.Charts.AddCategoryAsync(chart.Key, new Category(1, "cat 1", "#aaaaaa", true));
        var drawing = await Client.Charts.RetrievePublishedVersionAsync(chart.Key);
        Assert.Single(drawing.Categories);
    }

    [Fact]
    public async Task RemoveCategory()
    {
        var categories = new[]
        {
            new Category(1, "Category 1", "#aaaaaa"),
            new Category("cat-2", "Category 2", "#bbbbbb", true)
        };
        var chart = await Client.Charts.CreateAsync("aChart", "BOOTHS", categories);

        await Client.Charts.RemoveCategoryAsync(chart.Key, 1);

        var drawing = await Client.Charts.RetrievePublishedVersionAsync(chart.Key);
        Assert.Single(drawing.Categories);
    }

    [Fact]
    public async Task ListCategories()
    {
        var categories = new[]
        {
            new Category(1, "Category 1", "#aaaaaa"),
            new Category("cat-2", "Category 2", "#bbbbbb", true)
        };
        var chart = await Client.Charts.CreateAsync("aChart", "BOOTHS", categories);

        IEnumerable<Category> categoryList = await Client.Charts.ListCategoriesAsync(chart.Key);
        Assert.Equal(categories, categoryList);
    }
}