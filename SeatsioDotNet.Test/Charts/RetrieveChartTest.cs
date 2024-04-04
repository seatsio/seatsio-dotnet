using System.Linq;
using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class RetrieveChartTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync();
        await Client.Charts.AddTagAsync(chart.Key, "tag1");
        await Client.Charts.AddTagAsync(chart.Key, "tag2");

        Chart retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);

        Assert.NotEqual(0, retrievedChart.Id);
        Assert.NotNull(retrievedChart.Key);
        Assert.Equal("NOT_USED", retrievedChart.Status);
        Assert.Equal("Untitled chart", retrievedChart.Name);
        Assert.NotNull(retrievedChart.PublishedVersionThumbnailUrl);
        Assert.Null(retrievedChart.DraftVersionThumbnailUrl);
        Assert.Null(retrievedChart.Events);
        CustomAssert.ContainsOnly(new[] {"tag1", "tag2"}, retrievedChart.Tags);
        Assert.False(retrievedChart.Archived);
    }

    [Fact]
    public async Task WithEvents()
    {
        var chart = await Client.Charts.CreateAsync();
        var event1 = await Client.Events.CreateAsync(chart.Key);
        var event2 = await Client.Events.CreateAsync(chart.Key);

        Chart retrievedChart = await Client.Charts.RetrieveAsync(chart.Key, expandEvents: true);

        Assert.Equal(new[] {event2.Id, event1.Id}, retrievedChart.Events.Select(e => e.Id));
    }
}