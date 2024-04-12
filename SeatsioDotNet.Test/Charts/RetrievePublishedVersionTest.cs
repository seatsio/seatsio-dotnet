using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class RetrievePublishedVersionTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chart = await Client.Charts.CreateAsync("aChart");

        var drawing = await Client.Charts.RetrievePublishedVersionAsync(chart.Key);
        Assert.Equal("aChart", drawing.Name);
    }
}