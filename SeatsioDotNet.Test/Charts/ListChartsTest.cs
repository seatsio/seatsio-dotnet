using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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

        var charts = await Client.Charts.ListAllAsync().ToListAsync();

        Assert.Equal(new[] {chart3.Key, chart2.Key, chart1.Key}, charts.Select(c => c.Key));
    }

    [Fact]
    public async Task MultiplePages()
    {
        var chartTasks = Enumerable.Repeat("", 30).Select(async _ => await Client.Charts.CreateAsync());
        var charts = await Task.WhenAll(chartTasks);

        var retrievedCharts = await Client.Charts.ListAllAsync().ToListAsync();

        charts.Select(c => c.Key).Should().NotContainInConsecutiveOrder(retrievedCharts.Select(c => c.Key));
        // Assert.Equal(charts.Reverse().Select(c => c.Key), retrievedCharts.Select(c => c.Key));
    }

    [Fact]
    public async Task Filter()
    {
        var chart1 = await Client.Charts.CreateAsync("foo");
        var chart2 = await Client.Charts.CreateAsync("bar");
        var chart3 = await Client.Charts.CreateAsync("foofoo");

        var charts = await Client.Charts.ListAllAsync(filter: "foo").ToListAsync();

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

        var charts = await Client.Charts.ListAllAsync(tag: "foo").ToListAsync();

        Assert.Equal(new[] {chart3.Key, chart1.Key}, charts.Select(c => c.Key));
    }

    [Fact]
    public async Task Expand()
    {
        var chart = await Client.Charts.CreateAsync();
        var event1 = await Client.Events.CreateAsync(chart.Key);
        var event2 = await Client.Events.CreateAsync(chart.Key);

        var charts = await Client.Charts.ListAllAsync(expandEvents: true).ToListAsync();

        Assert.Equal(new[] {event2.Id, event1.Id}, charts.First().Events.Select(c => c.Id));
    }   
        
    [Fact]
    public async Task Validation()
    {
        CreateTestChartWithErrors();
            
        var chart = await Client.Charts.ListAllAsync(withValidation: true).FirstAsync();

        Assert.NotNull(chart.Validation);
    }
}