using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class ManageCategoriesTest : SeatsioClientTest
{
    [Fact]
    public async Task AddCategory()
    {
        var chart = await Client.Charts.CreateAsync("aChart", "SIMPLE");

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
        var chart = await Client.Charts.CreateAsync("aChart", "SIMPLE", categories);

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
        var chart = await Client.Charts.CreateAsync("aChart", "SIMPLE", categories);

        IEnumerable<Category> categoryList = await Client.Charts.ListCategoriesAsync(chart.Key);
        Assert.Equal(categories, categoryList);
    }

    [Fact]
    public async Task UpdateCategory()
    {
        var chart = await Client.Charts.CreateAsync("aChart", "SIMPLE");
        await Client.Charts.AddCategoryAsync(chart.Key, new Category(1, "cat 1", "#aaaaaa", false));

        await Client.Charts.UpdateCategoryAsync(chart.Key, 1,
            new CategoryUpdateParams("Updated label", "#bbbbbb", true));
        
        IEnumerable<Category> categoryList = await Client.Charts.ListCategoriesAsync(chart.Key);
        Assert.Equal(new Category(1, "Updated label", "#bbbbbb", true), categoryList.First());
    }
}