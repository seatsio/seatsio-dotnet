using System.Collections.Generic;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class ManageCategoriesTest : SeatsioClientTest
{
        
    [Fact]
    public void AddCategory()
    {
        var chart = Client.Charts.Create("aChart", "BOOTHS");
            
        Client.Charts.AddCategory(chart.Key, new Category(1, "cat 1", "#aaaaaa", true));
        var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
        Assert.Single(drawing.Categories);
    }

    [Fact]
    public void RemoveCategory()
    {
        var categories = new[]
        {
            new Category(1, "Category 1", "#aaaaaa"), 
            new Category("cat-2", "Category 2", "#bbbbbb", true)
        };
        var chart = Client.Charts.Create("aChart", "BOOTHS", categories);
            
        Client.Charts.RemoveCategory(chart.Key, 1);
            
        var drawing = Client.Charts.RetrievePublishedVersion(chart.Key);
        Assert.Single(drawing.Categories);
    }

    [Fact]
    public void ListCategories()
    {
        var categories = new[]
        {
            new Category(1, "Category 1", "#aaaaaa"), 
            new Category("cat-2", "Category 2", "#bbbbbb", true)
        };
        var chart = Client.Charts.Create("aChart", "BOOTHS", categories);
            
        IEnumerable<Category> categoryList = Client.Charts.ListCategories(chart.Key);
        Assert.Equal(categories, categoryList);
    }
}