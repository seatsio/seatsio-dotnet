using System.Threading.Tasks;
using SeatsioDotNet.Charts;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class RemoveTagTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync();
        await Client.Charts.AddTagAsync(chart.Key, "tag1");
        await Client.Charts.AddTagAsync(chart.Key, "tag2");

        await Client.Charts.RemoveTagAsync(chart.Key, "tag2");

        Chart retrievedChart = await Client.Charts.RetrieveAsync(chart.Key);
        CustomAssert.ContainsOnly(new[] {"tag1"}, retrievedChart.Tags);
    }
}