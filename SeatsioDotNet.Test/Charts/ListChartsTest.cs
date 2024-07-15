using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class ListChartsTest : SeatsioClientTest
{
    [Fact]
    public async Task All()
    {
        var chart1 = await Client.Charts.CreateAsync();
        var chart2 = await Client.Charts.CreateAsync();
        var chart3 = await Client.Charts.CreateAsync();

        var charts = Client.Charts.ListAllAsync();

        Assert.Equal(new[] {chart3.Key, chart2.Key, chart1.Key}, charts.Select(c => c.Key));
    }

    [Fact]
    public async Task MultiplePages()
    {
        var charts = new List<Chart>();
        for (var i = 1; i <= 30; i++)
        {
            var chart = await Client.Charts.CreateAsync();
            charts.Add(chart);
        }

        var retrievedCharts = Client.Charts.ListAllAsync();

        Assert.Equal(charts.Select(c => c.Key).Reverse(), retrievedCharts.Select(c => c.Key));
    }

    [Fact]
    public async Task Filter()
    {
        var chart1 = await Client.Charts.CreateAsync("foo");
        var chart2 = await Client.Charts.CreateAsync("bar");
        var chart3 = await Client.Charts.CreateAsync("foofoo");

        var charts = Client.Charts.ListAllAsync(filter: "foo");

        Assert.Equal(new[] {chart3.Key, chart1.Key}, charts.Select(c => c.Key));
    }

    [Fact]
    public async Task Tag()
    {
        var chart1 = await Client.Charts.CreateAsync();
        await Client.Charts.AddTagAsync(chart1.Key, "foo");

        var chart2 = await Client.Charts.CreateAsync();

        var chart3 = await Client.Charts.CreateAsync();
        await Client.Charts.AddTagAsync(chart3.Key, "foo");

        var charts = Client.Charts.ListAllAsync(tag: "foo");

        Assert.Equal(new[] {chart3.Key, chart1.Key}, charts.Select(c => c.Key));
    }

    [Fact]
    public async Task ExpandAll()
    {
        var chart = await Client.Charts.CreateAsync();
        var event1 = await Client.Events.CreateAsync(chart.Key);
        var event2 = await Client.Events.CreateAsync(chart.Key);

        var charts = await Client.Charts.ListAllAsync(expandEvents: true, expandValidation: true, expandVenueType: true).ToListAsync();

        Assert.Equal(new[] {event2.Id, event1.Id}, charts.First().Events.Select(c => c.Id));
        Assert.Equal("MIXED", charts.First().VenueType);
        Assert.NotNull(charts.First().Validation);
    }

    [Fact]
    public async Task ExpandNone()
    {
        var chart = await Client.Charts.CreateAsync();

        var charts = await Client.Charts.ListAllAsync(expandEvents: true).ToListAsync();

        Assert.Empty(charts.First().Events);
        Assert.Null(charts.First().Validation);
        Assert.Null(charts.First().VenueType);
    }
}