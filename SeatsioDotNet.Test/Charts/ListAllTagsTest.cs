using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class ListAllTagsTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart1 = await Client.Charts.CreateAsync();
        await Client.Charts.AddTagAsync(chart1.Key, "tag1");
        await Client.Charts.AddTagAsync(chart1.Key, "tag2");

        var chart2 = await Client.Charts.CreateAsync();
        await Client.Charts.AddTagAsync(chart2.Key, "tag3");

        var tags = await Client.Charts.ListAllTagsAsync();
        CustomAssert.ContainsOnly(new[] {"tag1", "tag2", "tag3"}, tags);
    }
}